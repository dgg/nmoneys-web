using Api.Tests.Currencies.DataTypes.Mappings.Support;

using Mapster;

using NMoneys;
using NMoneys.Api.Currencies.DataTypes;
using NMoneys.Api.Currencies.DataTypes.Mappings;

namespace Api.Tests.Currencies.DataTypes.Mappings;

[TestFixture]
public class SnapshotCodesMappingTester : MappingTesterBase
{
	protected override IRegister[] Mappings => [new SnapshotCodesMapping()];

	[Test]
	public void Adapt_CurrencyToCode_MapsProps()
	{
		var result = Currency.Eur.Adapt<SnapshotCodes>(Config);
		Assert.Multiple(() =>
		{
			Assert.That(result.Alphabetic, Is.EqualTo("EUR"));
			Assert.That(result.Numeric, Is.EqualTo(978));
		});
	}
}