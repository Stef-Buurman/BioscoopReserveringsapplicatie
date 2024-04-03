using System.Text.Json;
using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class IntensityConverter : JsonConverter<Intensity>
    {
        public override Intensity Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string Intens = JsonSerializer.Deserialize<string>(ref reader, options);
            if (Intens == null) return default;

            if (Enum.TryParse(Intens, out Intensity IntensityEnum)) return IntensityEnum;
            return default;
        }

        public override void Write(Utf8JsonWriter writer, Intensity value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}
