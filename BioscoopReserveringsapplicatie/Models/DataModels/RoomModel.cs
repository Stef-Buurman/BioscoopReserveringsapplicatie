using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class RoomModel : IID
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("locationId")]
        public int LocationId { get; set; }

        [JsonPropertyName("roomNumber")]
        public int RoomNumber { get; set; }

        [JsonPropertyName("capacity")]
        public int Capacity { get; set; }

        public RoomModel(int id, int locationId, int roomNumber, int capacity)
        {
            Id = id;
            RoomNumber = roomNumber;
            LocationId = locationId;
            Capacity = capacity;
        }
    }
}