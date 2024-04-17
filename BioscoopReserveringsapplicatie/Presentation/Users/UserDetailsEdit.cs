using System.Drawing;

namespace BioscoopReserveringsapplicatie
{
    static class UserDetailsEdit
    {
        private static UserLogic _userLogic = new UserLogic();
        private static Action actionWhenEscapePressed = UserDetails.Start;

        private static string newName = "";
        private static string newEmail = "";
        private static List<Genre> newGenres = new List<Genre>();
        private static AgeCategory newAgeCategory = AgeCategory.Undefined;
        private static Intensity newintensity = Intensity.Undefined;

        private static string _returnToName = "Name";
        private static string _returnToEmail = "Email";
        private static string _returnToGenres = "Genres";
        private static string _returnToRating = "Rating";
        private static string _returnToIntensity = "Intensity";

        public static void Start(string returnTo = "")
        {
            if (UserLogic.CurrentUser != null)
            {
                if (returnTo == "" || returnTo == _returnToName) UserName();
                if (returnTo == "" || returnTo == _returnToEmail) UserEmail();
                if (returnTo == "" || returnTo == _returnToGenres) UserGenres();
                if (returnTo == "" || returnTo == _returnToRating)  UserRating();
                if (returnTo == "" || returnTo == _returnToIntensity) UserIntensity();

                

                newGenres = Preferences.SelectGenres();

                newAgeCategory = Preferences.SelectAgeCategory();

                newintensity = Preferences.SelectIntensity();

                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Ja", () => {
                        if(_userLogic.Edit(UserLogic.CurrentUser.Id, newName, newEmail, newGenres, newintensity, newAgeCategory))
                        {
                            Console.Clear();
                            ColorConsole.WriteColorLine("Gebruikersgegevens zijn gewijzigd!", Globals.SuccessColor);
                            Thread.Sleep(2000);
                            UserDetails.Start();
                        }
                        else
                        {
                            List<Option<string>> options = new List<Option<string>>
                            {
                                new Option<string>("Terug", () => {Console.Clear(); UserDetails.Start();}),
                            };
                            SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Er is een fout opgetreden tijdens het bewerken van uw gebruikersgegevens. Probeer het opnieuw.\n", Globals.ErrorColor));
                        }
                    }),
                    new Option<string>("Nee", actionWhenEscapePressed)
                };
                SelectionMenuUtil.Create(options, () => PendingChanges(newName, newEmail, newGenres, newintensity, newAgeCategory));
            }
        }

        private static void UserName()
        {
            newName = "";
            bool validName = false;
            while (!validName)
            {
                Console.Clear();
                newName = ReadLineUtil.EditValue(UserLogic.CurrentUser.FullName,
                    () =>
                    {
                        ColorConsole.WriteColor("Voer uw [naam] in: ", Globals.ColorInputcClarification);
                    },
                    actionWhenEscapePressed,
                    "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n"
                );
                validName = _userLogic.ValidateName(newName);
            }
        }

        private static void UserEmail()
        {
            newEmail = "";
            bool validEmail = false;
            while (!validEmail)
            {
                Console.Clear();
                newEmail = ReadLineUtil.EditValue(UserLogic.CurrentUser.EmailAddress,
                    () =>
                    {
                        ColorConsole.WriteColorLine($"Voer uw [naam] in: {newName}", Globals.ColorInputcClarification);
                        ColorConsole.WriteColor("Voer uw [emailadres] in: ", Globals.ColorInputcClarification);
                    },
                    () => Start(_returnToName),
                    "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n"
                );
                validEmail = _userLogic.ValidateEmail(newEmail);
            }
        }

        private static void UserGenres()
        {
            throw new NotImplementedException();
        }

        private static void UserRating()
        {
            throw new NotImplementedException();
        }

        private static void UserIntensity()
        {
            throw new NotImplementedException();
        }

        private static void PendingChanges(string newName, string newEmail, List<Genre> newGenres, Intensity newIntensity, AgeCategory newAgeCategory)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("Nieuwe Profielgegevens", ConsoleColor.Cyan);
            ColorConsole.WriteColorLine($"[Naam: ]{newName}", ConsoleColor.Cyan);
            ColorConsole.WriteColorLine($"[Email: ]{newEmail}\n", ConsoleColor.Cyan);
            ColorConsole.WriteColorLine("Persoonlijke voorkeuren", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Genre: ]{(newGenres.Any() ? string.Join(", ", newGenres) : "Undefined")}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Kijkwijzer: ]{newAgeCategory.GetDisplayName()}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Intensiteit: ]{newIntensity}\n", ConsoleColor.Green);
            ColorConsole.WriteColorLine("Weet je zeker dat je de gegevens wilt aanpassen?", ConsoleColor.Red);
        }
    }
}