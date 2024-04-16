namespace BioscoopReserveringsapplicatie
{
    static class UserMenu
    {
        public static void Start()
        {
            Console.Clear();

            if (UserLogic.CurrentUser != null)
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Experienceoverzicht", () => PreferredExperiences.Start()),
                    new Option<string>("Mijn account", () => UserDetails.Start()),
                    new Option<string>("Uitloggen", () => LandingPage.Start()),
                };
                SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine($"Welkom [{UserLogic.CurrentUser.FullName}]!\n", ConsoleColor.Green));
            }
            else
            {
                ColorConsole.WriteColorLine("Geen gebruiker gevonden.", Globals.ErrorColor);
            }
        }
    }
}