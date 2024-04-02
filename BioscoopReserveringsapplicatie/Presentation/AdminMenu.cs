namespace BioscoopReserveringsapplicatie
{
    static class AdminMenu
    {
        private static UserModel? CurrentUser = UserLogic.CurrentUser;
        public static void Start()
        {
            if (CurrentUser != null && CurrentUser.IsAdmin)
            {
                Console.Clear();
                List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Film toevoegen", () => AddMovie.Start()),
                new Option<string>("Filmoverzicht", () => MovieOverview.Start()),
                new Option<string>("Experience toevoegen", () => AddExperience.Start()),
                new Option<string>("Experienceoverzicht", () => ExperienceOverview.Start()),
                new Option<string>("Uitloggen", () => LandingPage.Start()),
            };
                SelectionMenu.Create(options, () => Console.WriteLine($"Welkom {CurrentUser.FullName}!\n"));
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Uw account heeft geen toestemming om deze pagina te bekijken.");
            }
        }
    }
}