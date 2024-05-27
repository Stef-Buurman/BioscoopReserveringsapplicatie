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

        [JsonConverter(typeof(TupleListConverter<int, int>))]
        [JsonPropertyName("seat")]
        public List<(int, int)> Seat { get; set; }

        [JsonPropertyName("isCanceled")]
        public bool IsCanceled { get; set; }

        public ReservationModel(int id, int scheduleId, int userId, List<(int, int)> seat, bool isCanceled = false)
        {
            Id = id;
            ScheduleId = scheduleId;
            UserId = userId;
            Seat = seat;
            IsCanceled = isCanceled;
        }
    }
}