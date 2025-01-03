﻿using System.Text.Json.Serialization;
using System.Xml;

namespace BioscoopReserveringsapplicatie
{
    public class UserModel : IID
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("isAdmin")]
        public bool IsAdmin { get; }

        [JsonPropertyName("emailAddress")]
        public string EmailAddress { get; set; }

        [JsonPropertyName("password")]
        public string Password { get; set; }

        [JsonPropertyName("salt")]
        public byte[] Salt { get; set; }

        [JsonPropertyName("fullName")]
        public string FullName { get; set; }

        [JsonConverter(typeof(EnumListConverter<Genre>))]
        [JsonPropertyName("genres")]
        public List<Genre> Genres { get; set; }

        [JsonConverter(typeof(EnumConverter<AgeCategory>))]
        [JsonPropertyName("ageCategory")]
        public AgeCategory AgeCategory { get; set; }

        [JsonConverter(typeof(EnumConverter<Intensity>))]
        [JsonPropertyName("intensity")]
        public Intensity Intensity { get; set; }

        [JsonConverter(typeof(EnumConverter<Language>))]
        [JsonPropertyName("language")]
        public Language Language { get; set; }

        [JsonPropertyName("promotionsSeen")]
        public Dictionary<int, DateTime> PromotionsSeen { get; set; }

        public UserModel(int id, bool isAdmin, string emailAddress, string password, byte[] salt, string fullName, List<Genre> genres, AgeCategory ageCategory, Intensity intensity, Language language, Dictionary<int, DateTime> promotionsSeen)
        {
            Id = id;
            IsAdmin = isAdmin;
            EmailAddress = emailAddress;
            Password = password;
            Salt = salt;
            FullName = fullName;
            Genres = genres;
            AgeCategory = ageCategory;
            Intensity = intensity;
            Language = language;
            PromotionsSeen = promotionsSeen;
        }
    }
}