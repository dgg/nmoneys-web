using System.Net;
using System.Text.Json;

using Alba;

using Api.Tests.Currencies.Support;

using FastEndpoints;

using NMoneys;
using NMoneys.Api.Currencies;

namespace Api.Tests.Currencies;

[TestFixture, Category("Integration")]
public class CurrenciesTester
{
	#region CurrenciesListing

	[Test, CancelAfter(2000)]
	public async Task CurrenciesListing_ReturnsAllCurrenciesSorted(CancellationToken ct)
	{
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Get.Url("/currencies");
			x.StatusCodeShouldBeOk();
		});
		using var response = await result.ReadAsJsonAsync<JsonDocument>();

		JsonElement currencies = assertTopLevelCurrencies(response);
		assertAllCurrencies(currencies);
		assertSortedCurrencies(currencies);
		assertSnapshotProps(currencies[0]);
	}

	private static JsonElement assertTopLevelCurrencies(JsonDocument response)
	{
		Assert.That(response, Is.Not.Null);

		Assert.That(response.RootElement.ValueKind, Is.EqualTo(JsonValueKind.Object),
			"top-level object");
		Assert.That(response.RootElement.TryGetProperty("currencies", out var currencies), Is.True);
		Assert.That(currencies.ValueKind, Is.EqualTo(JsonValueKind.Array), "currencies[]");
		return currencies;
	}

	private static void assertSortedCurrencies(JsonElement currencies)
	{
		var alphaCodes = currencies.EnumerateArray()
			.Select(c => c.GetProperty("alphabetic_code"))
			.Select(c => c.GetString()!);
		Assert.That(alphaCodes, Is.Ordered.Ascending,
			"sorted by alphabetic code");
	}

	private static void assertAllCurrencies(JsonElement currencies)
	{
		int currencyCount = Currency.FindAll().Count();
		Assert.That(currencies.GetArrayLength(), Is.EqualTo(currencyCount), "all currencies");
	}

	private static void assertSnapshotProps(JsonElement snapshot)
	{
		Assert.That(snapshot, Haz.Prop("alphabetic_code"));
		Assert.That(snapshot, Haz.Prop("english_name"));
	}

	#endregion

	#region CurrencyRetrieval

	[Test, CancelAfter(2000)]
	public async Task CurrencyRetrieval_InvalidCode_BadRequest(CancellationToken ct)
	{
		string nonAnIsoCode = "LOL";
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Get.Url($"/currencies/{nonAnIsoCode}");
			x.StatusCodeShouldBe(HttpStatusCode.BadRequest);
		});
		var problem = await result.ReadAsJsonAsync<ProblemDetails>();
		Assert.That(problem, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(problem.Status, Is.EqualTo(400));
			Assert.That(problem.Title, Is.EqualTo("Bad Request"));
			Assert.That(problem.Detail, Does.Contain(nonAnIsoCode).And
				.Contains(nameof(CurrencyIsoCode)));
			var errors = problem.Errors.ToArray();
			Assert.That(errors, Has.Length.EqualTo(1));
			Assert.That(errors[0].Name, Is.EqualTo("alphabetic_code"));
			Assert.That(errors[0].Reason, Does.Contain(nonAnIsoCode).And
				.Contains(nameof(CurrencyIsoCode)));
		});
	}

	[Test, CancelAfter(2000)]
	public async Task CurrencyRetrieval_ValidCode_Details(CancellationToken ct)
	{
		string isoCode = "DKK";
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Get.Url($"/currencies/{isoCode}");
			x.StatusCodeShouldBeOk();
		});
		using var response = await result.ReadAsJsonAsync<JsonDocument>();
		Assert.That(response, Is.Not.Null);
		JsonElement currency = assertTopLevelCurrency(response);
		assertExtendedCode(currency);
		assertName(currency);
		assertDetailProps(currency);
	}

	[Test, CancelAfter(2000)]
	public async Task CurrencyRetrieval_CaseInsensitive_Details(CancellationToken ct)
	{
		string isoCode = "dkk";
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Get.Url($"/currencies/{isoCode}");
			x.StatusCodeShouldBeOk();
		});
		using var response = await result.ReadAsJsonAsync<JsonDocument>();
		Assert.That(response, Is.Not.Null);
		assertTopLevelCurrency(response);
	}

	private static JsonElement assertTopLevelCurrency(JsonDocument response)
	{
		Assert.That(response, Is.Not.Null);

		Assert.That(response.RootElement.ValueKind, Is.EqualTo(JsonValueKind.Object),
			"top-level object");
		Assert.That(response.RootElement.TryGetProperty("currency", out var currency), Is.True);
		Assert.That(currency.ValueKind, Is.EqualTo(JsonValueKind.Object), "currency{}");
		return currency;
	}

	private static void assertExtendedCode(JsonElement currency)
	{
		var codes = currency.GetProperty("codes");
		Assert.That(codes.ValueKind, Is.EqualTo(JsonValueKind.Object), "currency.codes{}");
		Assert.That(codes, Haz.Prop("alphabetic"));
		Assert.That(codes, Haz.Prop("numeric"));
		Assert.That(codes, Haz.Prop("padded"));
	}

	private static void assertName(JsonElement currency)
	{
		var names = currency.GetProperty("names");
		Assert.That(names.ValueKind, Is.EqualTo(JsonValueKind.Object), "currency.names{}");
		Assert.That(names, Haz.Prop("english"));
		Assert.That(names, Haz.Prop("native"));
	}

	private static void assertDetailProps(JsonElement currency)
	{
		Assert.That(currency, Haz.Prop("symbol"));
		Assert.That(currency, Haz.Prop("significant_digits"));
		Assert.That(currency, Haz.Prop("is_obsolete"));
	}

	#endregion

	#region AmountsFormatting

	[Test, CancelAfter(2000)]
	public async Task AmountsFormatting_CodeAmounts_Success(CancellationToken ct)
	{
		string alphabeticCode = "DKK";
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Post.Json(new { amounts = new[] { -123.45m, 999.999m } }).ToUrl($"/currencies/{alphabeticCode}/formats");
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
		string alphabeticCode = "USD";
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Post.Json(new { amounts = new[] { -123.45m } }).ToUrl($"/currencies/{alphabeticCode}/formats");
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
		string nonIsoCode = "LOL";
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Post.Json(new[] { 0m }, JsonStyle.MinimalApi).ToUrl($"/currencies/{nonIsoCode}/formats");
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
	public async Task AmountsFormatting_WrongCasing_BadRequest(CancellationToken ct)
	{
		string alphabeticCode = "dkk";
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Post.Json(new[] { 0m }, JsonStyle.MinimalApi).ToUrl($"/currencies/{alphabeticCode}/formats");
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
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Post.Json(new { amounts = Array.Empty<decimal>() }).ToUrl("/currencies/CHF/formats");
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