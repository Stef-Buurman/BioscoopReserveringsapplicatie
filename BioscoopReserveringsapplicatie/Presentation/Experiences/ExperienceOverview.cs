namespace BioscoopReserveringsapplicatie
{
    static class ExperienceOverview
    {
        static private ExperiencesLogic ExperiencesLogic = new ExperiencesLogic();

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
                SelectionMenuUtil.Create(options, () => Console.WriteLine($"Alle experiences"));
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

            options.Add(new Option<int>(0, "Terug", () => { Console.Clear(); Start(); }));

            int experienceId = SelectionMenuUtil.Create(options, Print);
            Console.Clear();
            ShowExperienceDetails(experienceId);
            return experienceId;
        }

        private static int ShowAllArchivedExperiences()
        {
            List<ExperiencesModel> archivedExperiences = ExperiencesLogic.GetAllArchivedExperiences();
            return ShowExperiences(archivedExperiences);
        }

        private static int ShowAllActiveExperiences()
        {
            List<ExperiencesModel> activeExperiences = ExperiencesLogic.GetAllActiveExperiences();
            return ShowExperiences(activeExperiences);
        }

        private static int ShowAllExperiences()
        {
            List<ExperiencesModel> allExperiences = ExperiencesLogic.GetExperiences();
            return ShowExperiences(allExperiences);
        }

        private static void Print()
        {
            Console.WriteLine("Dit zijn alle experiences die momenteel beschikbaar zijn:");
        }
    }
}
