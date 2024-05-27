using System.Text.Json.Serialization;
using System.Text.Json;

namespace BioscoopReserveringsapplicatie
{
    public class TupleListConverter<T1, T2> : JsonConverter<List<(T1, T2)>>
    {
        public override List<(T1, T2)> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = new List<(T1, T2)>();

            if (reader.TokenType != JsonTokenType.StartArray)
                throw new JsonException();

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                    return result;

                if (reader.TokenType != JsonTokenType.StartArray)
                    throw new JsonException();

                reader.Read();
                var item1 = JsonSerializer.Deserialize<T1>(ref reader, options);
                reader.Read();
                var item2 = JsonSerializer.Deserialize<T2>(ref reader, options);
                reader.Read(); // EndArray

                result.Add((item1, item2));
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, List<(T1, T2)> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var tuple in value)
            {
                writer.WriteStartArray();
                JsonSerializer.Serialize(writer, tuple.Item1, options);
                JsonSerializer.Serialize(writer, tuple.Item2, options);
                writer.WriteEndArray();
            }
            writer.WriteEndArray();
        }
    }
}