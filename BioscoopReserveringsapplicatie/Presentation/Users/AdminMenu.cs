namespace BioscoopReserveringsapplicatie
{
    static class AdminMenu
    {
        public static void Start()
        {
            Console.Clear();

            if (UserLogic.CurrentUser != null && UserLogic.CurrentUser.IsAdmin)
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Film toevoegen", () => AddMovie.Start()),
                    new Option<string>("Filmoverzicht", () => MovieOverview.Start()),
                    new Option<string>("Experience toevoegen", () => AddExperience.Start()),
                    new Option<string>("Experienceoverzicht", () => ExperienceOverview.Start()),
                    new Option<string>("Promoties", () => Promotions.Start()),
                    new Option<string>("Uitloggen", () => LandingPage.Start())
                };
                Console.WriteLine($"Welkom {UserLogic.CurrentUser.FullName}!\n");
                new SelectionMenuUtil2<string>(options).Create();
            }
            else
            {
                ColorConsole.WriteColorLine("Uw account heeft geen toestemming om deze pagina te bekijken.", Globals.ErrorColor);
            }
        }
    }
}