using Alba;

using NJsonSchema;

using NMoneys;
using NMoneys.Api.Amounts;

using NSwag;

namespace Api.Tests.Amounts.DataTypes;

[TestFixture, Category("Integration")]
public class AmountsOpenApiTester
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
	
		#region AmountsFormattingRequest

	[Test]
	public void AmountsFormattingRequest_RequiredProps()
	{
		JsonSchema request = _openApi.Components.Schemas[nameof(AmountsFormattingRequest)];

		Assert.Multiple(() =>
		{
			Assert.That(request.Type, Is.EqualTo(JsonObjectType.Object));
			Assert.That(request.AllowAdditionalProperties, Is.False);
			Assert.That(request.RequiredProperties, Is.EquivalentTo(["amounts", "alphabetic_code"]));
			Assert.That(request.Description, Is.Not.Null);
		});
	}

	[Test]
	public void AmountsFormattingRequest_Amounts_RequiredArrayWithMinLength()
	{
		JsonSchemaProperty amounts = _openApi.Components
			.Schemas[nameof(AmountsFormattingRequest)]
			.Properties[nameof(amounts)];

		Assert.Multiple(() =>
		{
			Assert.That(amounts.Type, Is.EqualTo(JsonObjectType.Array));
			Assert.That(amounts.IsRequired, Is.True);
			Assert.That(amounts.Description, Is.Not.Null);
			Assert.That(amounts.MinItems, Is.EqualTo(1));
			Assert.That(amounts.Item, Is.Not.Null);
			Assert.That(amounts.Item!.Type, Is.EqualTo(JsonObjectType.Number));
			Assert.That(amounts.Example, Is.Null);
		});
	}
	
	[Test]
	public void AmountsFormattingRequest_AlphabeticCode_RequiredEnumRef()
	{
		JsonSchemaProperty alphabeticCode = _openApi.Components
			.Schemas[nameof(AmountsFormattingRequest)]
			.Properties["alphabetic_code"];

		Assert.Multiple(() =>
		{
			Assert.That(alphabeticCode.Type, Is.EqualTo(JsonObjectType.None), "oneOf");
			Assert.That(alphabeticCode.IsRequired, Is.True);
			Assert.That(alphabeticCode.Description, Is.Not.Null);
			Assert.That(alphabeticCode.OneOf, Has.Count.EqualTo(1));
			JsonSchema reference = alphabeticCode.OneOf.ElementAt(0).ActualSchema;
			Assert.That(reference.Type, Is.EqualTo(JsonObjectType.String), "currency iso code");
			int currencyCount = Enum.GetNames<CurrencyIsoCode>().Length;
			Assert.That(reference.EnumerationNames, Has.Count.EqualTo(currencyCount));
		});
	}

	#endregion

	#region AmountsFormattingResponse

	[Test]
	public void AmountsFormattingResponse_RequiredProps()
	{
		JsonSchema response = _openApi.Components.Schemas[nameof(AmountsFormattingResponse)];

		Assert.Multiple(() =>
		{
			Assert.That(response.Type, Is.EqualTo(JsonObjectType.Object));
			Assert.That(response.AllowAdditionalProperties, Is.False);
			Assert.That(response.RequiredProperties, Is.EquivalentTo(["formatted"]));
			Assert.That(response.Description, Is.Not.Null);
		});
	}

	[Test]
	public void AmountsFormattingResponse_Formatted_RequiredNotEmptyArray()
	{
		JsonSchemaProperty formatted = _openApi.Components
			.Schemas[nameof(AmountsFormattingResponse)]
			.Properties[nameof(formatted)];

		Assert.Multiple(() =>
		{
			Assert.That(formatted.Type, Is.EqualTo(JsonObjectType.Array));
			Assert.That(formatted.IsRequired, Is.True);
			Assert.That(formatted.Description, Is.Not.Null);
			Assert.That(formatted.MinItems, Is.EqualTo(1));
			Assert.That(formatted.Item, Is.Not.Null);
			Assert.That(formatted.Item!.Type, Is.EqualTo(JsonObjectType.String));
			Assert.That(formatted.Example, Is.Null);
		});
	}

	#endregion
}