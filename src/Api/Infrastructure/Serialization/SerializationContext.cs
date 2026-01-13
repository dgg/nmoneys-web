using System.Text.Json;
using System.Text.Json.Serialization;

using FastEndpoints;

using NMoneys.Api.Currencies;

namespace NMoneys.Api.Infrastructure.Serialization;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web,
	DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
	PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
	UseStringEnumConverter = true)]
[JsonSerializable(typeof(CurrenciesListingResponse))]
[JsonSerializable(typeof(CurrencyRetrievalResponse))]
[JsonSerializable(typeof(AmountsFormattingRequest))]
[JsonSerializable(typeof(AmountsFormattingResponse))]
[JsonSerializable(typeof(ProblemDetails))]
internal partial class SerializationContext : JsonSerializerContext
{
	public void ConfigureOptions(JsonSerializerOptions options)
	{
		// we need to repeat ourselves to set the props specified in the attribute to the options
		options.DefaultIgnoreCondition = Options.DefaultIgnoreCondition;
		options.PropertyNamingPolicy = Options.PropertyNamingPolicy;
		// equivalent to UseStringEnumConverter = true
		options.Converters.Add(new JsonStringEnumConverter());
	}
}