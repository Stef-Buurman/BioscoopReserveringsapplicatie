using System.Text.Json;
using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class LanguageConverter : JsonConverter<Language>
    {
        public override Language Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string Intens = JsonSerializer.Deserialize<string>(ref reader, options);
            if (Intens == null) return default;

            if (Enum.TryParse(Intens, out Language IntensityEnum)) return IntensityEnum;
            return default;
        }

        public override void Write(Utf8JsonWriter writer, Language value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
