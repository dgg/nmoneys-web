using System.Net;
using System.Text.Json;

using Alba;

using Api.Tests.Currencies.Support;

using NMoneys;

namespace Api.Tests.Currencies;

[TestFixture, Category("Integration")]
public class CurrenciesTester
{
	[Test , CancelAfter(2000)]
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
	
	[Test , CancelAfter(2000)]
	public async Task CurrencyRetrieval_InvalidCode_BadRequest(CancellationToken ct)
	{
		string nonAnIsoCode = "LOL";
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Get.Url($"/currencies/{nonAnIsoCode}");
			x.StatusCodeShouldBe(HttpStatusCode.BadRequest);
		});
		using var response = await result.ReadAsJsonAsync<JsonDocument>();
		Assert.That(response, Is.Not.Null);
		// TODO: assert problem
	}
	
	[Test , CancelAfter(2000)]
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
}