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
                    Console.WriteLine("(druk op Enter om de huidige te behouden)");
                    Console.Write("Voer uw naam in: ");
                    newName = EditDefaultValueUtil.EditDefaultValue(UserLogic.CurrentUser.FullName);
                    if(newName != null && newName != "")
                    {
                        validName = true;
                    }
                }

                string newEmail = "";
                bool validEmail = false;
                while(!validEmail)
                {
                    Console.Clear();
                    Console.WriteLine("(druk op Enter om de huidige te behouden)");
                    Console.Write("Voer uw emailadres in: ");
                    newEmail = EditDefaultValueUtil.EditDefaultValue(UserLogic.CurrentUser.EmailAddress);
                    validEmail = _userLogic.ValidateEmail(newEmail);
                }

                List<Genre> newGenres = new List<Genre>();
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

                List<Intensity> intensities = Globals.GetAllEnum<Intensity>();
                Intensity newIntensity = SelectionMenu.Create(intensities, () => ColorConsole.WriteColorLine("Kies een [Intensiteit]: \n", Globals.ColorInputcClarification));

                List<AgeCategory> ageCategories = Globals.GetAllEnum<AgeCategory>();
                AgeCategory newAgeCategory = SelectionMenu.Create(ageCategories, () => ColorConsole.WriteColorLine("Kies een [Kijkwijzer]: \n", Globals.ColorInputcClarification));

                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Ja", () => {
                        if(_userLogic.EditUser(newName, newEmail, newGenres, newIntensity, newAgeCategory))
                        {
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
            ColorConsole.WriteColorLine($"[Kijkwijzer: ]{newAgeCategory}\n", ConsoleColor.Green);
        }
    }
}