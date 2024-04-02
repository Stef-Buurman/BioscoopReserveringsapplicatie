using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class ExperiencesModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("filmId")]
        public int FilmId { get; set; }

        [JsonPropertyName("intensity")]
        public string Intensity { get; set; }

        [JsonPropertyName("timeLength")]
        public int TimeLength { get; set; }

        [JsonPropertyName("archived")]
        public bool Archived { get; set; }

        public ExperiencesModel(int id, string name, int FilmId, string intensity, int timeLength, bool archived)
        {
            this.Id = id;
            this.Name = name;
            this.FilmId = FilmId;
            this.Intensity = intensity;
            this.TimeLength = timeLength;
            this.Archived = archived;
        }
        public ExperiencesModel(string name, int FilmId, string intensity, int timeLength, bool archived) : this(0, name, FilmId, intensity, timeLength, archived)
        { }

        public ExperiencesModel() : this(0, "", 0, "", 0, false)
        { }
    }
}