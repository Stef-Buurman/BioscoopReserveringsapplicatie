using System.ComponentModel;
using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class ExperienceModel : IID
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("filmId")]
        public int FilmId { get; set; }

        [JsonConverter(typeof(EnumConverter<Intensity>))]
        [JsonPropertyName("intensity")]
        public Intensity Intensity { get; set; }

        [JsonPropertyName("timeLength")]
        public int TimeLength { get; set; }

        [JsonConverter(typeof(EnumConverter<Status>))]
        [JsonPropertyName("status")]
        public Status Status { get; set; }

        public ExperienceModel(int id, string name, string description, int FilmId, Intensity intensity, int timeLength, Status status)
        {
            this.Id = id;
            this.Name = name;
            this.Description = description;
            this.FilmId = FilmId;
            this.Intensity = intensity;
            this.TimeLength = timeLength;
            this.Status = status;
        }
        public ExperienceModel(string name, string description, int FilmId, Intensity intensity, int timeLength, Status status) : this(0, name, description, FilmId, intensity, timeLength, status)
        { }

        public ExperienceModel() : this(0, "", "", 0, default, 0, default)
        { }
    }
}