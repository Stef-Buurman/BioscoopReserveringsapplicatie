namespace BioscoopReserveringsapplicatie
{
    static class ExperienceArchive
    {
        static private ExperiencesLogic ExperienceLogic = new ExperiencesLogic();

        public static void Start(int experienceId)
        {
            ExperiencesModel experience = ExperienceLogic.GetById(experienceId);
            
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    ExperienceLogic.ArchiveExperience(experienceId);
                    // Hier moet nog admin experience overzicht komen.
                    // Maar voor nu terug naar preferred.
                    PreferredExperiences.Start();
                }),
                new Option<string>("Nee", () => {
                    ExperienceDetails.Start(experienceId);
                }),
            };
            SelectionMenu.Create(options, () => Print(experience.Name, experience.Intensity, experience.TimeLength));
        }

        public static void Print(string name, string intensity, int timeLength)
        {
            Console.WriteLine("De experience details zijn:");
            Console.WriteLine($"Experience naam: {name}");
            Console.WriteLine($"Experience intensiteit: {intensity}");
            Console.WriteLine($"Experience lengte (in minuten): {timeLength}\n");
            Console.WriteLine($"Weet u zeker dat u de experience {name} wilt verwijderen?");
        }
    }
}
