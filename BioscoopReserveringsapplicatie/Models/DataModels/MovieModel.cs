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

       [JsonPropertyName("archived")]
        public bool Archived { get; set; }

        public MovieModel(int id, string title, string description, List<Genre> genres, AgeCategory ageCategory, bool archived)
        {
            Id = id;
            Title = title;
            Description = description;
            Genres = genres;
            AgeCategory = ageCategory;
            Archived = archived;
        }
        public MovieModel(int id, string title, string description, List<Genre> genres, AgeCategory ageCategory) : this(id, title, description, genres, ageCategory, false)
        { }
        public MovieModel(string title, string description, List<Genre> genres, AgeCategory ageCategory, bool archived) : this(0, title, description, genres, ageCategory, archived)
        { }
        public MovieModel() : this(0, "", "", default, default, false)
        { }
    }
}



