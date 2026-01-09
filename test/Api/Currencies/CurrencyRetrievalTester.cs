using System.Text.Json;

using Alba;

using NMoneys;

namespace Api.Tests.Currencies;

[TestFixture, Category("Integration")]
public class CurrencyRetrievalTester
{
	[Test, CancelAfter(2000)]
	public async Task CurrencyRetrieval_ValidCode_Returns200WithDetails(CancellationToken ct)
	{
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Get.Url("/currencies/CHF");
			x.StatusCodeShouldBeOk();
		});
		using var response = await result.ReadAsJsonAsync<JsonDocument>();

		Assert.That(response, Is.Not.Null);
		Assert.That(response.RootElement.ValueKind, Is.EqualTo(JsonValueKind.Object));
		Assert.That(response.RootElement.TryGetProperty("currency", out var currency), Is.True);

		Assert.Multiple(() =>
		{
			Assert.That(currency.TryGetProperty("codes", out var codes), Is.True);
			Assert.That(codes.GetProperty("alphabetic").GetString(), Is.EqualTo("CHF"));
			Assert.That(codes.GetProperty("numeric").GetInt32(), Is.EqualTo(756));

			Assert.That(currency.TryGetProperty("names", out var names), Is.True);
			Assert.That(names.GetProperty("english").GetString(), Is.EqualTo("Swiss Franc"));
			Assert.That(names.GetProperty("native").GetString(), Is.EqualTo("Schweizer Franken"));

			Assert.That(currency.TryGetProperty("symbol", out var symbol), Is.True);
			Assert.That(symbol.GetString(), Is.EqualTo("Fr."));

			Assert.That(currency.TryGetProperty("significant_digits", out var digits), Is.True);
			Assert.That(digits.GetInt32(), Is.EqualTo(2));
		});
	}

	[Test, CancelAfter(2000)]
	public async Task CurrencyRetrieval_InvalidCode_Returns400(CancellationToken ct)
	{
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Get.Url("/currencies/INVALID");
			x.StatusCodeShouldBe(400);
		});
	}

	[Test, CancelAfter(2000)]
	public async Task CurrencyRetrieval_CaseInsensitive_Returns200(CancellationToken ct)
	{
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Get.Url("/currencies/eur");
			x.StatusCodeShouldBeOk();
		});
		using var response = await result.ReadAsJsonAsync<JsonDocument>();

		Assert.That(response, Is.Not.Null);
		Assert.That(response.RootElement.TryGetProperty("currency", out var currency), Is.True);
		Assert.That(currency.GetProperty("codes").GetProperty("alphabetic").GetString(), Is.EqualTo("EUR"));
	}

	[Test, CancelAfter(2000)]
	public async Task CurrencyRetrieval_ObsoleteCurrency_Obsolete(CancellationToken ct)
	{
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Get.Url("/currencies/CUC");
			x.StatusCodeShouldBeOk();
		});
		using var response = await result.ReadAsJsonAsync<JsonDocument>();

		Assert.That(response, Is.Not.Null);
		Assert.That(response.RootElement.TryGetProperty("currency", out var currency), Is.True);
		Assert.That(currency.TryGetProperty("is_obsolete", out var isObsolete), Is.True);
		Assert.That(isObsolete.GetBoolean(), Is.True);
	}

	[Test, CancelAfter(2000)]
	public async Task CurrencyRetrieval_ResponseStructure_MatchesContract(CancellationToken ct)
	{
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Get.Url("/currencies/EUR");
			x.StatusCodeShouldBeOk();
		});
		using var response = await result.ReadAsJsonAsync<JsonDocument>();

		Assert.That(response, Is.Not.Null);
		var root = response.RootElement;

		// Top-level must be an object with "currency" property
		Assert.That(root.ValueKind, Is.EqualTo(JsonValueKind.Object));
		Assert.That(root.TryGetProperty("currency", out var currency), Is.True);

		// Currency must have required properties
		Assert.That(currency.TryGetProperty("codes", out _), Is.True);
		Assert.That(currency.TryGetProperty("names", out _), Is.True);
		Assert.That(currency.TryGetProperty("symbol", out _), Is.True);
		Assert.That(currency.TryGetProperty("significant_digits", out _), Is.True);
		Assert.That(currency.TryGetProperty("is_obsolete", out _), Is.True);
	}
}
