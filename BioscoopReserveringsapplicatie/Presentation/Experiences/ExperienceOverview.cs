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
                SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Kies een van de volgende experience overzichten: \n", Globals.TitleColor));
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
            List<Option<int>> options = new List<Option<int>>();

            foreach (ExperiencesModel experience in experiences)
            {
                options.Add(new Option<int>(experience.Id, experience.Name));
            }

            int experienceId = SelectionMenuUtil.Create(options, 21, Print, () => { Console.Clear(); Start(); });
            Console.Clear();
            ShowExperienceDetails(experienceId);
            return experienceId;
        }

        private static void ShowAllArchivedExperiences()
        {
            List<ExperiencesModel> archivedExperiences = ExperiencesLogic.GetAllArchivedExperiences();

            if(archivedExperiences.Count == 0) PrintWhenNoExperiencesFound("Er zijn geen gearchiveerde experiences gevonden.");
            else ShowExperiences(archivedExperiences);
        }

        private static void ShowAllActiveExperiences()
        {
            List<ExperiencesModel> activeExperiences = ExperiencesLogic.GetAllActiveExperiences();

            if (activeExperiences.Count == 0) PrintWhenNoExperiencesFound("Er zijn geen actieve experiences gevonden.");
            else  ShowExperiences(activeExperiences);
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

        private static void Print()
        {
            Console.WriteLine("Dit zijn alle experiences die momenteel beschikbaar zijn:");
        }
    }
}
