using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

using FastEndpoints;

using NMoneys.Api.Amounts;
using NMoneys.Api.Authn;
using NMoneys.Api.Currencies;

namespace NMoneys.Api.Infrastructure.Serialization;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web,
	DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
	PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower,
	UseStringEnumConverter = true,
	Converters = [
		typeof(EmailConverter)
	])]
[JsonSerializable(typeof(CurrenciesListingResponse))]
[JsonSerializable(typeof(CurrencyRetrievalResponse))]
[JsonSerializable(typeof(AmountsFormattingRequest))]
[JsonSerializable(typeof(AmountsFormattingResponse))]
[JsonSerializable(typeof(ClientCreationRequest))]
[JsonSerializable(typeof(ClientCreationResponse))]
[JsonSerializable(typeof(TokenRetrievalRequest))]
[JsonSerializable(typeof(TokenRetrievalResponse))]
[JsonSerializable(typeof(ProblemDetails))]
[ExcludeFromCodeCoverage]
internal partial class SerializationContext : JsonSerializerContext
{
	public void ConfigureOptions(JsonSerializerOptions options)
	{
		// we need to repeat ourselves to set the props specified in the attribute to the options
		options.DefaultIgnoreCondition = Options.DefaultIgnoreCondition;
		options.PropertyNamingPolicy = Options.PropertyNamingPolicy;
		// equivalent to UseStringEnumConverter = true
		options.Converters.Add(new JsonStringEnumConverter());
		// add the configured converters
		foreach (JsonConverter converter in Options.Converters)
		{
			options.Converters.Add(converter);
		}
	}
}