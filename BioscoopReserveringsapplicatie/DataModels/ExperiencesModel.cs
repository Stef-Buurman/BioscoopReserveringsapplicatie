using System.Text.Json.Serialization;

public class ExperiencesModel
{
    [JsonPropertyName("id")]
    public int Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("filmId")]
    public int FilmId { get; set; }

    [JsonPropertyName("intensity")]
    public int Intensity { get; set; }

    [JsonPropertyName("timeLength")]
    public int TimeLength { get; set; }

    public ExperiencesModel(int id, string name, int FilmId, int intensity, int timeLength)
    {
        this.Id = id;
        this.Name = name;
        this.FilmId = FilmId;
        this.Intensity = intensity;
        this.TimeLength = timeLength;
    }
    public ExperiencesModel(string name, int FilmId, int intensity, int timeLength) : this(0, name, FilmId, intensity, timeLength)
    { }

    public ExperiencesModel() : this(0, "", 0, 0, 0)
    { }
}
