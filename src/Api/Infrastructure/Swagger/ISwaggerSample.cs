namespace NMoneys.Api.Infrastructure.Swagger;

public interface ISwaggerSample<out TModel> where TModel : class, IEquatable<TModel>
{
	static TModel Example { get; } = default!;
}
