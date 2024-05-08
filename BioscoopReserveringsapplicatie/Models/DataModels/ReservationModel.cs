using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class ReservationModel : IID
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("scheduleId")]
        public int ScheduleId { get; set; }

        [JsonPropertyName("userId")]
        public int UserId { get; set; }

        [JsonPropertyName("isCanceled")]
        public bool IsCanceled { get; set; }

        public ReservationModel(int id, int scheduleId, int userId, bool isCanceled = false)
        {
            Id = id;
            ScheduleId = scheduleId;
            UserId = userId;
            IsCanceled = isCanceled;
        }
    }
}