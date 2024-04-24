using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class PromotionModel : IID
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("title")]
        public string Title { get; set; }

        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("status")]
        public bool Status { get; set; }

        public PromotionModel(int id, string title, string description, bool status)
        {
            Id = id;
            Title = title;
            Description = description;
            Status = status;
        }
    }
}