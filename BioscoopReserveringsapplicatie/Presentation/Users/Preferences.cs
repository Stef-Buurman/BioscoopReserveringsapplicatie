namespace BioscoopReserveringsapplicatie
{
    public static class Preferences
    {
        public static UserLogic PreferencesLogic = new UserLogic();

        private static List<Genre> _selectedGenres = new List<Genre>();
        private static AgeCategory _ageCategory = AgeCategory.Undefined;
        private static Intensity _intensity = Intensity.Undefined;
        private static Language _language = Language.Undefined;

        private static UserModel _user;

        private static readonly string _returnToGenres = "Genres";
        private static readonly string _returnToAgeCategory = "AgeCategory";
        private static readonly string _returnToIntensity = "Intensity";
        private static readonly string _returnToLanguage = "Language";

        public static void Start(UserModel user, string returnTo = "")
        {
            Console.Clear();

            if (_user == null) _user = user;

            if (returnTo == "" && returnTo == _returnToGenres) SelectGenres();
            if (returnTo == "" && returnTo == _returnToAgeCategory) SelectAgeCategory();
            if (returnTo == "" && returnTo == _returnToIntensity) SelectIntensity();
            if (returnTo == "" && returnTo == _returnToLanguage) SelectLanguage();

            PreferencesLogic.addPreferencesToAccount(_selectedGenres, _ageCategory, _intensity, _language, user);
        }

        public static List<Genre> SelectGenres()
        {
            PrintEditedList();
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
            SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Wilt u [genres] selecteren?", Globals.ColorInputcClarification));

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
                        ColorConsole.WriteColorLine("Welkom op de voorkeur pagina", Globals.TitleColor);
                        ColorConsole.WriteColorLine("Hier kunt u uw voorkeuren selecteren.\n", Globals.TitleColor);
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
                    ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                }
                firstTime = false;
            }

            return selectedGenres;
        }

        public static AgeCategory SelectAgeCategory()
        {
            PrintEditedList();
            List<AgeCategory> AgeCatagories = Globals.GetAllEnumIncludeUndefined<AgeCategory>();
            List<Option<AgeCategory>> options = new List<Option<AgeCategory>>();
            foreach (AgeCategory option in AgeCatagories)
            {
                if(option == AgeCategory.Undefined)
                    options.Add(new Option<AgeCategory>(option, "Niet invullen"));
                else 
                    options.Add(new Option<AgeCategory>(option, option.GetDisplayName()));
            }
            ColorConsole.WriteColorLine("Wat is uw [leeftijdscatagorie]: \n", Globals.ColorInputcClarification);
            SelectionMenuUtil2<AgeCategory> selectionMenu = new SelectionMenuUtil2<AgeCategory>(options);
            AgeCategory ageCategory = selectionMenu.Create();

            while (!PreferencesLogic.ValidateAgeCategory(ageCategory))
            {
                ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                ageCategory = selectionMenu.Create();
            }
            return ageCategory;
        }

        public static Intensity SelectIntensity()
        {
            PrintEditedList();
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
            SelectionMenuUtil.Create(optionsMenu, () => ColorConsole.WriteColorLine("Wilt u een [intensiteit] selecteren?", Globals.ColorInputcClarification));

            if (!choose)
            {
                return Intensity.Undefined;
            }

            List<Intensity> options = Globals.GetAllEnum<Intensity>();
            Intensity intensity = SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification));

            while (!PreferencesLogic.ValidateIntensity(intensity))
            {
                ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                intensity = SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification));
            }

            return intensity;
        }

        public static Language SelectLanguage()
        {
            PrintEditedList();
            List<Language> options = Globals.GetAllEnum<Language>();
            Language language = SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Wat is uw [taal]? (What is your [_language]?) \n", Globals.ColorInputcClarification));

            while (!PreferencesLogic.ValidateLanguage(language))
            {
                ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                language = SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Wat is uw [taal]? (What is your [_language]?) \n", Globals.ColorInputcClarification));
            }

            return language;
        }

        private static void PrintEditedList()
        {
            Console.Clear();
            //if (_newName != "" || _selectedMovieId != 0 || _Intensity != Intensity.Undefined || _timeInInt != 0)
            //{
            //    ColorConsole.WriteColorLine("[Huidige Experience Details]", Globals.ExperienceColor);
            //}
            if (_selectedGenres.Count > 0)
            {
                ColorConsole.WriteColorLine($"[Naam experience:] {string.Join(", ", _selectedGenres)}", Globals.ExperienceColor);
            }
            if (_ageCategory != AgeCategory.Undefined)
            {
                ColorConsole.WriteColorLine($"[Film titel:] {_ageCategory.GetDisplayName()}", Globals.ExperienceColor);
            }
            if (_intensity != Intensity.Undefined)
            {
                ColorConsole.WriteColorLine($"[Intensiteit experience:] {_intensity.GetDisplayName()}", Globals.ExperienceColor);
            }
            if (_language != Language.Undefined)
            {
                ColorConsole.WriteColorLine($"[Tijdsduur experience:] {_language.GetDisplayName()} minuten", Globals.ExperienceColor);
            }
            //if (_newName != "" || _selectedMovieId != 0 || _Intensity != Intensity.Undefined || _timeInInt != 0)
            //{
            //    ColorConsole.WriteColorLine("---------------------------------------------------------------", ConsoleColor.White);
            //}
        }
    }
}