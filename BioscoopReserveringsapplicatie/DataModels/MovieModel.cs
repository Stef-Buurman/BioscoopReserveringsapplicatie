using System.Text.Json.Serialization;

class MovieModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("title")]
    public string Title { get; set; }

    [JsonPropertyName("description")]
    public string Description { get; set; }

    [JsonPropertyName("genre")]
    public string Genre { get; set; }

    [JsonPropertyName("rating")]
    public string Rating { get; set; }

    public MovieModel(int id, string title, string description, string genre, string rating)
    {
        Id = id;
        Title = title;
        Description = description;
        Genre = genre;
        Rating = rating;
    }
}




