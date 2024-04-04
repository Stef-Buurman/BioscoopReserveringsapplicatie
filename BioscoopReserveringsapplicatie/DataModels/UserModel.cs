using System.Text.Json.Serialization;
using System.Xml;

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

        [JsonConverter(typeof(GenreListConverter))]
        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; }

        [JsonConverter(typeof(AgeCategoryConverter))]
        [JsonPropertyName("ageCategory")]
        public AgeCategory AgeCategory { get; set; }

        [JsonConverter(typeof(IntensityConverter))]
        [JsonPropertyName("intensity")]
        public Intensity Intensity { get; set; }

        [JsonConverter(typeof(LanguageConverter))]
        [JsonPropertyName("language")]
        public Language Language { get; set; }

        public UserModel(int id, bool isAdmin, string emailAddress, string password, string fullName, List<Genre> genres, AgeCategory ageCategory, Intensity intensity, Language language)
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