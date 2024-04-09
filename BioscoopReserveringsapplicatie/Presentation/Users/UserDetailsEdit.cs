using System.Drawing;

namespace BioscoopReserveringsapplicatie
{
    static class UserDetailsEdit
    {
        private static UserLogic _userLogic = new UserLogic();

        public static void Start()
        {
            if (UserLogic.CurrentUser != null)
            {
                string newName = "";
                bool validName = false;
                while (!validName)
                {
                    Console.Clear();
                    newName = ReadLineUtil.EditValue(UserLogic.CurrentUser.FullName,
                        () =>
                        {
                            Console.Write("Voer uw naam in: ");
                        },
                        () => UserDetails.Start(),
                        "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)"
                    );
                    validName = _userLogic.ValidateName(newName);
                }

                string newEmail = "";
                bool validEmail = false;
                while (!validEmail)
                {
                    Console.Clear();
                    newEmail = ReadLineUtil.EditValue(UserLogic.CurrentUser.EmailAddress,
                        () =>
                        {
                            Console.Write("Voer uw emailadres in: ");
                        },
                        () => UserDetails.Start(),
                        "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)"
                    );
                    validEmail = _userLogic.ValidateEmail(newEmail);
                }

                List<Genre> selectedGenres = Preferences.SelectGenres();
                // if (selectedGenres.Count == 0)
                // {
                //     selectedGenres = UserLogic.CurrentUser.Genres;
                // }

                AgeCategory ageCategory = Preferences.SelectAgeCategory();
                // if (ageCategory == AgeCategory.Undefined)
                // {
                //     ageCategory = UserLogic.CurrentUser.AgeCategory;
                // }

                Intensity intensity = Preferences.SelectIntensity();
                // if (intensity == Intensity.Undefined)
                // {
                //     intensity = UserLogic.CurrentUser.Intensity;
                // }

                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Ja", () => {
                        if(_userLogic.Edit(UserLogic.CurrentUser.Id, newName, newEmail, selectedGenres, intensity, ageCategory))
                        {
                            Console.Clear();
                            ColorConsole.WriteColorLine("[Gebruikers gegevens zijn gewijzigd!]", ConsoleColor.Green);
                            Thread.Sleep(2000);
                            UserDetails.Start();
                        }
                        else
                        {
                            List<Option<string>> options = new List<Option<string>>
                            {
                                new Option<string>("Terug", () => {Console.Clear(); UserDetails.Start();}),
                            };
                            SelectionMenuUtil.Create(options, () => Console.WriteLine("Er is een fout opgetreden tijdens het bewerken van uw persoonsgegevens. Probeer het opnieuw.\n"));
                        }
                    }),
                    new Option<string>("Nee", () => UserDetails.Start())
                };
                SelectionMenuUtil.Create(options, () => PendingChanges(newName, newEmail, selectedGenres, intensity, ageCategory));
            }
        }

        private static void PendingChanges(string newName, string newEmail, List<Genre> newGenres, Intensity newIntensity, AgeCategory newAgeCategory)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("[Nieuwe Profielgegevens]", ConsoleColor.Cyan);
            ColorConsole.WriteColorLine($"[Naam: ]{newName}", ConsoleColor.Cyan);
            ColorConsole.WriteColorLine($"[Email: ]{newEmail}\n", ConsoleColor.Cyan);
            ColorConsole.WriteColorLine("[Persoonlijke voorkeuren]", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Genre: ]{(newGenres.Any() ? string.Join(", ", newGenres) : "Undefined")}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Kijkwijzer: ]{newAgeCategory}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Intensiteit: ]{newIntensity}\n", ConsoleColor.Green);
            ColorConsole.WriteColorLine("[Weet je zeker dat je de gegevens wilt aanpassen ?]", ConsoleColor.Red);
        }
    }
}