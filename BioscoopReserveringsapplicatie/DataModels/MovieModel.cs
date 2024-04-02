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

        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; }

        [JsonPropertyName("rating")]
        public string Rating { get; set; }

        public MovieModel(int id, string title, string description, List<Genre> genres, string rating)
        {
            Id = id;
            Title = title;
            Description = description;
            Genres = genres;
            Rating = rating;
        }
    }
}



