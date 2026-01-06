using System.Text.Json;
using System.Text.Json.Serialization;

using NMoneys.Api.Currencies;

namespace NMoneys.Api.Infrastructure.Serialization;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web,
	DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
	PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(CurrenciesListingResponse))]
internal partial class SerializationContext : JsonSerializerContext
{
	public void ConfigureOptions(JsonSerializerOptions options)
	{
		// we need to repeat ourselves to set the props specified in the attribute to the options
		options.DefaultIgnoreCondition = Options.DefaultIgnoreCondition;
		options.PropertyNamingPolicy = Options.PropertyNamingPolicy;
	}
}