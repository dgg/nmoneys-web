using System.Text.Json;

using Alba;

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
			.Select(c => c.GetProperty("code"))
			.Select(c => c.GetProperty("alphabetic"))
			.Select(c => c.GetString()!);
		Assert.That(alphaCodes, Is.Ordered.Ascending, 
			"sorted by alphabetic code");
	}

	private static void assertAllCurrencies(JsonElement currencies)
	{
		int currencyCount = Currency.FindAll().Count();
		Assert.That(currencies.GetArrayLength(), Is.EqualTo(currencyCount), "all currencies");
	}
}