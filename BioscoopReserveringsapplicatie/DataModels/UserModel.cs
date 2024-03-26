using System.Text.Json.Serialization;

namespace BioscoopReserveringsapplicatie
{
    public class UserModel
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("isAdmin")]
        public bool IsAdmin { get; }

        [JsonPropertyName("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [JsonPropertyName("genres")]
        public List<string> Genres { get; set; }

        [JsonPropertyName("ageCategory")]
        public int AgeCategory { get; set; }

        [JsonPropertyName("intensity")]
        public string Intensity { get; set; }

        [JsonPropertyName("language")]
        public string Language { get; set; }

        public UserModel(int id, bool isAdmin, string emailAddress, string password, string fullName, List<string> genres, int ageCategory, string intensity, string language)
        {
            Id = id;
            IsAdmin = isAdmin;
            EmailAddress = emailAddress;
            Password = password;
            FullName = fullName;
            Genres = genres;
            AgeCategory = ageCategory;
            Intensity = intensity;
            Language = language;
        }
    }
}