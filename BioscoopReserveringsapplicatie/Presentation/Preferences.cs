namespace BioscoopReserveringsapplicatie
{
    public static class Preferences
    {
        public static UserLogic PreferencesLogic = new UserLogic();
        public static void Start()
        {
            Console.Clear();
            Console.WriteLine("Welkom op de voorkeur pagina\n");
            Console.WriteLine("Hier kunt u uw voorkeuren selecteren.\n");

            var selectedGenres = SelectGenres();
            var ageCategory = SelectAgeCategory();
            var intensity = SelectIntensity();
            var language = SelectLanguage();

            Console.Clear();
            Console.WriteLine("Dit zijn uw voorkeuren:\n");
            Console.WriteLine($"Genres: {string.Join(", ", selectedGenres)}");
            Console.WriteLine($"Kijkwijzer: {ageCategory}");
            Console.WriteLine($"Intensiteit: {intensity}");
            Console.WriteLine($"Taal: {language}");

            PreferencesLogic.addPreferencesToAccount(selectedGenres, ageCategory, intensity, language);
        }

        public static List<string> SelectGenres()
        {
            var selectedGenres = new List<string>();
            var availableGenres = new List<string>
        {
            "Horror", "Komedie", "Actie", "Drama", "Thriller", "Romantiek", "Sci-fi",
            "Fantasie", "Avontuur", "Animatie", "Misdaad", "Mysterie", "Familie",
            "Oorlog", "Geschiedenis", "Muziek", "Documentaire", "Westers", "TV-film"
        };

            while (selectedGenres.Count < 3)
            {
                Console.WriteLine("Selecteer een genre:");
                var genre = SelectionMenu.Create(availableGenres);

                if (!string.IsNullOrWhiteSpace(genre) && availableGenres.Contains(genre))
                {
                    availableGenres.Remove(genre);
                    selectedGenres.Add(genre);
                }
                else
                {
                    Console.WriteLine("Error. Probeer het opnieuw.");
                }
            }

            return selectedGenres;
        }

        public static int SelectAgeCategory()
        {
            var options = new List<int> { 6, 9, 12, 14, 16, 18 };

            int ageCategory = SelectionMenu.Create(options);
            while (!PreferencesLogic.ValidateAgeCategory(ageCategory))
            {
                Console.WriteLine("Error. Probeer het opnieuw.");
                ageCategory = SelectionMenu.Create(options);
            }

            return ageCategory;
        }

        public static string SelectIntensity()
        {
            var options = new List<string> { "Laag", "Medium", "Hoog" };
            string intensity = SelectionMenu.Create(options);

            while (!PreferencesLogic.ValidateIntensity(intensity))
            {
                Console.WriteLine("Error. Probeer het opnieuw.");
                intensity = SelectionMenu.Create(options);
            }

            return intensity;
        }

        public static string SelectLanguage()
        {
            var options = new List<string> { "Engels", "Nederlands" };
            string language = SelectionMenu.Create(options);

            while (!PreferencesLogic.ValidateLanguage(language))
            {
                Console.WriteLine("Error. Probeer het opnieuw.");
                language = SelectionMenu.Create(options);
            }

            return language;
        }
    }
}