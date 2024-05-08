using System.Text.Json;
using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class EnumConverter<T> : JsonConverter<T> where T : Enum
    {
        public override T Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            if (reader.TokenType == JsonTokenType.Number)
            {
                int enumValue = reader.GetInt32();

                if (Enum.IsDefined(typeof(T), enumValue))
                {
                    return (T)Enum.ToObject(typeof(T), enumValue);
                }
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                string enumValueString = reader.GetString();

                foreach (var enumValue in Enum.GetValues(typeof(T)))
                {
                    if (string.Equals(enumValue.ToString(), enumValueString, StringComparison.OrdinalIgnoreCase))
                    {
                        return (T)enumValue;
                    }
                }

                throw new JsonException($"Invalid enum value: {enumValueString}");
            }
            return default(T);
        }

        public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}