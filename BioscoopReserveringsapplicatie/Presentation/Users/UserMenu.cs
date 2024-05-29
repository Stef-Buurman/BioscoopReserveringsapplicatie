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
                    new Option<string>("Experiences", () => PreferredExperiences.Start()),
                    new Option<string>("Mijn account", () => UserDetails.Start()),
                    new Option<string>("Mijn reserveringen",() => UserReservations.Start()),
                    new Option<string>("Uitloggen", () => {UserLogic.Logout(); LandingPage.Start();}),
                };
                ColorConsole.WriteColorLine($"Welkom [{UserLogic.CurrentUser.FullName}]!\n", ConsoleColor.Green);
                new SelectionMenuUtil<string>(options).Create();
            }
            else
            {
                ColorConsole.WriteColorLine("Geen gebruiker gevonden.", Globals.ErrorColor);
            }
        }
    }
}