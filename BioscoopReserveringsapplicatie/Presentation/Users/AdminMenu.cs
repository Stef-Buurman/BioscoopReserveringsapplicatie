namespace BioscoopReserveringsapplicatie
{
    static class AdminMenu
    {
        public static void Start()
        {
            Console.Clear();

            if (UserLogic.IsAdmin())
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Planning", () => ScheduledExperiences.Start()),
                    new Option<string>("Experiences", () => ExperienceOverview.Start()),
                    new Option<string>("Films", () => MovieOverview.Start()),
                    new Option<string>("Promoties", () => PromotionOverview.Start()),
                    new Option<string>("Locaties", () => LocationOverview.Start()),
                    new Option<string>("Zalen", () => RoomOverview.Start()),
                    new Option<string>("Uitloggen", () => {UserLogic.Logout(); LandingPage.Start();})
                };
                ColorConsole.WriteColorLine($"Welkom [{UserLogic.CurrentUser.FullName}]!\n", ConsoleColor.Green);
                new SelectionMenuUtil<string>(options).Create();
            }
            else
            {
                ColorConsole.WriteColorLine("Uw account heeft geen toestemming om deze pagina te bekijken.", Globals.ErrorColor);
            }
        }
    }
}