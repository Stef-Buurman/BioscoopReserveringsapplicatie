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
                    new Option<string>("Experience inplannen", () => ScheduleExperince.Start()),
                    new Option<string>("Film toevoegen", () => AddMovie.Start()),
                    new Option<string>("Filmoverzicht", () => MovieOverview.Start()),
                    new Option<string>("Experience toevoegen", () => AddExperience.Start()),
                    new Option<string>("Experienceoverzicht", () => ExperienceOverview.Start()),
                    new Option<string>("Uitloggen", () => { UserLogic.Logout(); LandingPage.Start(); })
                };
                SelectionMenuUtil.Create(options, () => Console.WriteLine($"Welkom {UserLogic.CurrentUser.FullName}!\n"));
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Uw account heeft geen toestemming om deze pagina te bekijken.");
            }
        }
    }
}