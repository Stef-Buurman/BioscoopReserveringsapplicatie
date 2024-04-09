namespace BioscoopReserveringsapplicatie
{
    static class UserDetails
    {
        private static UserModel? CurrentUser = UserLogic.CurrentUser;

        public static void Start()
        {
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Profielgegevens bewerken",() => Console.WriteLine("Not implemented")),
                new Option<string>("Terug", () => Profile.Start())
            };
            SelectionMenu.Create(options, () => UserInfo());
        }

        private static void UserInfo()
        {
            if(CurrentUser != null)
            {
                Console.Clear();
                ColorConsole.WriteColorLine("[Profielgegevens]", ConsoleColor.Cyan);
                ColorConsole.WriteColorLine($"[Naam: ]{CurrentUser.FullName}", ConsoleColor.Cyan);
                ColorConsole.WriteColorLine($"[Email: ]{CurrentUser.EmailAddress}\n", ConsoleColor.Cyan);
                ColorConsole.WriteColorLine("[Persoonlijke voorkeuren]", ConsoleColor.Green);
                ColorConsole.WriteColorLine($"[Genre: ]{string.Join(", ", CurrentUser.Genres)}", ConsoleColor.Green);
                ColorConsole.WriteColorLine($"[Intensiteit: ]{CurrentUser.Intensity}", ConsoleColor.Green);
                ColorConsole.WriteColorLine($"[Kijkwijzer: ]{CurrentUser.AgeCategory.GetDisplayName()}\n", ConsoleColor.Green);
            }
        }
    }
}