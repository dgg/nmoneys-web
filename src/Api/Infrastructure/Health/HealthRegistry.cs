using FastEndpoints;

using Lamar;

namespace NMoneys.Api.Infrastructure.Health;

public class HealthRegistry : ServiceRegistry
{
	public HealthRegistry() => this
		.AddServiceHealthChecks(opts => opts.ReadyPath = "/healthz");
}