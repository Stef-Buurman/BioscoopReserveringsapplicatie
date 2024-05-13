namespace BioscoopReserveringsapplicatie
{
    static class UserDetails
    {
        public static void Start()
        {
            Console.Clear();
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Mijn reserveringen",() => UserReservations.Start()),
                new Option<string>("Profielgegevens bewerken",() => UserDetailsEdit.Start()),
                new Option<string>("Wachtwoord wijzigen", () => UserDetailsEdit.ChangePassword()),
                new Option<string>("Terug", () => UserMenu.Start())
            };
            if (UserLogic.CurrentUser != null)
            {
                ColorConsole.WriteColorLine("Profielgegevens", ConsoleColor.Cyan);
                ColorConsole.WriteColorLine($"[Naam: ]{UserLogic.CurrentUser.FullName}", ConsoleColor.Cyan);
                ColorConsole.WriteColorLine($"[Email: ]{UserLogic.CurrentUser.EmailAddress}", ConsoleColor.Cyan);

                ColorConsole.WriteColorLine("Persoonlijke voorkeuren", ConsoleColor.Green);
                ColorConsole.WriteColorLine($"[Genre: ]{(UserLogic.CurrentUser.Genres.Any() ? string.Join(", ", UserLogic.CurrentUser.Genres) : "Undefined")}", ConsoleColor.Green);
                ColorConsole.WriteColorLine($"[Kijkwijzer: ]{UserLogic.CurrentUser.AgeCategory.GetDisplayName()}", ConsoleColor.Green);
                ColorConsole.WriteColorLine($"[Intensiteit: ]{UserLogic.CurrentUser.Intensity.GetDisplayName()}", ConsoleColor.Green);
                ColorConsole.WriteColorLine($"[Taal: ]{UserLogic.CurrentUser.Language}\n", ConsoleColor.Green);

            }
            new SelectionMenuUtil2<string>(options).Create();
        }
    }
}