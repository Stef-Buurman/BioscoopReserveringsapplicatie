using System.Text.Json.Serialization;
using System.Text.Json;

namespace BioscoopReserveringsapplicatie
{
    public class TupleListConverter<T> : JsonConverter<List<T>>
    {
        public override List<T> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            using (JsonDocument doc = JsonDocument.ParseValue(ref reader))
            {
                var root = doc.RootElement;
                var result = new List<T>();

                foreach (var element in root.EnumerateArray())
                {
                    var tupleArray = JsonSerializer.Deserialize<int[]>(element.GetRawText());
                    result.Add((T)Convert.ChangeType((tupleArray[0], tupleArray[1]), typeof(T)));
                }

                return result;
            }
        }

        public override void Write(Utf8JsonWriter writer, List<T> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var tuple in value)
            {
                var tupleArray = JsonSerializer.SerializeToUtf8Bytes(tuple, options);
                writer.WriteRawValue(tupleArray);
            }
            writer.WriteEndArray();
        }
    }
}
