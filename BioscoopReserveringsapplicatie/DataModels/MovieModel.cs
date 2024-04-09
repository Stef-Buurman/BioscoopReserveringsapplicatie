using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    class MovieModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonConverter(typeof(GenreListConverter))]
        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; }

        [JsonConverter(typeof(AgeCategoryConverter))]
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
    }
}



