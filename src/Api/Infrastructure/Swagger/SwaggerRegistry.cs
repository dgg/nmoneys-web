using FastEndpoints.Swagger;

using Lamar;

namespace NMoneys.Api.Infrastructure.Swagger;

// configure swagger
public class SwaggerRegistry : ServiceRegistry
{
	public SwaggerRegistry()
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