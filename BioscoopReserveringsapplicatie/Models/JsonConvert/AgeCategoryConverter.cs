using System.Text.Json;
using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class AgeCategoryConverter : JsonConverter<AgeCategory>
    {
        public override AgeCategory Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                JsonElement element = doc.RootElement;

                if (element.ValueKind != JsonValueKind.Number) return default;
                
                int ageCategoryValue = element.GetInt32();

                if (Enum.TryParse(ageCategoryValue.ToString(), out AgeCategory ageCategory))
                {
                    return ageCategory;
                }
                else
                {
                    return default;
                }
            }
        }

        public override void Write(Utf8JsonWriter writer, AgeCategory value, JsonSerializerOptions options)
        {
            writer.WriteNumberValue(Convert.ToInt32(value));
        }
    }
}
