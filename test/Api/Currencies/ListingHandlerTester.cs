using Api.Tests.Currencies.Support;

using Mapster;

using NMoneys;
using NMoneys.Api.Currencies;
using NMoneys.Api.Currencies.DataTypes;
using NMoneys.Api.Infrastructure.Mapping;

using Subject = NMoneys.Api.Currencies.ListingHandler;

namespace Api.Tests.Currencies;

[TestFixture]
public class ListingHandlerTester
{
	[OneTimeSetUp]
	public void SetupMapping() => _ = new MappingRegistry();

	[Test]
	public async Task ExecuteAsync_SnapshotsSortedByAscendingAlphabeticCode()
	{
		var subject = new Subject();

		var response = await subject.ExecuteAsync(new ListCurrencies(), CancellationToken.None);

		var byAlphaCode = response.Currencies.Select(c => c.AlphabeticCode);
		Assert.That(byAlphaCode, Is.Ordered.Ascending);
	}
	
	[Test]
	public async Task ExecuteAsync_AllMappedCurrenciesReturned()
	{
		var subject = new Subject();
		
		var response = await subject.ExecuteAsync(new ListCurrencies(), CancellationToken.None);

		var allCurrencies = Currency.FindAll().Count();
		Assert.That(response.Currencies, Has.Length.EqualTo(allCurrencies));
	}
	
	[Test]
	public async Task ExecuteAsync_ObsoleteCurrenciesAlsoReturned()
	{
		var subject = new Subject();
		
		var response = await subject.ExecuteAsync(new ListCurrencies(), CancellationToken.None);
		
		Assert.That(response.Currencies, Has.Some.Obsolete());
	}
}