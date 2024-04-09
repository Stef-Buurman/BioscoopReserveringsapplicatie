using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    class LocationModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        public LocationModel(int id, string title)
        {
            Id = id;
            Title = title;
        }
    }
}