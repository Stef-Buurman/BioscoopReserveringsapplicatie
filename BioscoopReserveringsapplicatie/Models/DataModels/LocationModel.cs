using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class LocationModel : IID
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonConverter(typeof(EnumConverter<Status>))]
        [JsonPropertyName("status")]
        public Status Status { get; set; }

        public LocationModel(int id, string name)
        {
            Id = id;
            Name = name;
            Status = Status.Active;
        }
    }
}