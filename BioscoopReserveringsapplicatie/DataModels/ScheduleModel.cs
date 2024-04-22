using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class ScheduleModel : IID
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("experienceId")]
        public int ExperienceId { get; set; }

        [JsonPropertyName("locationId")]
        public int LocationId { get; set; }

        [JsonPropertyName("roomId")]
        public int RoomId { get; set; }

        [JsonPropertyName("scheduledDateTimeStart")]
        public DateTime ScheduledDateTimeStart { get; set; }

        [JsonPropertyName("scheduledDateTimeEnd")]
        public DateTime ScheduledDateTimeEnd { get; set; }

        public ScheduleModel(int id, int experienceId, int locationId, int roomId, DateTime scheduledDateTimeStart, DateTime scheduledDateTimeEnd)
        {
            Id = id;
            ExperienceId = experienceId;
            LocationId = locationId;
            RoomId = roomId;
            ScheduledDateTimeStart = scheduledDateTimeStart;
            ScheduledDateTimeEnd =  scheduledDateTimeEnd;
        }
    }
}