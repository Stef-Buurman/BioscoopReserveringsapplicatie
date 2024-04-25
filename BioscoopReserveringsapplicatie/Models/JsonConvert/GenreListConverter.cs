using System.Text.Json;
using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class GenreListConverter : JsonConverter<List<Genre>>
    {
        public override List<Genre> Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            List<string> genres = JsonSerializer.Deserialize<List<string>>(ref reader, options);
            if (genres == null) return null;

            List<Genre> result = new List<Genre>();
            foreach (var genre in genres)
            {
                if (Enum.TryParse(genre, out Genre genreEnum)) result.Add(genreEnum);
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, List<Genre> value, JsonSerializerOptions options)
        {
            writer.WriteStartArray();
            foreach (var genre in value)
            {
                writer.WriteStringValue(genre.ToString());
            }
            writer.WriteEndArray();
        }
    }
}
