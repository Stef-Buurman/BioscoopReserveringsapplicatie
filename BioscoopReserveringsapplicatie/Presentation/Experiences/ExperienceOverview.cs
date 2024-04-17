namespace BioscoopReserveringsapplicatie
{
    static class ExperienceOverview
    {
        private static ExperiencesLogic ExperiencesLogic = new ExperiencesLogic();

        public static void Start()
        {
            Console.Clear();
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Alle active experiences", () => ShowAllActiveExperiences()),
                new Option<string>("Alle gearchiveerde experiences", () => ShowAllArchivedExperiences()),
                new Option<string>("Alle experiences", () => ShowAllExperiences()),
                new Option<string>("Terug", () => AdminMenu.Start()),
            };
            ColorConsole.WriteColorLine("Kies een van de volgende experience overzichten: \n", Globals.TitleColor);
            new SelectionMenuUtil2<string>(options).Create();
        }

        private static void ShowExperienceDetails(int experienceId)
        {
            if (experienceId != 0)
            {
                ExperienceDetails.Start(experienceId);
            }
        }

        private static int ShowExperiences(List<ExperiencesModel> experiences)
        {
            Console.Clear();
            List<Option<int>> options = new List<Option<int>>();

            foreach (ExperiencesModel experience in experiences)
            {
                options.Add(new Option<int>(experience.Id, experience.Name));
            }
            ColorConsole.WriteColorLine("Dit zijn alle experiences die momenteel beschikbaar zijn:", Globals.TitleColor);
            int experienceId = new SelectionMenuUtil2<int>(options,
                () =>
                {
                    Start();
                }, 
                () => 
                {
                    ShowExperiences(experiences);
                }).Create();
            Console.Clear();
            ShowExperienceDetails(experienceId);
            return experienceId;
        }

        private static void ShowAllArchivedExperiences()
        {
            List<ExperiencesModel> archivedExperiences = ExperiencesLogic.GetAllArchivedExperiences();

            if (archivedExperiences.Count == 0) PrintWhenNoExperiencesFound("Er zijn geen gearchiveerde experiences gevonden.");
            else ShowExperiences(archivedExperiences);
        }

        private static void ShowAllActiveExperiences()
        {
            List<ExperiencesModel> activeExperiences = ExperiencesLogic.GetAllActiveExperiences();

            if (activeExperiences.Count == 0) PrintWhenNoExperiencesFound("Er zijn geen actieve experiences gevonden.");
            else ShowExperiences(activeExperiences);
        }

        private static void ShowAllExperiences()
        {
            List<ExperiencesModel> allExperiences = ExperiencesLogic.GetExperiences();

            if (allExperiences.Count == 0) PrintWhenNoExperiencesFound("Er zijn geen experiences gevonden.");
            else ShowExperiences(allExperiences);
        }

        private static void PrintWhenNoExperiencesFound(string whichExperiences)
        {
            Console.Clear();
            Console.WriteLine(whichExperiences);
            Thread.Sleep(500);
            Console.WriteLine("Terug naar experience overzicht...");
            Thread.Sleep(1500);
            Start();
        }
    }
}
