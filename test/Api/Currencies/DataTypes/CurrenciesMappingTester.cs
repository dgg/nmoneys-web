using Mapster;

using NMoneys;
using NMoneys.Api.Currencies.DataTypes;

namespace Api.Tests.Currencies.DataTypes;

[TestFixture]
public class CurrenciesMappingTester
{
	private TypeAdapterConfig _config;

	[OneTimeSetUp]
	public void SetupMapping()
	{
		_config = new TypeAdapterConfig();
		new CurrenciesMapping().Register(_config);
		// catch errors
		_config.Compile();
	}

	#region Currency -> CurrencyCode

	[Test]
	public void Adapt_CurrencyToCode_MapsProps()
	{
		var result = Currency.Eur.Adapt<CurrencyCode>(_config);
		Assert.Multiple(() =>
		{
			Assert.That(result.Alphabetic, Is.EqualTo("EUR"));
			Assert.That(result.Numeric, Is.EqualTo(978));
		});
	}

	#endregion

	#region Currency -> CurrencyName

	[Test]
	public void Adapt_CurrencyToName_MapsProps()
	{
		var result = Currency.Chf.Adapt<CurrencyName>(_config);

		Assert.Multiple(() =>
		{
			Assert.That(result.English, Is.EqualTo("Swiss Franc"));
			Assert.That(result.Native, Is.EqualTo("Schweizer Franken"));
		});
	}

	[Test]
	public void Adapt_CurrencyToSameNativeName_SameProps()
	{
		var result = Currency.Aud.Adapt<CurrencyName>(_config);

		Assert.That(result.English, Is.EqualTo(result.Native).And
			.EqualTo("Australian Dollar"));
	}

	#endregion

	#region Currency -> CurrencySnapshot

	[Test]
	public void Adapt_CurrencyToSnapshot_MapsCodes()
	{
		var result = Currency.Eur.Adapt<CurrencySnapshot>(_config);

		Assert.That(result.Code, Is.Not.Null);
		Assert.Multiple(() =>
		{
			Assert.That(result.Code.Alphabetic, Is.EqualTo("EUR"));
			Assert.That(result.Code.Numeric, Is.EqualTo(978));
		});
	}

	[Test]
	public void Adapt_CurrencyToSnapshot_MapsNames()
	{
		var result = Currency.Chf.Adapt<CurrencySnapshot>(_config);

		Assert.That(result.Name, Is.Not.Null);
		using (Assert.EnterMultipleScope())
		{
			Assert.That(result.Name.English, Is.EqualTo("Swiss Franc"));
			Assert.That(result.Name.Native, Is.EqualTo("Schweizer Franken"));
		}
	}

	[Test]
	public void Adapt_ObsoleteCurrencyToSnapshot_True()
	{
#pragma warning disable CS0618 // Type or member is obsolete
		var obsolete = Currency.Get(CurrencyIsoCode.CUC); // CUC (Cuban Convertible Peso) is obsolete
#pragma warning restore CS0618

		var result = obsolete.Adapt<CurrencySnapshot>(_config);
		
		Assert.That(result.IsObsolete, Is.True);
	}

	[Test]
	public void Adapt_NonObsoleteCurrencyToSnapshot_Null()
	{
		var legalTender = Currency.Aud; // Australian Dollar is not obsolete

		var result = legalTender.Adapt<CurrencySnapshot>(_config);
		
		Assert.That(result.IsObsolete, Is.Null);
	}

	#endregion
}
