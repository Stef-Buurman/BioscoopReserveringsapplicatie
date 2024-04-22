using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class ReservationModel : IID
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("experienceId")]
        public int ExperienceId { get; set; }

        [JsonPropertyName("locationId")]
        public int? LocationId { get; set; }

        [JsonPropertyName("dateTime")]
        public DateTime? DateTime { get; set; }

        [JsonPropertyName("roomId")]
        public int? RoomId { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        public ReservationModel(int id, int experienceId, int? locationId, DateTime? dateTime, int? roomId, int userId)
        {
            Id = id;
            ExperienceId = experienceId;
            LocationId = locationId;
            DateTime = dateTime;
            RoomId = roomId;
            UserId = userId;
        }
    }
}