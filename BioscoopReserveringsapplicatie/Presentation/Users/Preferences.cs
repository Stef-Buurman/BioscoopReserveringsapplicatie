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

        private static bool _GenresNotFilledIn = false;
        private static bool _AgeCategoryNotFilledIn = false;
        private static bool _IntensityNotFilledIn = false;
        private static bool _LanguageNotFilledIn = false;

        private static readonly string _NotFilledIn = "Niet invullen";
        private static readonly string _StopFillingIn = "Stop";

        public static void Start(UserModel user, string returnTo = "")
        {
            Console.Clear();

            if (_user == null) _user = user;

            if ((returnTo == "" || returnTo == _returnToGenres) && !_GenresNotFilledIn) SelectGenres();
            if ((returnTo == "" || returnTo == _returnToAgeCategory) && !_AgeCategoryNotFilledIn) SelectAgeCategory();
            if ((returnTo == "" || returnTo == _returnToIntensity) && !_IntensityNotFilledIn) SelectIntensity();
            if ((returnTo == "" || returnTo == _returnToLanguage) && !_LanguageNotFilledIn) SelectLanguage();

            List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Inloggen", UserLogin.Start),
                };
            PrintEditedList();
            if (!PreferencesLogic.addPreferencesToAccount(_selectedGenres, _ageCategory, _intensity, _language, user))
            {
                options.Add(new Option<string>("Opniew proberen", () => Start(user)));
                ColorConsole.WriteColorLine("Er is een error opgetreden tijdens het toevoegen van de experience.", Globals.ErrorColor);
            }
            new SelectionMenuUtil2<string>(options).Create();
        }

        public static List<Genre> SelectGenres()
        {
            List<Genre> Genres = Globals.GetAllEnumIncludeUndefined<Genre>();
            List<Option<Genre>> availableGenres = new List<Option<Genre>>();
            bool firstTime = true;
            while (_selectedGenres.Count < Genres.Count - 1)
            {
                PrintEditedList();
                Genre genre;
                ColorConsole.WriteColorLine("Welkom op de voorkeur pagina", Globals.TitleColor);
                ColorConsole.WriteColorLine("Hier kunt u uw voorkeuren selecteren.\n", Globals.TitleColor);
                ColorConsole.WriteColorLine("Kies uw favoriete [genre]: \n", Globals.ColorInputcClarification);

                availableGenres.Clear();
                foreach (Genre option in Genres)
                {
                    if (_selectedGenres.Contains(option))  continue;
                    if (option == Genre.Undefined)
                    {
                        if (!firstTime)
                            availableGenres.Add(new Option<Genre>(option, _StopFillingIn));
                        else
                            availableGenres.Add(new Option<Genre>(option, _NotFilledIn));
                    }
                    else
                        availableGenres.Add(new Option<Genre>(option, option.GetDisplayName()));
                }
                genre = new SelectionMenuUtil2<Genre>(availableGenres, 9).Create();

                if (genre == Genre.Undefined)
                {
                    if (_selectedGenres.Count > 0)
                    {
                        _GenresNotFilledIn = false;
                    }
                    else
                    {
                        _GenresNotFilledIn = true;
                    }
                    break;
                }
                Option<Genre>? GenreIsInAvailable = availableGenres.Find(x => x.Value == genre);
                if (genre != default && GenreIsInAvailable != null)
                {
                    _selectedGenres.Add(genre);
                }
                else
                {
                    ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                }
                firstTime = false;
            }
            return _selectedGenres;
        }

        public static AgeCategory SelectAgeCategory()
        {
            PrintEditedList();
            List<AgeCategory> AgeCatagories = Globals.GetAllEnumIncludeUndefined<AgeCategory>();
            List<Option<AgeCategory>> options = new List<Option<AgeCategory>>();
            foreach (AgeCategory option in AgeCatagories)
            {
                if(option == AgeCategory.Undefined)
                    options.Add(new Option<AgeCategory>(option, _NotFilledIn));
                else 
                    options.Add(new Option<AgeCategory>(option, option.GetDisplayName()));
            }
            ColorConsole.WriteColorLine("Wat is uw [leeftijdscatagorie]: \n", Globals.ColorInputcClarification);
            SelectionMenuUtil2<AgeCategory> selectionMenu = new SelectionMenuUtil2<AgeCategory>(options, () =>
            {
                _GenresNotFilledIn = false;
                Start(_user, _returnToGenres);
            }, 
            () =>
            {
                _AgeCategoryNotFilledIn = false;
                Start(_user, _returnToAgeCategory);
            });
            _ageCategory = selectionMenu.Create();

            while (!PreferencesLogic.ValidateAgeCategory(_ageCategory))
            {
                ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                _ageCategory = selectionMenu.Create();
            }
            if (_ageCategory == AgeCategory.Undefined) _AgeCategoryNotFilledIn = true;
            return _ageCategory;
        }

        public static Intensity SelectIntensity()
        {
            PrintEditedList();
            List<Intensity> Intensities = Globals.GetAllEnumIncludeUndefined<Intensity>();
            List<Option<Intensity>> options = new List<Option<Intensity>>();
            foreach (Intensity option in Intensities)
            {
                if (option == Intensity.Undefined)
                    options.Add(new Option<Intensity>(option, _NotFilledIn));
                else
                    options.Add(new Option<Intensity>(option, option.GetDisplayName()));
            }
            ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification);
            SelectionMenuUtil2<Intensity> SelectionMenu = new SelectionMenuUtil2<Intensity>(options, () =>
            {
                _AgeCategoryNotFilledIn = false;
                Start(_user, _returnToAgeCategory);
            },
            () =>
            {
                _IntensityNotFilledIn = false;
                Start(_user, _returnToIntensity);
            });
            _intensity = SelectionMenu.Create();
            while (!PreferencesLogic.ValidateIntensity(_intensity))
            {
                ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                _intensity = SelectionMenu.Create();
            }
            if (_intensity == Intensity.Undefined) _IntensityNotFilledIn = true;
            return _intensity;
        }

        public static Language SelectLanguage()
        {
            PrintEditedList();
            List<Language> Intensities = Globals.GetAllEnumIncludeUndefined<Language>();
            List<Option<Language>> options = new List<Option<Language>>();
            foreach (Language option in Intensities)
            {
                if (option == Language.Undefined)
                    options.Add(new Option<Language>(option, _NotFilledIn));
                else
                    options.Add(new Option<Language>(option, option.GetDisplayName()));
            }
            SelectionMenuUtil2<Language> selectionMenu = new SelectionMenuUtil2<Language>(options, () =>
            {
                _IntensityNotFilledIn = false;
                Start(_user, _returnToIntensity);
            },
            () =>
            {
                _LanguageNotFilledIn = false;
                Start(_user, _returnToLanguage);
            });
            ColorConsole.WriteColorLine("Wat is uw [taal]? (What is your [language]?) \n", Globals.ColorInputcClarification);
            _language = selectionMenu.Create();

            while (!PreferencesLogic.ValidateLanguage(_language))
            {
                ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                _language = selectionMenu.Create();
            }
            if (_language == Language.Undefined) _LanguageNotFilledIn = true;
            return _language;
        }

        private static void PrintEditedList()
        {
            string NotFilledIn = "Niet ingevuld";
            Console.Clear();
            bool AnyOfTheFieldsFilledIn = _selectedGenres.Count > 0 || _ageCategory != AgeCategory.Undefined 
                || _intensity != Intensity.Undefined || _language != Language.Undefined 
                || _GenresNotFilledIn || _AgeCategoryNotFilledIn || _IntensityNotFilledIn 
                || _LanguageNotFilledIn;
            if (AnyOfTheFieldsFilledIn)
            {
                ColorConsole.WriteColorLine("[Aangepaste voorkeuren]", Globals.ExperienceColor);
            }
            if (_selectedGenres.Count > 0)
            {
                ColorConsole.WriteColorLine($"[Genres:] {string.Join(", ", _selectedGenres)}", Globals.ExperienceColor);
            }else if (_GenresNotFilledIn)
            {
                ColorConsole.WriteColorLine($"[Genres:] {NotFilledIn}", Globals.ExperienceColor);
            }
            if (_ageCategory != AgeCategory.Undefined)
            {
                ColorConsole.WriteColorLine($"[leeftijdscatagorie:] {_ageCategory.GetDisplayName()}", Globals.ExperienceColor);
            }
            else if (_AgeCategoryNotFilledIn)
            {
                ColorConsole.WriteColorLine($"[leeftijdscatagorie:] {NotFilledIn}", Globals.ExperienceColor);
            }
            if (_intensity != Intensity.Undefined)
            {
                ColorConsole.WriteColorLine($"[intensiteit:] {_intensity.GetDisplayName()}", Globals.ExperienceColor);
            }
            else if (_IntensityNotFilledIn)
            {
                ColorConsole.WriteColorLine($"[intensiteit:] {NotFilledIn}", Globals.ExperienceColor);
            }
            if (_language != Language.Undefined)
            {
                ColorConsole.WriteColorLine($"[Taal (Language):] {_language.GetDisplayName()} minuten", Globals.ExperienceColor);
            }
            else if (_LanguageNotFilledIn)
            {
                ColorConsole.WriteColorLine($"[Taal (Language):] {NotFilledIn}", Globals.ExperienceColor);
            }
            if (AnyOfTheFieldsFilledIn)
            {
                ColorConsole.WriteColorLine("---------------------------------------------------------------", ConsoleColor.White);
            }
        }
    }
}