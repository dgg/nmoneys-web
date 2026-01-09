using Api.Tests.Currencies.DataTypes.Mappings.Support;

using Mapster;

using NMoneys;
using NMoneys.Api.Currencies.DataTypes;
using NMoneys.Api.Currencies.DataTypes.Mappings;

namespace Api.Tests.Currencies.DataTypes.Mappings;

[TestFixture]
public class CurrencySnapshotMappingTester : MappingTesterBase
{
	protected override IRegister[] Mappings =>
		[new CurrencySnapshotMapping(), new CurrencyNameMapping(), new SnapshotCodesMapping()];

	[Test]
	public void Adapt_Snapshot_MapsCodes()
	{
		var result = Currency.Eur.Adapt<CurrencySnapshot>(Config);

		Assert.That(result.Code, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(result.Code.Alphabetic, Is.EqualTo("EUR"));
			Assert.That(result.Code.Numeric, Is.EqualTo(978));
		});
	}

	[Test]
	public void Adapt_Snapshot_MapsNames()
	{
		var result = Currency.Chf.Adapt<CurrencySnapshot>(Config);

		Assert.That(result.Name, Is.Not.Null);
		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.Name.English, Is.EqualTo("Swiss Franc"));
			Assert.That(result.Name.Native, Is.EqualTo("Schweizer Franken"));
		}
	}

	[Test]
	public void Adapt_ObsoleteCurrency_IsObsolete()
	{
#pragma warning disable CS0618 // Type or member is obsolete
		var obsolete = Currency.Get(CurrencyIsoCode.CUC); // CUC (Cuban Convertible Peso) is obsolete
#pragma warning restore CS0618

		var result = obsolete.Adapt<CurrencySnapshot>(Config);

		Assert.That(result.IsObsolete, Is.True);
	}

	[Test]
	public void Adapt_NonObsoleteCurrency_NullObsolete()
	{
		var legalTender = Currency.Aud; // Australian Dollar is not obsolete

		var result = legalTender.Adapt<CurrencySnapshot>(Config);

		Assert.That(result.IsObsolete, Is.Null);
	}
}