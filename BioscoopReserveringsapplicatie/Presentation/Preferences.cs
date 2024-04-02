namespace BioscoopReserveringsapplicatie
{
    public static class Preferences
    {
        public static UserLogic PreferencesLogic = new UserLogic();
        public static void Start()
        {
            Console.Clear();

            List<Genre> selectedGenres = SelectGenres();
            int ageCategory = SelectAgeCategory();
            Intensity intensity = SelectIntensity();
            string language = SelectLanguage();

            Console.Clear();
            Console.WriteLine("Uw geselecteerde voorkeuren zijn:\n");
            Console.WriteLine($"Genres: {string.Join(", ", selectedGenres)}");
            Console.WriteLine($"Leeftijdscategorie: {ageCategory}");
            Console.WriteLine($"Intensiteit: {intensity}");
            Console.WriteLine($"Taal: {language}");

            PreferencesLogic.addPreferencesToAccount(selectedGenres, ageCategory, intensity, language);
        }

        public static List<Genre> SelectGenres()
        {
            List<Genre> selectedGenres = new List<Genre>();
            List<Genre> availableGenres = Globals.GetAllEnum<Genre>();
            bool firstTime = true;
            while (selectedGenres.Count < 3)
            {
                Genre genre;
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

                if (genre != default && availableGenres.Contains(genre))
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

        public static Intensity SelectIntensity()
        {
            List<Intensity> options = Globals.GetAllEnum<Intensity>();
            Intensity intensity = SelectionMenu.Create(options, () => ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification));

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