using System.Text.Json;
using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class EnumListConverter<T> : JsonConverter<List<T>>
    {
        public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<string> items = JsonSerializer.Deserialize<List<string>>(ref reader, options);
            if (items == null) return new List<T>();

            List<T> convertedItems = new List<T>();
            List<string> allEnumValues = Enum.GetValues(typeof(T)).Cast<T>().Select(e => e.ToString()).ToList();

            foreach (string item in items)
            {
                if (allEnumValues.Contains(item))
                {
                    convertedItems.Add((T)Enum.Parse(typeof(T), item));
                }
            }
            return convertedItems;
        }

        public override void Write(Utf8JsonWriter writer, List<T> values, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var value in values)
            {
                writer.WriteStringValue(value.ToString());
            }
            writer.WriteEndArray();
        }
    }
}