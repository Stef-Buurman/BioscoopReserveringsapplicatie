using System.Drawing;

namespace BioscoopReserveringsapplicatie
{
    static class UserDetailsEdit
    {
        private static UserLogic _userLogic = new UserLogic();

        public static void Start()
        {
            if(UserLogic.CurrentUser != null)
            {   
                string newName = "";
                bool validName = false;
                while(!validName)
                {
                    Console.Clear();
                    newName = ReadLineUtil.EditValue(UserLogic.CurrentUser.FullName, 
                        () => {
                            Console.Write("Voer uw naam in: ");
                        },
                        () => UserDetails.Start(),
                        "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)"
                    );
                    validName = _userLogic.ValidateName(newName);
                }

                string newEmail = "";
                bool validEmail = false;
                while(!validEmail)
                {
                    Console.Clear();
                    newEmail = ReadLineUtil.EditValue(UserLogic.CurrentUser.EmailAddress, 
                        () => {
                            Console.Write("Voer uw emailadres in: ");
                        },
                        () => UserDetails.Start(),
                        "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)"
                    );
                    validEmail = _userLogic.ValidateEmail(newEmail);
                }


                List<Genre> newGenres = new List<Genre>();
                bool validGenres = false;
                while(!validGenres)
                {
                    List<Genre> availableGenres = Globals.GetAllEnum<Genre>();
                    while (newGenres.Count < 3)
                    {
                        Genre newGenre = SelectionMenu.Create(availableGenres, () => ColorConsole.WriteColorLine("Kies een [genre]: \n", Globals.ColorInputcClarification));

                        if(availableGenres.Contains(newGenre))
                        {
                            availableGenres.Remove(newGenre);
                            newGenres.Add(newGenre);
                        }
                    }
                    validGenres = _userLogic.ValidateGenres(newGenres);
                }

                Intensity newIntensity = Intensity.Undefined;
                bool validIntensity = false;
                while(!validIntensity)
                {
                    List<Intensity> intensities = Globals.GetAllEnum<Intensity>();
                    newIntensity = SelectionMenu.Create(intensities, () => ColorConsole.WriteColorLine("Kies een [Intensiteit]: \n", Globals.ColorInputcClarification));
                    validIntensity = _userLogic.ValidateIntensity(newIntensity);
                }

                AgeCategory newAgeCategory = AgeCategory.Undefined;
                bool validAgeCategory = false;
                while(!validAgeCategory)
                {
                    List<AgeCategory> ageCategories = Globals.GetAllEnum<AgeCategory>();
                    newAgeCategory = SelectionMenu.Create(ageCategories, () => ColorConsole.WriteColorLine("Kies een [Kijkwijzer]: \n", Globals.ColorInputcClarification));
                    validAgeCategory = _userLogic.ValidateAgeCategory(newAgeCategory);
                }


                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Ja", () => {
                        if(_userLogic.Edit(UserLogic.CurrentUser.Id, newName, newEmail, newGenres, newIntensity, newAgeCategory))
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
                            SelectionMenu.Create(options, () => Console.WriteLine("Er is een fout opgetreden tijdens het bewerken van uw persoonsgegevens. Probeer het opnieuw.\n"));
                        }
                    }),
                    new Option<string>("Nee", () => UserDetails.Start())
                };
                SelectionMenu.Create(options, () => PendingChanges(newName, newEmail, newGenres, newIntensity, newAgeCategory));
            }
        }

        private static void PendingChanges(string newName, string newEmail, List<Genre> newGenres, Intensity newIntensity, AgeCategory newAgeCategory)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("[Nieuwe Profielgegevens]", ConsoleColor.Cyan);
            ColorConsole.WriteColorLine($"[Naam: ]{newName}", ConsoleColor.Cyan);
            ColorConsole.WriteColorLine($"[Email: ]{newEmail}\n", ConsoleColor.Cyan);
            ColorConsole.WriteColorLine("[Persoonlijke voorkeuren]", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Genre: ]{string.Join(", ", newGenres)}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Intensiteit: ]{newIntensity}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Kijkwijzer: ]{newAgeCategory}\n\n", ConsoleColor.Green);
            ColorConsole.WriteColorLine("[Weet je zeker dat je de gegevens wilt aanpassen ?]", ConsoleColor.Red);
        }
    }
}