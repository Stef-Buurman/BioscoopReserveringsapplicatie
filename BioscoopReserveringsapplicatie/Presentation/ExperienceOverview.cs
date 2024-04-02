namespace BioscoopReserveringsapplicatie
{
    static class ExperienceOverview
    {
        static private ExperiencesLogic ExperiencesLogic = new ExperiencesLogic();

        public static void Start()
        {
            int experienceId = ShowAllExperiences();

            if (experienceId != 0)
            {
                ExperienceDetails.Start(experienceId);
            }
        }

        private static int ShowAllExperiences()
        {
            List<Option<int>> options = new List<Option<int>>();
            List<ExperiencesModel> experiences = ExperiencesLogic.GetExperiences();

            foreach (ExperiencesModel experience in experiences)
            {
                options.Add(new Option<int>(experience.Id, experience.Name));
            }

            options.Add(new Option<int>(0, "Terug", () => { Console.Clear(); AdminMenu.Start(); }));

            int experienceId = SelectionMenu.Create(options, Print);
            Console.Clear();
            return experienceId;
        }

        private static void Print()
        {
            Console.WriteLine("Dit zijn alle experiences die momenteel beschikbaar zijn:");
        }
    }
}

