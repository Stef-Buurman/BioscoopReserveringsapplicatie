using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    class LocationModel : IID
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        public LocationModel(int id, string name)
        {
            Id = id;
            Name = name;
        }
    }
}