using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using System.Text.Json.Serialization;

using NMoneys.Api.Infrastructure.Auth.Models;

namespace NMoneys.Api.Infrastructure.Auth;

[JsonSourceGenerationOptions(JsonSerializerDefaults.Web,
	DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
	PropertyNamingPolicy = JsonKnownNamingPolicy.SnakeCaseLower)]
[JsonSerializable(typeof(GetTokenBody))]
[JsonSerializable(typeof(OAuthToken))]
[JsonSerializable(typeof(CreateClientBody))]
[JsonSerializable(typeof(ApiClient))]
[JsonSerializable(typeof(GrantClientBody))]
[ExcludeFromCodeCoverage]
internal partial class ManagementSerializationContext : JsonSerializerContext;