using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class MovieModel : IID
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonConverter(typeof(EnumListConverter<Genre>))]
        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; }

        [JsonConverter(typeof(EnumConverter<AgeCategory>))]
        [JsonPropertyName("ageCategory")]
        public AgeCategory AgeCategory { get; set; }

        [JsonConverter(typeof(EnumConverter<Status>))]
        [JsonPropertyName("status")]
        public Status Status { get; set; }

        public MovieModel(int id, string title, string description, List<Genre> genres, AgeCategory ageCategory, Status status)
        {
            Id = id;
            Title = title;
            Description = description;
            Genres = genres;
            AgeCategory = ageCategory;
            Status = status;
        }
        public MovieModel(int id, string title, string description, List<Genre> genres, AgeCategory ageCategory) : this(id, title, description, genres, ageCategory, Status.Active)
        { }
        public MovieModel(string title, string description, List<Genre> genres, AgeCategory ageCategory, Status status) : this(0, title, description, genres, ageCategory, status)
        { }
        public MovieModel() : this(0, "", "", default, default, Status.Active)
        { }
    }
}



