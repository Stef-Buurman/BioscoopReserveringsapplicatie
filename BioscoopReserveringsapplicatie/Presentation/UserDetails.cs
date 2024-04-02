namespace BioscoopReserveringsapplicatie
{
    static class UserDetails
    {
        private static UserModel? CurrentUser = UserLogic.CurrentUser;

        public static void Start()
        {
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Profielgegevens",() => Console.WriteLine("Not implemented")),
                new Option<string>("Terug", () => Profile.Start())
            };
            SelectionMenu.Create(options, () => UserInfo());
        }

        private static void UserInfo()
        {
            if(CurrentUser != null)
            {
                Console.Clear();
                ColorConsole.WriteColorLine("[Profielgegevens]\n", ConsoleColor.Cyan);
                
            }
        }
    }
}