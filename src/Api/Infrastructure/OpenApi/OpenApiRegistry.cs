using FastEndpoints.Swagger;

using Lamar;

namespace NMoneys.Api.Infrastructure.OpenApi;

// configure swagger
public class OpenApiRegistry : ServiceRegistry
{
	public OpenApiRegistry()
	{
		this.SwaggerDocument(o =>
		{
			o.ShortSchemaNames = true;
			o.RemoveEmptyRequestSchema = true;
			o.DocumentSettings = d =>
			{
				d.Title = "nMoneys API";
				d.Version = "v1";
				d.Description = "API for currency information retrieval";
				d.MarkNonNullablePropsAsRequired();
			};
		});
	}
}