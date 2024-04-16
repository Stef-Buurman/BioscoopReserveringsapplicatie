namespace BioscoopReserveringsapplicatie
{
    static class PreferredExperiences
    {
        static private ExperiencesLogic ExperienceLogic = new ExperiencesLogic();

        public static void Start()
        {
            int experienceId = ShowExperiencesWithUserPreferences();

            if (experienceId != 0) ExperienceDetails.Start(experienceId);
        }

        private static int ShowExperiencesWithUserPreferences()
        {

            if (UserLogic.CurrentUser == null)
            {
                Console.WriteLine("Geen gebruiker gevonden");
                return 0;
            }

            List<Option<int>> options = new List<Option<int>>();
            List<ExperiencesModel> experiences = ExperienceLogic.GetExperiencesByUserPreferences(UserLogic.CurrentUser);

            foreach (ExperiencesModel experience in experiences)
            {
                options.Add(new Option<int>(experience.Id, experience.Name));
            }
            Console.WriteLine("Dit zijn uw aanbevolen experiences op basis van uw voorkeuren:");
            int experienceId = new SelectionMenuUtil2<int>(options, 21).Create();
            Console.Clear();
            return experienceId;
        }
    }
}
