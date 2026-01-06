using FastEndpoints;
using FastEndpoints.Swagger;

using Microsoft.AspNetCore.Http.Json;

using NMoneys.Api.Infrastructure.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services
	.Configure<JsonOptions>(o => SerializationContext.Default.ConfigureOptions(o.SerializerOptions))
	.AddFastEndpoints()
	.SwaggerDocument(o =>
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

var app = builder.Build();

app.UseFastEndpoints(c =>
{
	c.Endpoints.ShortNames = true;
});

// Use Swagger only in dev
if (app.Environment.IsDevelopment())
{
	app.UseSwaggerGen(uiConfig: ui =>
	{
		ui.ShowOperationIDs();
	});
}

app.Run();