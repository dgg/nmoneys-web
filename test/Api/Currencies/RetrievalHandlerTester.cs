using Microsoft.Testing.Platform.Requests;

using NMoneys;
using NMoneys.Api.Currencies;
using NMoneys.Api.Currencies.DataTypes;
using NMoneys.Api.Infrastructure.Mapping;

using Subject = NMoneys.Api.Currencies.RetrievalHandler;

namespace Api.Tests.Currencies;

[TestFixture]
public class RetrievalHandlerTester
{
	[OneTimeSetUp]
	public void SetupMapping() => _ = new MappingRegistry();

	[Test]
	public async Task ExecuteAsync_ReturnsDetail()
	{
		var subject = new Subject();

		var response = await subject.ExecuteAsync(new RetrieveCurrency(CurrencyIsoCode.CHF), CancellationToken.None);

		Assert.That(response, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(response.Currency, Is.Not.Null);
			Assert.That(response.Currency.Codes, Is.EqualTo(new CurrencyCodes("CHF", 756, "756")));
			Assert.That(response.Currency.Names,
				Is.EqualTo(new CurrencyNames("Swiss Franc", "Schweizer Franken")));
			Assert.That(response.Currency.Symbol, Is.EqualTo("Fr."));
			Assert.That(response.Currency.SignificantDigits, Is.EqualTo(2));
			Assert.That(response.Currency.IsObsolete, Is.False);
		});
	}

	[Test]
	public async Task ExecuteAsync_ObsoleteCurrency_ReturnsWithObsoleteFlag()
	{
		var subject = new Subject();

#pragma warning disable CS0618 // Type or member is obsolete
		var response = await subject.ExecuteAsync(new RetrieveCurrency(CurrencyIsoCode.CUC), CancellationToken.None);
#pragma warning restore CS0618

		Assert.That(response, Is.Not.Null);
		Assert.That(response.Currency.IsObsolete, Is.True);
	}

	[Test]
	public async Task ExecuteAsync_NotDefinedCurrency_Exception()
	{
		var subject = new Subject();
		CurrencyIsoCode notDefined = (CurrencyIsoCode) 1000;
		await Assert.ThatAsync(
			async () => await subject.ExecuteAsync(new RetrieveCurrency(notDefined), CancellationToken.None),
			Throws.InstanceOf<UndefinedCodeException>());
	}
}
