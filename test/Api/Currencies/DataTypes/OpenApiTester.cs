using Alba;

using NJsonSchema;

using NMoneys;
using NMoneys.Api.Currencies;
using NMoneys.Api.Currencies.DataTypes;

using NSwag;

namespace Api.Tests.Currencies.DataTypes;

[TestFixture, Category("Integration")]
public class OpenApiTester
{
	private OpenApiDocument _openApi;
	[OneTimeSetUp]
	public async Task FetchOpenApi()
	{
		var result = await TestWebApp.Host.Scenario(x =>
		{
			x.Get.Url("/swagger/v1/swagger.json");
			x.StatusCodeShouldBeOk();
		});
		var response = await result.ReadAsTextAsync();
		_openApi = await OpenApiDocument.FromJsonAsync(response);
	}
	
	#region SnapshotCodes
	
	[Test]
	public void SnapshotCodes_RequiredProps()
	{
		JsonSchema currencyCode = _openApi.Components.Schemas[nameof(SnapshotCodes)];
		
		Assert.That(currencyCode.Type, Is.EqualTo(JsonObjectType.Object));
		Assert.That(currencyCode.AllowAdditionalProperties, Is.False);
		Assert.That(currencyCode.RequiredProperties, Is.EquivalentTo(["alphabetic", "numeric"]));
		Assert.That(currencyCode.Description, Is.Not.Null);
	}
	
	[Test]
	public void SnapshotCodes_Alphabetic_Required3CharString()
	{
		JsonSchemaProperty alphabetic = _openApi.Components
			.Schemas[nameof(SnapshotCodes)]
			.Properties[nameof(alphabetic)];
		
		Assert.Multiple(() =>
		{
			Assert.That(alphabetic.Type, Is.EqualTo(JsonObjectType.String));
			Assert.That(alphabetic.IsRequired, Is.True);
			Assert.That(alphabetic.Description, Is.Not.Null);
			Assert.That(alphabetic.MinLength, Is.EqualTo(3));
			Assert.That(alphabetic.MaxLength, Is.EqualTo(3));
			Assert.That(alphabetic.Example, Is.Not.Null.And
				.EqualTo(CurrencySnapshot.Example.Code.Alphabetic));
		});
	}
	
	[Test]
	public void SnapshotCodes_Numeric_RequiredConstrainedInteger()
	{
		JsonSchemaProperty numeric = _openApi.Components
			.Schemas[nameof(SnapshotCodes)]
			.Properties[nameof(numeric)];
		using (Assert.EnterMultipleScope())
		{
			Assert.That(numeric.Type, Is.EqualTo(JsonObjectType.Integer));
			Assert.That(numeric.IsRequired, Is.True);
			Assert.That(numeric.Description, Is.Not.Null);
			Assert.That(numeric.Minimum, Is.Zero);
			Assert.That(numeric.Maximum, Is.EqualTo(999));
			Assert.That(numeric.Example, Is.Not.Null.And
				.EqualTo(CurrencySnapshot.Example.Code.Numeric));
		}
	}
	
	#endregion
	
	#region CurrencyNames
	
	[Test]
	public void CurrencyNames_RequiredProps()
	{
		JsonSchema currencyName = _openApi.Components.Schemas[nameof(CurrencyNames)];
		
		Assert.That(currencyName.Type, Is.EqualTo(JsonObjectType.Object));
		Assert.That(currencyName.AllowAdditionalProperties, Is.False);
		Assert.That(currencyName.RequiredProperties, Is.EquivalentTo(["english", "native"]));
		Assert.That(currencyName.Description, Is.Not.Null);
	}
	
