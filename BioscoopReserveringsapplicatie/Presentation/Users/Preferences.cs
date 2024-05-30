using System.Collections.Generic;
using System;

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

        private static bool showGenresPreference = false;

        public static void Start(UserModel user, string returnTo = "")
        {
            Console.Clear();

            if (_user == null) _user = user;

            if ((returnTo == "" || returnTo == _returnToGenres) && !_GenresNotFilledIn)
            {
                SelectGenres();
                returnTo = "";
            }
            if ((returnTo == "" || returnTo == _returnToAgeCategory) && !_AgeCategoryNotFilledIn)
            {
                SelectAgeCategory();
                returnTo = "";
            }
            if ((returnTo == "" || returnTo == _returnToIntensity) && !_IntensityNotFilledIn)
            {
                SelectIntensity();
                returnTo = "";
            }
            if ((returnTo == "" || returnTo == _returnToLanguage) && !_LanguageNotFilledIn)
            {
                SelectLanguage();
                returnTo = "";
            }

            PrintEditedList();

            if (!PreferencesLogic.addPreferencesToAccount(_selectedGenres, _ageCategory, _intensity, _language, user))
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Opnieuw proberen", () => Start(user))
                };
                ColorConsole.WriteColorLine("Er is een error opgetreden tijdens het toevoegen van de voorkeuren aan uw account.", Globals.ErrorColor);
                new SelectionMenuUtil<string>(options).Create();
            }
            else
            {
                _selectedGenres = new List<Genre>();
                _ageCategory = AgeCategory.Undefined;
                _intensity = Intensity.Undefined;
                _language = Language.Undefined;
                ColorConsole.WriteColorLine("\nU bent klaar met het instellen van uw account en kunt nu inloggen.", Globals.SuccessColor);

                WaitUtil.WaitTime(4000);

                UserLogin.Start();
            }
        }

        public static void SelectGenres()
        {
            showGenresPreference = false;
            PrintEditedList();
            ColorConsole.WriteColorLine("Kies een of meerdere [genre(s)]: \n", Globals.ColorInputcClarification);
            List<Genre> Genres = Globals.GetAllEnum<Genre>();
            List<Option<Genre>> availableGenres = new List<Option<Genre>>();
            List<Option<Genre>> selectedGenres = new List<Option<Genre>>();

            foreach (Genre option in Genres)
            {
                if (_selectedGenres.Contains(option))
                {
                    selectedGenres.Add(new Option<Genre>(option, option.GetDisplayName()));
                }
                availableGenres.Add(new Option<Genre>(option, option.GetDisplayName()));
            }

            _selectedGenres = new SelectionMenuUtil<Genre>(availableGenres, selectedGenres).CreateMultiSelect(out selectedGenres);

            showGenresPreference = true;
        }

        public static void SelectAgeCategory()
        {
            PrintEditedList();
            List<AgeCategory> AgeCatagories = Globals.GetAllEnum<AgeCategory>();
            List<Option<AgeCategory>> options = new List<Option<AgeCategory>>();
            foreach (AgeCategory option in AgeCatagories)
            {
                options.Add(new Option<AgeCategory>(option, option.GetDisplayName()));
            }
            ColorConsole.WriteColorLine("Kies een [leeftijdscategorie]: \n", Globals.ColorInputcClarification);
            SelectionMenuUtil<AgeCategory> selectionMenu = new SelectionMenuUtil<AgeCategory>(options, () =>
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
        }

        public static void SelectIntensity()
        {
            PrintEditedList();
            List<Intensity> Intensities = Globals.GetAllEnum<Intensity>();
            List<Option<Intensity>> options = new List<Option<Intensity>>();
            foreach (Intensity option in Intensities)
            {
                options.Add(new Option<Intensity>(option, option.GetDisplayName()));
            }
            ColorConsole.WriteColorLine("Kies een [intensiteit]: \n", Globals.ColorInputcClarification);
            SelectionMenuUtil<Intensity> SelectionMenu = new SelectionMenuUtil<Intensity>(options, () =>
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
        }

        public static void SelectLanguage()
        {
            PrintEditedList();
            List<Language> Intensities = Globals.GetAllEnum<Language>();
            List<Option<Language>> options = new List<Option<Language>>();
            foreach (Language option in Intensities)
            {
                options.Add(new Option<Language>(option, option.GetDisplayName()));
            }
            SelectionMenuUtil<Language> selectionMenu = new SelectionMenuUtil<Language>(options, () =>
            {
                _IntensityNotFilledIn = false;
                Start(_user, _returnToIntensity);
            },
            () =>
            {
                _LanguageNotFilledIn = false;
                Start(_user, _returnToLanguage);
            });
            ColorConsole.WriteColorLine("Kies een [taal] (Choose a [language]) \n", Globals.ColorInputcClarification);
            _language = selectionMenu.Create();

            while (!PreferencesLogic.ValidateLanguage(_language))
            {
                ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                _language = selectionMenu.Create();
            }
            if (_language == Language.Undefined) _LanguageNotFilledIn = true;
        }

        private static void PrintEditedList()
        {
            string NotFilledIn = "Niet ingevuld";
            Console.Clear();
            bool AnyOfTheFieldsFilledIn = _selectedGenres.Count >= 0 || _ageCategory != AgeCategory.Undefined
                || _intensity != Intensity.Undefined || _language != Language.Undefined
                || _GenresNotFilledIn || _AgeCategoryNotFilledIn || _IntensityNotFilledIn
                || _LanguageNotFilledIn;
            if (AnyOfTheFieldsFilledIn)
            {
                ColorConsole.WriteColorLine("[Uw voorkeuren]", Globals.ExperienceColor);
            }
            if (_selectedGenres.Count > 0)
            {
                ColorConsole.WriteColorLine($"[Genre(s):] {string.Join(", ", _selectedGenres)}", Globals.ExperienceColor);
            }
            else if (_selectedGenres.Count == 0 && showGenresPreference)
            {
                ColorConsole.WriteColorLine($"[Genre(s):] {NotFilledIn}", Globals.ExperienceColor);
            }
            if (_ageCategory != AgeCategory.Undefined)
            {
                ColorConsole.WriteColorLine($"[Leeftijdscategorie:] {_ageCategory.GetDisplayName()}", Globals.ExperienceColor);
            }
            else if (_AgeCategoryNotFilledIn)
            {
                ColorConsole.WriteColorLine($"[Leeftijdscategorie:] {NotFilledIn}", Globals.ExperienceColor);
            }
            if (_intensity != Intensity.Undefined)
            {
                ColorConsole.WriteColorLine($"[Intensiteit:] {_intensity.GetDisplayName()}", Globals.ExperienceColor);
            }
            else if (_IntensityNotFilledIn)
            {
                ColorConsole.WriteColorLine($"[Intensiteit:] {NotFilledIn}", Globals.ExperienceColor);
            }
            if (_language != Language.Undefined)
            {
                ColorConsole.WriteColorLine($"[Taal (Language):] {_language.GetDisplayName()}", Globals.ExperienceColor);
            }
            else if (_LanguageNotFilledIn)
            {
                ColorConsole.WriteColorLine($"[Taal (Language):] {NotFilledIn}", Globals.ExperienceColor);
            }
            if (AnyOfTheFieldsFilledIn)
            {
                HorizontalLine.Print();
            }
        }
    }
}