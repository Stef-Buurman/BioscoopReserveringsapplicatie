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
                    ExperienceDetails.Start(experienceId);
                }),
                new Option<string>("Nee", () => {
                    ExperienceDetails.Start(experienceId);
                }),
            };
            SelectionMenuUtil.Create(options, () => Print(experience.Name, experience.Intensity.ToString(), experience.TimeLength));
        }

        public static void Print(string name, string intensity, int timeLength)
        {
            ColorConsole.WriteColorLine("De experience details zijn:", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience naam:] {name}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience intensiteit:] {intensity}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience lengte (in minuten):] {timeLength}\n", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"Weet u zeker dat u de experience {name} wilt [archiveren]?", Globals.ColorInputcClarification);
        }
    }
}