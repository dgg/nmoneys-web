using System.Net.Mail;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace NMoneys.Api.Infrastructure.Serialization;

internal class EmailConverter : JsonConverter<MailAddress>
{
	public override MailAddress? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		string s = reader.GetString() ?? throw new JsonException("Expected 'string' but was 'null'.");
		return new MailAddress(s);
	}

	public override void Write(Utf8JsonWriter writer, MailAddress value, JsonSerializerOptions options) => writer.WriteStringValue(value.Address);
}