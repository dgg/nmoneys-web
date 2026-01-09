using Api.Tests.Currencies.DataTypes.Mappings.Support;

using Mapster;

using NMoneys;
using NMoneys.Api.Currencies.DataTypes;
using NMoneys.Api.Currencies.DataTypes.Mappings;

namespace Api.Tests.Currencies.DataTypes.Mappings;

[TestFixture]
public class ExtendedCodesMappingTester : MappingTesterBase
{
	protected override IRegister[] Mappings => [new ExtendedCodesMapping()];

	[Test]
	public void Adapt_ThreeFigureCodeCurrency_MapsProps()
	{
		var result = Currency.Eur.Adapt<ExtendedCodes>(Config);
		Assert.Multiple(() =>
		{
			Assert.That(result.Alphabetic, Is.EqualTo("EUR"));
			Assert.That(result.Numeric, Is.EqualTo(978));
			Assert.That(result.Padded, Is.EqualTo("978"));
		});
	}

	[Test]
	public void Adapt_TwoFigureCodeCurrency_MapsProps()
	{
		var result = Currency.Aud.Adapt<ExtendedCodes>(Config);
		Assert.Multiple(() =>
		{
			Assert.That(result.Alphabetic, Is.EqualTo("AUD"));
			Assert.That(result.Numeric, Is.EqualTo(36));
			Assert.That(result.Padded, Is.EqualTo("036"));
		});
	}
}