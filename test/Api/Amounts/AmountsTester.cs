using System.Net;
using System.Text.Json;

using Alba;

using Api.Tests.Support.Auth;

using FastEndpoints;

using NMoneys;
using NMoneys.Api.Amounts;
using NMoneys.Api.Infrastructure.Auth;

namespace Api.Tests.Amounts;

[TestFixture, Category("Integration")]
public class AmountsTester
{
	#region AmountsFormatting

	[Test, CancelAfter(2000)]
	public async Task AmountsFormatting_Anonymous_Unauthorized(CancellationToken ct) =>
		_ = await TestWebApp.Host.Scenario(x =>
		{
			x.Anonymous();
			x.Post.Json(new { currency_code = CurrencyIsoCode.XTS, amounts = new[] { 0m } }).ToUrl($"/amounts/formats");
			x.StatusCodeShouldBe(HttpStatusCode.Unauthorized);
		});

	[Test, CancelAfter(2000)]
	public async Task AmountsFormatting_MissingScope_Forbidden(CancellationToken ct) =>
		_ = await TestWebApp.Host.Scenario(x =>
		{
			x.WithoutScope();
			x.Post.Json(new { currency_code = CurrencyIsoCode.XTS, amounts = new[] { 0m } }).ToUrl($"/amounts/formats");
			x.StatusCodeShouldBe(HttpStatusCode.Forbidden);
		});

	[Test, CancelAfter(2000)]
	public async Task AmountsFormatting_CodeAmounts_Success(CancellationToken ct)
	{
		var multipleAmounts = new { amounts = new[] { -123.45m, 999.999m }, alphabetic_code = "DKK" };
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.WithScopes(AuthorizationScope.FormatAmounts);
			x.Post.Json(multipleAmounts).ToUrl($"/amounts/formats");
			x.StatusCodeShouldBeOk();
		});
		using var response = await result.ReadAsJsonAsync<JsonDocument>();

		Assert.That(response, Is.Not.Null);
		JsonElement formatted = assertTopLevelFormatted(response);
		// comma-separated decimals and round up to 2 decimals
		assertFormatted(formatted, ["-123,45 kr.", "1.000,00 kr."]);
	}

	[Test, CancelAfter(2000)]
	public async Task AmountsFormatting_SingleAmount_Success(CancellationToken ct)
	{
		var singleAmount = new { amounts = new[] { -123.45m }, alphabetic_code = "USD" };
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.WithScopes(AuthorizationScope.FormatAmounts);
			x.Post.Json(singleAmount).ToUrl($"/amounts/formats");
			x.StatusCodeShouldBeOk();
		});
		using var response = await result.ReadAsJsonAsync<JsonDocument>();

		Assert.That(response, Is.Not.Null);
		JsonElement formatted = assertTopLevelFormatted(response);
		// dot-separated decimals
		assertFormatted(formatted, ["-$123.45"]);
	}

	[Test, CancelAfter(2000)]
	public async Task AmountsFormatting_InvalidCode_BadRequest(CancellationToken ct)
	{
		var nonIsoCode = new { amounts = new[] { -123.45m }, alphabetic_code = "LOL" };
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.WithScopes(AuthorizationScope.FormatAmounts);
			x.Post.Json(nonIsoCode).ToUrl($"/amounts/formats");
			x.StatusCodeShouldBe(HttpStatusCode.BadRequest);
		});

		var problem = await result.ReadAsJsonAsync<ProblemDetails>();
		Assert.That(problem, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(problem.Status, Is.EqualTo(400));
			Assert.That(problem.Title, Is.EqualTo("Bad Request"));
			Assert.That(problem.Detail, Does.Contain(nameof(AmountsFormattingRequest)),
				"different message");
			var errors = problem.Errors.ToArray();
			Assert.That(errors, Has.Length.EqualTo(1));
			Assert.That(errors[0].Name, Is.EqualTo("alphabetic_code"));
			Assert.That(errors[0].Reason, Does.Contain(nameof(AmountsFormattingRequest)));
		});
	}

	[Test, CancelAfter(2000)]
	public async Task AmountsFormatting_WrongCasing_BadRequest(CancellationToken ct)
	{
		var lowercase = new[] { new { amounts = new[] { 0m }, alphabetic_code = "dkk" } };
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.WithScopes(AuthorizationScope.FormatAmounts);
			x.Post.Json(lowercase).ToUrl($"/amounts/formats");
			x.StatusCodeShouldBe(HttpStatusCode.BadRequest);
		});
		var problem = await result.ReadAsJsonAsync<ProblemDetails>();
		Assert.That(problem, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(problem.Status, Is.EqualTo(400));
			Assert.That(problem.Title, Is.EqualTo("Bad Request"));
			Assert.That(problem.Detail, Does.Contain(nameof(AmountsFormattingRequest)),
				"different message");
			var errors = problem.Errors.ToArray();
			Assert.That(errors, Has.Length.EqualTo(1));
			Assert.That(errors[0].Name, Is.EqualTo("serializer_errors"), "not attached to property");
			Assert.That(errors[0].Reason, Does.Contain(nameof(AmountsFormattingRequest)));
		});
	}

	[Test, CancelAfter(2000)]
	public async Task AmountsFormatting_EmptyAmounts_BadRequest(CancellationToken ct)
	{
		var emptyAmounts = new { amounts = Array.Empty<decimal>(), alphabetic_code = "XTS" };
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.WithScopes(AuthorizationScope.FormatAmounts);
			x.Post.Json(emptyAmounts).ToUrl("/amounts/formats");
			x.StatusCodeShouldBe(HttpStatusCode.BadRequest);
		});
		var problem = await result.ReadAsJsonAsync<ProblemDetails>();
		Assert.That(problem, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(problem.Status, Is.EqualTo(400));
			Assert.That(problem.Title, Is.EqualTo("Bad Request"));
			Assert.That(problem.Detail, Does.Contain("amounts").And.Contain("empty"));
			var errors = problem.Errors.ToArray();
			Assert.That(errors, Has.Length.EqualTo(1));
			Assert.That(errors[0].Name, Is.EqualTo("amounts"), "not attached to property");
			Assert.That(errors[0].Reason, Does.Contain("amounts").And.Contain("empty"));
		});
	}

	private static JsonElement assertTopLevelFormatted(JsonDocument response)
	{
		Assert.That(response, Is.Not.Null);

		Assert.That(response.RootElement.ValueKind, Is.EqualTo(JsonValueKind.Object),
			"top-level object");
		Assert.That(response.RootElement.TryGetProperty("formatted", out var formatted), Is.True);
		Assert.That(formatted.ValueKind, Is.EqualTo(JsonValueKind.Array), "formatted{}");
		return formatted;
	}

	private static void assertFormatted(JsonElement formatted, string[] expected)
	{
		Assert.That(formatted.GetArrayLength(), Is.EqualTo(expected.Length));
		Assert.Multiple(() =>
		{
			for (int i = 0; i < expected.Length; i++)
			{
				Assert.That(formatted[i].GetString(), Is.EqualTo(expected[i]));
			}
		});
	}

	#endregion
}