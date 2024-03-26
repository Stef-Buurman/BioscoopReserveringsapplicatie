namespace BioscoopReserveringsapplicatie
{
    public static class Preferences
    {
        public static UserLogic PreferencesLogic = new UserLogic();
        public static void Start()
        {
            Console.Clear();

            List<string> selectedGenres = SelectGenres();
            int ageCategory = SelectAgeCategory();
            string intensity = SelectIntensity();
            string language = SelectLanguage();

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
            List<string> selectedGenres = new List<string>();
            List<string> availableGenres = new List<string>
            {
                "Horror", "Komedie", "Actie", "Drama", "Thriller", "Romantiek", "Sci-fi",
                "Fantasie", "Avontuur", "Animatie", "Misdaad", "Mysterie", "Familie",
                "Oorlog", "Geschiedenis", "Muziek", "Documentaire", "Westers", "TV-film"
            };
            bool firstTime = true;
            while (selectedGenres.Count < 3)
            {
                string genre = "";
                if (firstTime)
                {
                    genre = SelectionMenu.Create(availableGenres, () =>
                    {
                        ColorConsole.WriteColorLine("[Welkom op de voorkeur pagina]", Globals.TitleColor);
                        ColorConsole.WriteColorLine("[Hier kunt u uw voorkeuren selecteren.]\n", Globals.TitleColor);
                        ColorConsole.WriteColorLine("Kies uw favoriete [genre]: \n", Globals.ColorInputcClarification);
                    }
                    );
                }
                else
                {
                    genre = SelectionMenu.Create(availableGenres, () => ColorConsole.WriteColorLine("Kies uw favoriete [genre]: \n", Globals.ColorInputcClarification));
                }

                if (!string.IsNullOrWhiteSpace(genre) && availableGenres.Contains(genre))
                {
                    availableGenres.Remove(genre);
                    selectedGenres.Add(genre);
                }
                else
                {
                    Console.WriteLine("Error. Probeer het opnieuw.");
                }
                firstTime = false;
            }

            return selectedGenres;
        }

        public static int SelectAgeCategory()
        {
            List<int> options = new List<int> { 6, 9, 12, 14, 16, 18 };

            int ageCategory = SelectionMenu.Create(options, () => ColorConsole.WriteColorLine("Wat is uw [leeftijdscatagorie]: \n", Globals.ColorInputcClarification));
            while (!PreferencesLogic.ValidateAgeCategory(ageCategory))
            {
                Console.WriteLine("Error. Probeer het opnieuw.");
                ageCategory = SelectionMenu.Create(options, () => ColorConsole.WriteColorLine("Wat is uw [leeftijdscatagorie]: \n", Globals.ColorInputcClarification));
            }

            return ageCategory;
        }

        public static string SelectIntensity()
        {
            List<string> options = new List<string> { "Laag", "Medium", "Hoog" };
            string intensity = SelectionMenu.Create(options, () => ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification));

            while (!PreferencesLogic.ValidateIntensity(intensity))
            {
                Console.WriteLine("Error. Probeer het opnieuw.");
                intensity = SelectionMenu.Create(options, () => ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification));
            }

            return intensity;
        }

        public static string SelectLanguage()
        {
            List<string> options = new List<string> { "English", "Nederlands" };
            string language = SelectionMenu.Create(options, () => ColorConsole.WriteColorLine("Wat is uw [taal]? (What is your [language]?): \n", Globals.ColorInputcClarification));

            while (!PreferencesLogic.ValidateLanguage(language))
            {
                Console.WriteLine("Error. Probeer het opnieuw.");
                language = SelectionMenu.Create(options, () => ColorConsole.WriteColorLine("Wat is uw [taal]? (What is your [language]?): \n", Globals.ColorInputcClarification));
            }

            return language;
        }
    }
}