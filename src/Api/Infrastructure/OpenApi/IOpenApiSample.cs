namespace NMoneys.Api.Infrastructure.OpenApi;

public interface IOpenApiSample<out TModel> where TModel : class, IEquatable<TModel>
{
	static TModel Example { get; } = default!;
}
