using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class ExperienceModel : IID
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("filmId")]
        public int FilmId { get; set; }

        [JsonConverter(typeof(IntensityConverter))]
        [JsonPropertyName("intensity")]
        public Intensity Intensity { get; set; }

        [JsonPropertyName("timeLength")]
        public int TimeLength { get; set; }

        [JsonPropertyName("archived")]
        public bool Archived { get; set; }

        public ExperienceModel(int id, string name, int FilmId, Intensity intensity, int timeLength, bool archived)
        {
            this.Id = id;
            this.Name = name;
            this.FilmId = FilmId;
            this.Intensity = intensity;
            this.TimeLength = timeLength;
            this.Archived = archived;
        }
        public ExperienceModel(string name, int FilmId, Intensity intensity, int timeLength, bool archived) : this(0, name, FilmId, intensity, timeLength, archived)
        { }

        public ExperienceModel() : this(0, "", 0, default, 0, false)
        { }
    }
}