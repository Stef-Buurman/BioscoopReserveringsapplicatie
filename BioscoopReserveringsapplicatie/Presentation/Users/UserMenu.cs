namespace BioscoopReserveringsapplicatie
{
    static class UserMenu
    {
        public static void Start()
        {
            if (UserLogic.CurrentUser != null)
            {
                Console.Clear();
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Experienceoverzicht", () => PreferredExperiences.Start()),
                    new Option<string>("Mijn account", () => UserDetails.Start()),
                    new Option<string>("Uitloggen", () => { UserLogic.Logout(); LandingPage.Start(); }),
                };
                ColorConsole.WriteColorLine($"Welkom [{UserLogic.CurrentUser.FullName}]!\n", ConsoleColor.Green);
                new SelectionMenuUtil2<string>(options).Create();
            }
            else
            {
                Console.Clear();
                ColorConsole.WriteColorLine("Geen gebruiker gevonden.", Globals.ErrorColor);
            }
        }
    }
}