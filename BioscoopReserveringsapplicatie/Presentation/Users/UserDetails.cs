namespace BioscoopReserveringsapplicatie
{
    static class UserDetails
    {
        public static void Start()
        {
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Profielgegevens bewerken",() => UserDetailsEdit.Start()),
                new Option<string>("Terug", () => UserMenu.Start())
            };
            SelectionMenuUtil.Create(options, () => UserInfo());
        }

        private static void UserInfo()
        {
            if (UserLogic.CurrentUser != null)
            {
                Console.Clear();
                ColorConsole.WriteColorLine("Profielgegevens", ConsoleColor.Cyan);
                ColorConsole.WriteColorLine($"[Naam: ]{UserLogic.CurrentUser.FullName}", ConsoleColor.Cyan);
                ColorConsole.WriteColorLine($"[Email: ]{UserLogic.CurrentUser.EmailAddress}\n", ConsoleColor.Cyan);
                ColorConsole.WriteColorLine("Persoonlijke voorkeuren", ConsoleColor.Green);
                ColorConsole.WriteColorLine($"[Genre: ]{(UserLogic.CurrentUser.Genres.Any() ? string.Join(", ", UserLogic.CurrentUser.Genres) : "Undefined")}", ConsoleColor.Green);
                ColorConsole.WriteColorLine($"[Kijkwijzer: ]{UserLogic.CurrentUser.AgeCategory.GetDisplayName()}", ConsoleColor.Green);
                ColorConsole.WriteColorLine($"[Intensiteit: ]{UserLogic.CurrentUser.Intensity}\n", ConsoleColor.Green);
            }
        }
    }
}