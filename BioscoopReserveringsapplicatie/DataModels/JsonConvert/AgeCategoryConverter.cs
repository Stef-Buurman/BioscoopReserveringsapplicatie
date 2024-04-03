using System.Text.Json;
using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class AgeCategoryConverter : JsonConverter<AgeCategory>
    {
        public override AgeCategory Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string Intens = JsonSerializer.Deserialize<string>(ref reader, options);
            if (Intens == null) return default;

            if (Enum.TryParse(Intens, out AgeCategory IntensityEnum)) return IntensityEnum;
            return default;
        }

        public override void Write(Utf8JsonWriter writer, AgeCategory value, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }
    }
}
