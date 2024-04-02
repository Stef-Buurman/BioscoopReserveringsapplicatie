namespace BioscoopReserveringsapplicatie
{
    static class Profile
    {
        private static UserModel? CurrentUser = UserLogic.CurrentUser;
        public static void Start()
        {
            if(CurrentUser != null){
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Profielgegevens",() => UserDetails.Start()),
                    new Option<string>("Terug", () => UserMenu.Start())
                };
                SelectionMenu.Create(options, () => ColorConsole.WriteColorLine($"[{CurrentUser.FullName}]\n", ConsoleColor.Cyan));
            }
        }
    }
}