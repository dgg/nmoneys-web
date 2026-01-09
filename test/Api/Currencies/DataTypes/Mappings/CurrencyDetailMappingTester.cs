using Api.Tests.Currencies.DataTypes.Mappings.Support;

using Mapster;

using NMoneys;
using NMoneys.Api.Currencies.DataTypes;
using NMoneys.Api.Currencies.DataTypes.Mappings;

namespace Api.Tests.Currencies.DataTypes.Mappings;

[TestFixture]
public class CurrencyDetailMappingTester : MappingTesterBase
{
	protected override IRegister[] Mappings =>
		[new CurrencyDetailMapping(), new CurrencyNameMapping(), new ExtendedCodesMapping()];

	[Test]
	public void Adapt_Detail_MapsCodes()
	{
		var result = Currency.Aud.Adapt<CurrencyDetail>(Config);

		Assert.That(result.Code, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(result.Code.Alphabetic, Is.EqualTo("AUD"));
			Assert.That(result.Code.Numeric, Is.EqualTo(36));
			Assert.That(result.Code.Padded, Is.EqualTo("036"));
		});
	}
	
	[Test]
	public void Adapt_Detail_MapsNames()
	{
		var result = Currency.Chf.Adapt<CurrencyDetail>(Config);

		Assert.That(result.Name, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(result.Name.English, Is.EqualTo("Swiss Franc"));
			Assert.That(result.Name.Native, Is.EqualTo("Schweizer Franken"));
		});
	}

	[Test]
	public void Adapt_Detail_MapsSymbol()
	{
		var result = Currency.Usd.Adapt<CurrencyDetail>(Config);

		Assert.That(result.Symbol, Is.EqualTo("$"));
	}

	[Test]
	public void Adapt_NoFractions_ZeroSignificantDigits()
	{
		// JPY has no fractions
		var result = Currency.Jpy.Adapt<CurrencyDetail>(Config);

		Assert.That(result.SignificantDigits, Is.Zero);
	}
	[Test]
	public void Adapt_Fractions_NonSignificantDigits()
	{
		// EUR has cents
		var result = Currency.Eur.Adapt<CurrencyDetail>(Config);

		Assert.That(result.SignificantDigits, Is.EqualTo(2));
	}

	[Test]
	public void Adapt_ObsoleteCurrency_IsObsolete()
	{
#pragma warning disable CS0618 // Type or member is obsolete
		var obsolete = Currency.Get(CurrencyIsoCode.CUC); // CUC (Cuban Convertible Peso) is obsolete
#pragma warning restore CS0618

		var result = obsolete.Adapt<CurrencyDetail>(Config);

		Assert.That(result.IsObsolete, Is.True);
	}

	[Test]
	public void Adapt_NonObsoleteCurrency_False()
	{
		var legalTender = Currency.Eur; // Euro is not obsolete

		var result = legalTender.Adapt<CurrencyDetail>(Config);

		Assert.That(result.IsObsolete, Is.False);
	}
}