namespace BioscoopReserveringsapplicatie
{
    static class AdminMenu
    {
        public static void Start()
        {
            if (UserLogic.CurrentUser != null && UserLogic.CurrentUser.IsAdmin)
            {
                Console.Clear();
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Film toevoegen", () => AddMovie.Start()),
                    new Option<string>("Filmoverzicht", () => MovieOverview.Start()),
                    new Option<string>("Experience toevoegen", () => AddExperience.Start()),
                    new Option<string>("Experienceoverzicht", () => ExperienceOverview.Start()),
                    new Option<string>("Promoties", () => Promotions.Start()),
                    new Option<string>("Uitloggen", () => { UserLogic.Logout(); LandingPage.Start(); })
                };
                SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine($"Welkom [{UserLogic.CurrentUser.FullName}]!\n", ConsoleColor.Green));
            }
            else
            {
                Console.Clear();
                ColorConsole.WriteColorLine("Uw account heeft geen toestemming om deze pagina te bekijken.", Globals.ErrorColor);
            }
        }
    }
}