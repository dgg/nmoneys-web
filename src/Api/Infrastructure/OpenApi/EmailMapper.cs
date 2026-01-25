using System.Net.Mail;

using NJsonSchema;
using NJsonSchema.Generation.TypeMappers;

namespace NMoneys.Api.Infrastructure.OpenApi;

internal class EmailMapper() : PrimitiveTypeMapper(typeof(MailAddress), transformer)
{
	private static void transformer(JsonSchema schema)
	{
		schema.Type = JsonObjectType.String;
		schema.Format = JsonFormatStrings.Email;
	}
}