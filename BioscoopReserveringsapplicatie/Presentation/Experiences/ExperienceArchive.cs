using System.Xml.Linq;

namespace BioscoopReserveringsapplicatie
{
    static class ExperienceArchive
    {
        private static ExperiencesLogic ExperienceLogic = new ExperiencesLogic();

        public static void Start(int experienceId)
        {
            Console.Clear();
            ExperiencesModel experience = ExperienceLogic.GetById(experienceId);
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    ExperienceLogic.ArchiveExperience(experienceId);
                    Console.Clear();
                    ColorConsole.WriteColorLine($"De Experience: {experience.Name} is gearchiveerd!", Globals.SuccessColor);
                    Thread.Sleep(4000);
                    ExperienceDetails.Start(experienceId);
                }),
                new Option<string>("Nee", () => {
                    ExperienceDetails.Start(experienceId);
                }),
            };
            ColorConsole.WriteColorLine("De experience details zijn:", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience naam:] {experience.Name}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience intensiteit:] {experience.Intensity}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience lengte (in minuten):] {experience.TimeLength}\n", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"Weet u zeker dat u de experience {experience.Name} wilt [archiveren]?", Globals.ColorInputcClarification);
            new SelectionMenuUtil2<string>(options).Create();
        }
    }
}