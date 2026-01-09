using Api.Tests.Currencies.DataTypes.Mappings.Support;

using Mapster;

using NMoneys;
using NMoneys.Api.Currencies.DataTypes;
using NMoneys.Api.Currencies.DataTypes.Mappings;

namespace Api.Tests.Currencies.DataTypes.Mappings;

[TestFixture]
public class CurrencyNamesMappingTester : MappingTesterBase
{
	protected override IRegister[] Mappings => [new CurrencyNameMapping()];

	[Test]
	public void Adapt_Names_MapsProps()
	{
		var result = Currency.Chf.Adapt<CurrencyNames>(Config);

		Assert.Multiple(() =>
		{
			Assert.That(result.English, Is.EqualTo("Swiss Franc"));
			Assert.That(result.Native, Is.EqualTo("Schweizer Franken"));
		});
	}

	[Test]
	public void Adapt_SameNativeName_SameProps()
	{
		var result = Currency.Aud.Adapt<CurrencyNames>(Config);

		Assert.That(result.English, Is.EqualTo(result.Native).And
			.EqualTo("Australian Dollar"));
	}
}