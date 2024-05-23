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

        [JsonConverter(typeof(EnumConverter<RoomType>))]
        [JsonPropertyName("roomType")]
        public RoomType RoomType { get; set; }

        [JsonConverter(typeof(EnumConverter<Status>))]
        [JsonPropertyName("status")]
        public Status Status { get; set; }

        public RoomModel(int id, int locationId, int roomNumber, RoomType roomType, Status status)
        {
            Id = id;
            RoomNumber = roomNumber;
            LocationId = locationId;
            Status = status;
            RoomType = roomType;
        }
    }
}