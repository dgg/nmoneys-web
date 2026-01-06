using Alba;

using NJsonSchema;

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
	
	#region CurrencyCode
	
	[Test]
	public void CurrencyCode_RequiredProps()
	{
		JsonSchema currencyCode = _openApi.Components.Schemas[nameof(CurrencyCode)];
		
		Assert.That(currencyCode.Type, Is.EqualTo(JsonObjectType.Object));
		Assert.That(currencyCode.AllowAdditionalProperties, Is.False);
		Assert.That(currencyCode.RequiredProperties, Is.EquivalentTo(["alphabetic", "numeric"]));
		Assert.That(currencyCode.Description, Is.Not.Null);
	}
	
	[Test]
	public void CurrencyCode_Alphabetic_Required3CharString()
	{
		JsonSchemaProperty alphabetic = _openApi.Components
			.Schemas[nameof(CurrencyCode)]
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
	public void CurrencyCode_Numeric_RequiredConstrainedInteger()
	{
		JsonSchemaProperty numeric = _openApi.Components
			.Schemas[nameof(CurrencyCode)]
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
	
	#region CurrencyName
	
	[Test]
	public void CurrencyName_RequiredProps()
	{
		JsonSchema currencyName = _openApi.Components.Schemas[nameof(CurrencyName)];
		
		Assert.That(currencyName.Type, Is.EqualTo(JsonObjectType.Object));
		Assert.That(currencyName.AllowAdditionalProperties, Is.False);
		Assert.That(currencyName.RequiredProperties, Is.EquivalentTo(["english", "native"]));
		Assert.That(currencyName.Description, Is.Not.Null);
	}
	
	[Test]
	public void CurrencyName_English_RequiredNonEmpty()
	{
		JsonSchemaProperty english = _openApi.Components
			.Schemas[nameof(CurrencyName)]
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
	public void CurrencyName_Native_RequiredNonEmpty()
	{
		JsonSchemaProperty native = _openApi.Components
			.Schemas[nameof(CurrencyName)]
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
	
	#region CurrencySnapshot
	
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
}