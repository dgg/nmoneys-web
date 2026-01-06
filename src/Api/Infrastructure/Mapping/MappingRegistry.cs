using Lamar;

using Mapster;

namespace NMoneys.Api.Infrastructure.Mapping;

// configure mapping
// ServiceRegistry instances have to be public
public class MappingRegistry : ServiceRegistry
{
	public MappingRegistry()
	{
		TypeAdapterConfig.GlobalSettings.Scan(typeof(Program).Assembly);
		TypeAdapterConfig.GlobalSettings.Compile();
	}
}
