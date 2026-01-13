using FastEndpoints;
using FastEndpoints.Swagger;

using Lamar.Microsoft.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Configure Lamar for dependency injection
builder.Host.UseLamar(registry =>
{
	// Auto-registration with default convention
	registry.Scan(scanner =>
	{
		scanner.AssemblyContainingType<Program>();
		scanner.LookForRegistries();
		scanner.WithDefaultConventions();
	});
});

builder.Services
	.AddFastEndpoints();

var app = builder.Build();

app.UseFastEndpoints(c =>
{
	c.Endpoints.ShortNames = true;
	c.Errors.UseProblemDetails();
});

// do not expose swagger in prod
if (!app.Environment.IsProduction())
{
	app.UseSwaggerGen(uiConfig: ui =>
	{
		ui.ShowOperationIDs();
	});
}

app.Run();