	[Test]
	public void CurrencyNames_English_RequiredNonEmpty()
	{
		JsonSchemaProperty english = _openApi.Components
			.Schemas[nameof(CurrencyNames)]
			.Properties[nameof(english)];
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(english.Type, Is.EqualTo(JsonObjectType.String));
			Assert.That(english.IsRequired, Is.True);
			Assert.That(english.Description, Is.Not.Null);
			Assert.That(english.MinLength, Is.EqualTo(1));
			Assert.That(english.Example, Is.Not.Null.And
				.EqualTo(CurrencySnapshot.Example.Name.English));
		}
	}
	
	[Test]
	public void CurrencyNames_Native_RequiredNonEmpty()
	{
		JsonSchemaProperty native = _openApi.Components
			.Schemas[nameof(CurrencyNames)]
			.Properties[nameof(native)];
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(native.Type, Is.EqualTo(JsonObjectType.String));
			Assert.That(native.IsRequired, Is.True);
			Assert.That(native.Description, Is.Not.Null);
			Assert.That(native.MinLength, Is.EqualTo(1));
			Assert.That(native.Example, Is.Not.Null.And
				.EqualTo(CurrencySnapshot.Example.Name.Native));
		}
	}
	
	#endregion
	
	#region CurrencySnapshot
	
	[Test]
	public void CurrencySnapshot_RequiredProps()
	{
		JsonSchema currencySnapshot = _openApi.Components.Schemas[nameof(CurrencySnapshot)];
		
		Assert.That(currencySnapshot.Type, Is.EqualTo(JsonObjectType.Object));
		Assert.That(currencySnapshot.AllowAdditionalProperties, Is.False);
		Assert.That(currencySnapshot.RequiredProperties, Is.EquivalentTo(["code", "name"]));
		Assert.That(currencySnapshot.Description, Is.Not.Null);
	}
	
	[Test]
	public void CurrencySnapshot_Name_RequiredNoSample()
	{
		JsonSchemaProperty name = _openApi.Components
			.Schemas[nameof(CurrencySnapshot)]
			.Properties[nameof(name)];
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(name.IsRequired, Is.True);
			Assert.That(name.Description, Is.Not.Null);
			Assert.That(name.Example, Is.Null);
		}
	}
	
	[Test]
	public void CurrencySnapshot_Code_RequiredNoSample()
	{
		JsonSchemaProperty code = _openApi.Components
			.Schemas[nameof(CurrencySnapshot)]
			.Properties[nameof(code)];
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(code.IsRequired, Is.True);
			Assert.That(code.Description, Is.Not.Null);
			Assert.That(code.Example, Is.Null);
		}
	}
	
	[Test]
	public void CurrencySnapshot_IsObsolete_OptionalNullable()
	{
		JsonSchemaProperty obsolete = _openApi.Components
			.Schemas[nameof(CurrencySnapshot)]
			.Properties["is_obsolete"];
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(obsolete.Type, Is.EqualTo(JsonObjectType.Boolean));
			Assert.That(obsolete.IsRequired, Is.False);
			Assert.That(obsolete.IsNullableRaw, Is.True);
			Assert.That(obsolete.Description, Is.Not.Null);
			Assert.That(obsolete.Example, Is.Null);
		}
	}
	
	#endregion
	
	#region CurrenciesListingResponse
	
	[Test]
	public void CurrenciesListingResponse_RequiredProps()
	{
		JsonSchema response = _openApi.Components.Schemas[nameof(CurrenciesListingResponse)];
		
		Assert.That(response.Type, Is.EqualTo(JsonObjectType.Object));
		Assert.That(response.AllowAdditionalProperties, Is.False);
		Assert.That(response.RequiredProperties, Is.EquivalentTo(["currencies"]));
		Assert.That(response.Description, Is.Not.Null);
	}
	
	[Test]
	public void CurrenciesListingResponse_Currencies_RequiredNoSample()
	{
		JsonSchemaProperty currencies = _openApi.Components
			.Schemas[nameof(CurrenciesListingResponse)]
			.Properties[nameof(currencies)];
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(currencies.Type, Is.EqualTo(JsonObjectType.Array));
			Assert.That(currencies.IsRequired, Is.True);
			Assert.That(currencies.Description, Is.Not.Null);
			Assert.That(currencies.Example, Is.Null);
		}
	}
	
	#endregion

	#region CurrencyIsoCode
	
	[Test]
	public void CurrencyIsoCode_Enum()
	{
		JsonSchema currencyCode = _openApi.Components.Schemas[nameof(CurrencyIsoCode)];
		
		Assert.That(currencyCode.Type, Is.EqualTo(JsonObjectType.String));
		Assert.That(currencyCode.Description, Is.Not.Null);
		Assert.That(currencyCode.IsEnumeration, Is.True);
		Assert.That(currencyCode.Enumeration, Is.EqualTo(currencyCode.EnumerationNames).And
			.EquivalentTo(Enum.GetNames<CurrencyIsoCode>()));
	}
	
	#endregion
	
	#region ExtendedCodes
	
	[Test]
	public void ExtendedCodes_RequiredProps()
	{
		JsonSchema currencyCode = _openApi.Components.Schemas[nameof(ExtendedCodes)];
		
		Assert.That(currencyCode.Type, Is.EqualTo(JsonObjectType.Object));
		Assert.That(currencyCode.AllowAdditionalProperties, Is.False);
		Assert.That(currencyCode.RequiredProperties, Is.EquivalentTo(["alphabetic", "numeric", "padded"]));
		Assert.That(currencyCode.Description, Is.Not.Null);
	}
	
	[Test]
	public void ExtendedCodes_Alphabetic_Required3CharString()
	{
		JsonSchemaProperty alphabetic = _openApi.Components
			.Schemas[nameof(ExtendedCodes)]
			.Properties[nameof(alphabetic)];
		
		Assert.Multiple(() =>
		{
			Assert.That(alphabetic.Type, Is.EqualTo(JsonObjectType.String));
			Assert.That(alphabetic.IsRequired, Is.True);
			Assert.That(alphabetic.Description, Is.Not.Null);
			Assert.That(alphabetic.MinLength, Is.EqualTo(3));
			Assert.That(alphabetic.MaxLength, Is.EqualTo(3));
			Assert.That(alphabetic.Example, Is.Not.Null.And
				.EqualTo(CurrencySnapshot.Example.Code.Alphabetic));
		});
	}
	
	[Test]
	public void ExtendedCodes_Numeric_RequiredConstrainedInteger()
	{
		JsonSchemaProperty numeric = _openApi.Components
			.Schemas[nameof(ExtendedCodes)]
			.Properties[nameof(numeric)];
		using (Assert.EnterMultipleScope())
		{
			Assert.That(numeric.Type, Is.EqualTo(JsonObjectType.Integer));
			Assert.That(numeric.IsRequired, Is.True);
			Assert.That(numeric.Description, Is.Not.Null);
			Assert.That(numeric.Minimum, Is.Zero);
			Assert.That(numeric.Maximum, Is.EqualTo(999));
			Assert.That(numeric.Example, Is.Not.Null.And
				.EqualTo(CurrencySnapshot.Example.Code.Numeric));
		}
	}
	
	[Test]
	public void ExtendedCodes_Padded_RequiredConstrainedString()
	{
		JsonSchemaProperty padded = _openApi.Components
			.Schemas[nameof(ExtendedCodes)]
			.Properties[nameof(padded)];
		using (Assert.EnterMultipleScope())
		{
			Assert.That(padded.Type, Is.EqualTo(JsonObjectType.String));
			Assert.That(padded.IsRequired, Is.True);
			Assert.That(padded.Description, Is.Not.Null);
			Assert.That(padded.MinLength, Is.EqualTo(3));
			Assert.That(padded.MaxLength, Is.EqualTo(3));
			Assert.That(padded.Example, Is.Not.Null.And
				.EqualTo(CurrencyDetail.Example.Code.Padded));
		}
	}
	
	#endregion
	
	#region CurrencyDetail
	
	[Test]
	public void CurrencyDetail_RequiredProps()
	{
		JsonSchema currencyDetail = _openApi.Components.Schemas[nameof(CurrencyDetail)];
		
		Assert.That(currencyDetail.Type, Is.EqualTo(JsonObjectType.Object));
		Assert.That(currencyDetail.AllowAdditionalProperties, Is.False);
		Assert.That(currencyDetail.RequiredProperties, Is.EquivalentTo([
			"code", "name", "symbol", "significant_digits", "is_obsolete"
		]));
		Assert.That(currencyDetail.Description, Is.Not.Null);
	}
	
	[Test]
	public void CurrencyDetail_Code_RequiredNoSample()
	{
		JsonSchemaProperty code = _openApi.Components
			.Schemas[nameof(CurrencyDetail)]
			.Properties[nameof(code)];
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(code.IsRequired, Is.True);
			Assert.That(code.Description, Is.Not.Null);
			Assert.That(code.Example, Is.Null);
		}
	}
	
	[Test]
	public void CurrencyDetail_Name_RequiredNoSample()
	{
		JsonSchemaProperty name = _openApi.Components
			.Schemas[nameof(CurrencyDetail)]
			.Properties[nameof(name)];
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(name.IsRequired, Is.True);
			Assert.That(name.Description, Is.Not.Null);
			Assert.That(name.Example, Is.Null);
		}
	}
	
	[Test]
	public void CurrencyDetail_Symbol_RequiredNonEmpty()
	{
		JsonSchemaProperty symbol = _openApi.Components
			.Schemas[nameof(CurrencyDetail)]
			.Properties[nameof(symbol)];
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(symbol.Type, Is.EqualTo(JsonObjectType.String));
			Assert.That(symbol.IsRequired, Is.True);
			Assert.That(symbol.Description, Is.Not.Null);
			Assert.That(symbol.MinLength, Is.EqualTo(1));
			Assert.That(symbol.Example, Is.Not.Null);
		}
	}
	
	[Test]
	public void CurrencyDetail_SignificantDigits_RequiredRange()
	{
		JsonSchemaProperty digits = _openApi.Components
			.Schemas[nameof(CurrencyDetail)]
			.Properties["significant_digits"];
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(digits.Type, Is.EqualTo(JsonObjectType.Integer));
			Assert.That(digits.IsRequired, Is.True);
			Assert.That(digits.Description, Is.Not.Null);
			Assert.That(digits.Minimum, Is.Zero);
			Assert.That(digits.Maximum, Is.EqualTo(5));
			Assert.That(digits.Example, Is.Not.Null.And
				.EqualTo(CurrencyDetail.Example.SignificantDigits));
		}
	}
	
	[Test]
	public void CurrencyDetail_IsObsolete_RequiredBool()
	{
		JsonSchemaProperty obsolete = _openApi.Components
			.Schemas[nameof(CurrencyDetail)]
			.Properties["is_obsolete"];
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(obsolete.Type, Is.EqualTo(JsonObjectType.Boolean));
			Assert.That(obsolete.IsRequired, Is.True);
			Assert.That(obsolete.Description, Is.Not.Null);
			Assert.That(obsolete.Example, Is.Null);
		}
	}
	
	#endregion

	#region CurrencyRetrievalResponse
	
	[Test]
	public void CurrencyRetrievalResponse_RequiredProps()
	{
		JsonSchema response = _openApi.Components.Schemas[nameof(CurrencyRetrievalResponse)];
		
		Assert.That(response.Type, Is.EqualTo(JsonObjectType.Object));
		Assert.That(response.AllowAdditionalProperties, Is.False);
		Assert.That(response.RequiredProperties, Is.EquivalentTo(["currency"]));
		Assert.That(response.Description, Is.Not.Null);
	}
	
	[Test]
	public void CurrencyRetrievalResponse_Currency_RequiredNoSample()
	{
		JsonSchemaProperty currency = _openApi.Components
			.Schemas[nameof(CurrencyRetrievalResponse)]
			.Properties[nameof(currency)];
		
		using (Assert.EnterMultipleScope())
		{
			Assert.That(currency.IsRequired, Is.True);
			Assert.That(currency.Description, Is.Not.Null);
			Assert.That(currency.Example, Is.Null);
		}
	}
	
	#endregion
}