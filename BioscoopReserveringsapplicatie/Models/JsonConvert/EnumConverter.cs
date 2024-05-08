using System.Text.Json;
using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class EnumConverter<T> : JsonConverter<T> where T : Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType != JsonTokenType.Number)
            {
                throw new JsonException($"Unexpected token type: {reader.TokenType}");
            }

            int enumValue = reader.GetInt32();

            if (Enum.IsDefined(typeof(T), enumValue))
            {
                return (T)Enum.ToObject(typeof(T), enumValue);
            }
            else
            {
                throw new JsonException($"Invalid enum value: {enumValue}");
            }
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(Convert.ToInt32(value));
        }
    }
}