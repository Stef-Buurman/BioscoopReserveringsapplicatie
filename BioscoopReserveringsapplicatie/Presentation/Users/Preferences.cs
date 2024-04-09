namespace BioscoopReserveringsapplicatie
{
    public static class Preferences
    {
        public static UserLogic PreferencesLogic = new UserLogic();
        public static void Start(UserModel user)
        {
            Console.Clear();


            List<Genre> selectedGenres = SelectGenres();
            AgeCategory ageCategory = SelectAgeCategory();
            Intensity intensity = SelectIntensity();

            Language language = SelectLanguage();

            PreferencesLogic.addPreferencesToAccount(selectedGenres, ageCategory, intensity, language, user);
        }

        public static List<Genre> SelectGenres()
        {
            List<Genre> selectedGenres = new List<Genre>();

            bool choose = false;
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    choose = true;
                }),
                new Option<string>("Nee", () => {
                    choose = false;
                }),
            };
            SelectionMenuUtil.Create(options, () => Console.WriteLine("Wilt u genres selecteren?"));

            if (!choose)
            {
                return selectedGenres;
            }

            List<Genre> availableGenres = Globals.GetAllEnum<Genre>();
            bool firstTime = true;

            while (selectedGenres.Count < 3)
            {
                Genre genre;
                if (firstTime)
                {
                    genre = SelectionMenuUtil.Create(availableGenres, () =>
                    {
                        ColorConsole.WriteColorLine("[Welkom op de voorkeur pagina]", Globals.TitleColor);
                        ColorConsole.WriteColorLine("[Hier kunt u uw voorkeuren selecteren.]\n", Globals.TitleColor);
                        ColorConsole.WriteColorLine("Kies uw favoriete [genre]: \n", Globals.ColorInputcClarification);
                    }
                    );
                }
                else
                {
                    genre = SelectionMenuUtil.Create(availableGenres, () => ColorConsole.WriteColorLine("Kies uw favoriete [genre]: \n", Globals.ColorInputcClarification));
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

        public static AgeCategory SelectAgeCategory()
        {
            bool choose = false;
            List<Option<string>> optionsMenu = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    choose = true;
                }),
                new Option<string>("Nee", () => {
                    choose = false;
                }),
            };
            SelectionMenuUtil.Create(optionsMenu, () => Console.WriteLine("Wilt u een leeftijdscategorie selecteren?"));

            if (!choose)
            {
                return AgeCategory.Undefined;
            }

            List<AgeCategory> options = Globals.GetAllEnum<AgeCategory>();
            List<string> EnumDescription = options.Select(o => o.GetDisplayName()).ToList();
            string selectedDescription = SelectionMenuUtil.Create(EnumDescription, () => ColorConsole.WriteColorLine("Wat is uw [leeftijdscatagorie]: \n", Globals.ColorInputcClarification));
            AgeCategory ageCategory = options.First(o => o.GetDisplayName() == selectedDescription);


            while (!PreferencesLogic.ValidateAgeCategory(ageCategory))
            {
                Console.WriteLine("Error. Probeer het opnieuw.");
                selectedDescription = SelectionMenuUtil.Create(EnumDescription, () => ColorConsole.WriteColorLine("Wat is uw [leeftijdscatagorie]: \n", Globals.ColorInputcClarification));
                ageCategory = options.First(o => o.GetDisplayName() == selectedDescription);

            }

            return ageCategory;
        }

        public static Intensity SelectIntensity()
        {
            bool choose = false;
            List<Option<string>> optionsMenu = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    choose = true;
                }),
                new Option<string>("Nee", () => {
                    choose = false;
                }),
            };
            SelectionMenuUtil.Create(optionsMenu, () => Console.WriteLine("Wilt u een intensiteit selecteren?"));

            if (!choose)
            {
                return Intensity.Undefined;
            }

            List<Intensity> options = Globals.GetAllEnum<Intensity>();
<<<<<<< HEAD:BioscoopReserveringsapplicatie/Presentation/Users/Preferences.cs
            Intensity intensity = SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification));
=======

            Intensity intensity = SelectionMenu.Create(options, () => ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification));
>>>>>>> 06b9602 (Preferences are now optional to enter):BioscoopReserveringsapplicatie/Presentation/Preferences.cs

            while (!PreferencesLogic.ValidateIntensity(intensity))
            {
                Console.WriteLine("Error. Probeer het opnieuw.");
                intensity = SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification));
            }

            return intensity;
        }

        public static Language SelectLanguage()
        {
            List<Language> options = Globals.GetAllEnum<Language>();
            Language language = SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Wat is uw [taal]? (What is your [language]?): \n", Globals.ColorInputcClarification));

            while (!PreferencesLogic.ValidateLanguage(language))
            {
                Console.WriteLine("Error. Probeer het opnieuw.");
                language = SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Wat is uw [taal]? (What is your [language]?): \n", Globals.ColorInputcClarification));
            }

            return language;
        }
    }
}