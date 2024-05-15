namespace BioscoopReserveringsapplicatie
{
    static class UserDetailsEdit
    {
        private static UserLogic _userLogic = new UserLogic();

        private static string _newName = "";
        private static string _newEmail = "";
        private static List<Genre> _newGenres = new List<Genre>();
        private static AgeCategory _newAgeCategory = AgeCategory.Undefined;
        private static Intensity _newIntensity = Intensity.Undefined;
        private static Language _language = Language.Undefined;

        private static string _returnToName = "Name";
        private static string _returnToEmail = "Email";
        private static string _returnToGenres = "Genres";
        private static string _returnToAgeCategory = "Rating";
        private static string _returnToIntensity = "Intensity";
        private static string _returnToLanguage = "Language";

        private static bool _GenresNotFilledIn = false;
        private static bool _AgeCategoryNotFilledIn = false;
        private static bool _IntensityNotFilledIn = false;
        private static bool _LanguageNotFilledIn = false;

        private static readonly string _NotFilledIn = "Niet aanpassen";

        public static void Start(string returnTo = "")
        {
            if (UserLogic.CurrentUser == null) return;
            if (_newName == "") _newName = UserLogic.CurrentUser.FullName;
            if (_newEmail == "") _newEmail = UserLogic.CurrentUser.EmailAddress;
            if (_newGenres.Count == 0) _newGenres = UserLogic.CurrentUser.Genres;
            if (_newAgeCategory == AgeCategory.Undefined) _newAgeCategory = UserLogic.CurrentUser.AgeCategory;
            if (_newIntensity == Intensity.Undefined) _newIntensity = UserLogic.CurrentUser.Intensity;
            if (_language == Language.Undefined) _language = UserLogic.CurrentUser.Language;

            if (returnTo == "" || returnTo == _returnToName)
            {
                UserName();
                returnTo = "";
            }
            if (returnTo == "" || returnTo == _returnToEmail)
            {
                UserEmail();
                returnTo = "";
            }
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
            ColorConsole.WriteColorLine("Weet je zeker dat je deze wijzigingen wilt opslaan?\n", Globals.ColorInputcClarification);
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    NotFilledInToFalse();
                    Result<UserModel> result = _userLogic.Edit(_newName, _newEmail, _newGenres, _newIntensity, _newAgeCategory);
                    if(result.IsValid)
                    {
                        ColorConsole.WriteColorLine("\nGebruikersgegevens zijn gewijzigd!", Globals.SuccessColor);
                        Thread.Sleep(2000);
                        UserDetails.Start();
                    }
                    else
                    {
                        ColorConsole.WriteColorLine(result.ErrorMessage, Globals.ErrorColor);
                        List<Option<string>> options = new List<Option<string>>
                        {
                            new Option<string>("Terug", () => {UserDetails.Start();}),
                        };
                        ColorConsole.WriteColorLine("\nEr is een fout opgetreden tijdens het bewerken van uw gebruikersgegevens. Probeer het opnieuw.\n", Globals.ErrorColor);
                        new SelectionMenuUtil<string>(options).Create();
                    }
                }),
                new Option<string>("Nee, pas mijn gegevens aan", 
                ()=>{
                    NotFilledInToFalse();
                    Start(_returnToLanguage);
                }),
                new Option<string>("Nee, terug naar mijn details", 
                () => {
                    NotFilledInToFalse();
                    UserDetails.Start();
                })
            };
            new SelectionMenuUtil<string>(options).Create();
        }

        private static void NotFilledInToFalse()
        {
            _GenresNotFilledIn = false;
            _AgeCategoryNotFilledIn = false;
            _IntensityNotFilledIn = false;
            _LanguageNotFilledIn = false;
        }

        private static void UserName()
        {
            PrintEditedList();
            bool validName = false;
            while (!validName)
            {
                _newName = ReadLineUtil.EditValue(_newName, "Voer uw [naam] in: ",
                    () => { UserDetails.Start(); },
                    "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n"
                );
                validName = _userLogic.ValidateName(_newName);
            }
        }

        private static void UserEmail()
        {
            bool validEmail = false;
            while (!validEmail)
            {
                _newEmail = ReadLineUtil.EditValue(_newEmail, "Voer uw [emailadres] in: ",
                    () => Start(_returnToName),
                    "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n",
                false, false);
                validEmail = _userLogic.ValidateEmail(_newEmail);
            }
        }

        public static void SelectGenres()
        {
            PrintEditedList();
            List<Genre> Genres = Globals.GetAllEnum<Genre>();
            List<Option<Genre>> availableGenres = new List<Option<Genre>>();
            List<Option<Genre>> selectedGenres = new List<Option<Genre>>();

            foreach (Genre option in Genres)
            {
                if (_newGenres.Contains(option))
                {
                    selectedGenres.Add(new Option<Genre>(option, option.GetDisplayName()));
                }
                availableGenres.Add(new Option<Genre>(option, option.GetDisplayName()));
            }

            _newGenres = new SelectionMenuUtil<Genre>(availableGenres, 9,
                    () =>
                    {
                        _GenresNotFilledIn = false;
                        Start(_returnToEmail);
                    },
                    () =>
                    {
                        _GenresNotFilledIn = false;
                        Start(_returnToGenres);
                    }, "Kies uw favoriete [genre]: ", selectedGenres).CreateMultiSelect();
        }

        public static void SelectAgeCategory()
        {
            PrintEditedList();
            List<AgeCategory> AgeCatagories = Globals.GetAllEnumIncludeUndefined<AgeCategory>();
            List<Option<AgeCategory>> options = new List<Option<AgeCategory>>();
            foreach (AgeCategory option in AgeCatagories)
            {
                if (option == AgeCategory.Undefined)
                    options.Add(new Option<AgeCategory>(option, _NotFilledIn));
                else
                    options.Add(new Option<AgeCategory>(option, option.GetDisplayName()));
            }
            ColorConsole.WriteColorLine("Wat is uw [leeftijdscatagorie]: \n", Globals.ColorInputcClarification);
            SelectionMenuUtil<AgeCategory> selectionMenu = new SelectionMenuUtil<AgeCategory>(options, () =>
            {
                _GenresNotFilledIn = false;
                Start(_returnToGenres);
            },
            () =>
            {
                _AgeCategoryNotFilledIn = false;
                Start(_returnToAgeCategory);
            }, new Option<AgeCategory>(_newAgeCategory));
            _newAgeCategory = selectionMenu.Create();

            while (!_userLogic.ValidateAgeCategory(_newAgeCategory))
            {
                ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                _newAgeCategory = selectionMenu.Create();
            }
            if (_newAgeCategory == AgeCategory.Undefined)
            {
                _AgeCategoryNotFilledIn = true;
                _newAgeCategory = UserLogic.CurrentUser.AgeCategory;
            }
        }

        public static void SelectIntensity()
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
            SelectionMenuUtil<Intensity> SelectionMenu = new SelectionMenuUtil<Intensity>(options, () =>
            {
                _AgeCategoryNotFilledIn = false;
                Start(_returnToAgeCategory);
            },
            () =>
            {
                _IntensityNotFilledIn = false;
                Start(_returnToIntensity);
            }, new Option<Intensity>(_newIntensity));
            _newIntensity = SelectionMenu.Create();
            while (!_userLogic.ValidateIntensity(_newIntensity))
            {
                ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                _newIntensity = SelectionMenu.Create();
            }
            if (_newIntensity == Intensity.Undefined)
            {
                _IntensityNotFilledIn = true;
                _newIntensity = UserLogic.CurrentUser.Intensity;
            }
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
            SelectionMenuUtil<Language> selectionMenu = new SelectionMenuUtil<Language>(options, () =>
            {
                _IntensityNotFilledIn = false;
                Start(_returnToIntensity);
            },
            () =>
            {
                _LanguageNotFilledIn = false;
                Start(_returnToLanguage);
            }, new Option<Language>(_language));
            ColorConsole.WriteColorLine("Wat is uw [taal]? (What is your [language]?) \n", Globals.ColorInputcClarification);
            _language = selectionMenu.Create();

            while (!_userLogic.ValidateLanguage(_language))
            {
                ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                _language = selectionMenu.Create();
            }
            if (_language == Language.Undefined)
            {
                _LanguageNotFilledIn = true;
                _language = UserLogic.CurrentUser.Language;
            }
            return _language;
        }

        private static void PrintEditedList()
        {
            string notFilledIn = "Niet Ingevuld";
            Console.Clear();
            bool AnyOfTheFieldsFilledIn = _newGenres.Count > 0 || _newAgeCategory != AgeCategory.Undefined
                || _newIntensity != Intensity.Undefined || _language != Language.Undefined
                || _newName != "" || _newEmail != "";
            if (AnyOfTheFieldsFilledIn)
            {
                ColorConsole.WriteColorLine("[Huidige Account Details]", Globals.ExperienceColor);
            }
            if (_newName != "")
            {
                ColorConsole.WriteColorLine($"[Naam:] {_newName}", Globals.ExperienceColor);
            }
            if (_newEmail != "")
            {
                ColorConsole.WriteColorLine($"[Email:] {_newEmail}", Globals.ExperienceColor);
            }
            if (_newGenres.Count > 0)
            {
                ColorConsole.WriteColorLine($"[Genres:] {string.Join(", ", _newGenres)}", Globals.ExperienceColor);
            }
            else
            {
                ColorConsole.WriteColorLine($"[Genres:] {notFilledIn}", Globals.ExperienceColor);
            }
            if (_newAgeCategory != AgeCategory.Undefined)
            {
                ColorConsole.WriteColorLine($"[leeftijdscatagorie:] {_newAgeCategory.GetDisplayName()}", Globals.ExperienceColor);
            }
            else
            {
                ColorConsole.WriteColorLine($"[leeftijdscatagorie:] {notFilledIn}", Globals.ExperienceColor);
            }
            if (_newIntensity != Intensity.Undefined)
            {
                ColorConsole.WriteColorLine($"[intensiteit:] {_newIntensity.GetDisplayName()}", Globals.ExperienceColor);
            }
            else
            {
                ColorConsole.WriteColorLine($"[intensiteit:] {notFilledIn}", Globals.ExperienceColor);
            }
            if (_language != Language.Undefined)
            {
                ColorConsole.WriteColorLine($"[Taal (Language):] {_language.GetDisplayName()}", Globals.ExperienceColor);
            }
            else
            {
                ColorConsole.WriteColorLine($"[Taal (Language):] {notFilledIn}", Globals.ExperienceColor);
            }
            if (AnyOfTheFieldsFilledIn)
            {
                HorizontalLine.Print();
            }
        }
        public static void ChangePassword()
        {
            bool validOldPassword = false;
            PrintTitle();
            while (!validOldPassword)
            {
                string oldPassword = ReadLineUtil.EnterValue("Voer uw [oude wachtwoord] in: ", UserDetails.Start, true);
                validOldPassword = _userLogic.ValidateOldPassword(oldPassword);

                if (!validOldPassword)
                {
                    PrintTitle();
                    ColorConsole.WriteColorLine("Oud wachtwoord is onjuist.", Globals.ErrorColor);
                }
            }
            string newPassword = "";
            bool validNewPassword = false;
            PrintTitle();
            while (!validNewPassword)
            {
                newPassword = ReadLineUtil.EnterValue("Voer uw [nieuwe wachtwoord] in: ", UserDetails.Start, true);
                validNewPassword = _userLogic.ValidatePassword(newPassword);

                if (!validNewPassword)
                {
                    if (newPassword.Length < 5)
                    {
                        PrintTitle();
                        ColorConsole.WriteColorLine("Wachtwoord moet minimaal 5 tekens bevatten.", Globals.ErrorColor);
                    }
                }
            }

            bool validConfirmPassword = false;
            PrintTitle();
            while (!validConfirmPassword)
            {
                string confirmPassword = ReadLineUtil.EnterValue("Bevestig uw [nieuwe wachtwoord] in: ", UserDetails.Start, true);
                validConfirmPassword = newPassword == confirmPassword;
                if (!validConfirmPassword)
                {
                    PrintTitle();
                    ColorConsole.WriteColorLine("Het wachtwoord komt niet overeen, probeer het opnieuw", Globals.ErrorColor);
                    Thread.Sleep(2000);
                }
            }

            if (_userLogic != null)
            {
                if (_userLogic.EditPassword(newPassword))
                {
                    Console.Clear();
                    ColorConsole.WriteColorLine("Wachtwoord is gewijzigd!", Globals.SuccessColor);
                    Thread.Sleep(4000);
                }
            }
            UserDetails.Start();
        }

        public static void PrintTitle()
        {
            Console.Clear();
            ColorConsole.WriteColorLine("Wachtwoord aanpassen\n", Globals.ExperienceColor);
        }
    }
}