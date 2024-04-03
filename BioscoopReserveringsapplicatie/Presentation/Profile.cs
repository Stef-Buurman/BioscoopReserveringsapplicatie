namespace BioscoopReserveringsapplicatie
{
    static class Profile
    {
        public static void Start()
        {
            if(UserLogic.CurrentUser != null){
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Profielgegevens",() => UserDetails.Start()),
                    new Option<string>("Terug", () => UserMenu.Start())
                };
                SelectionMenu.Create(options, () => ColorConsole.WriteColorLine($"[{UserLogic.CurrentUser.FullName}]\n", ConsoleColor.Cyan));
            }
        }
    }
}