using Lamar;

using Microsoft.AspNetCore.Http.Json;

namespace NMoneys.Api.Infrastructure.Serialization;

// configure serialization
// ServiceRegistry instances have to be public
public class SerializationRegistry : ServiceRegistry
{
	public SerializationRegistry()
	{
		this.Configure<JsonOptions>(o => SerializationContext.Default.ConfigureOptions(o.SerializerOptions));
	}
}