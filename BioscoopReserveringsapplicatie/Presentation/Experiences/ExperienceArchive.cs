using System.Xml.Linq;

namespace BioscoopReserveringsapplicatie
{
    static class ExperienceArchive
    {
        private static ExperienceLogic ExperienceLogic = new ExperienceLogic();

        public static void Start(int experienceId)
        {
            Console.Clear();
            ExperienceModel experience = ExperienceLogic.GetById(experienceId);
            if (experience == null)
            {
                ColorConsole.WriteColorLine("Er is geen experience gevonden.", Globals.ErrorColor);
                Thread.Sleep(2000);
                ExperienceOverview.Start();
                return;
            }

            if (experience.Status == Status.Active)
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Ja", () => {
                        ExperienceLogic.Archive(experienceId);
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
                ColorConsole.WriteColorLine($"[Experience beschrijving:] {experience.Description}", Globals.ExperienceColor);
                ColorConsole.WriteColorLine($"[Experience intensiteit:] {experience.Intensity}", Globals.ExperienceColor);
                ColorConsole.WriteColorLine($"[Experience lengte (in minuten):] {experience.TimeLength}\n", Globals.ExperienceColor);
                ColorConsole.WriteColorLine($"Weet u zeker dat u de experience {experience.Name} wilt [archiveren]?", Globals.ColorInputcClarification);
                new SelectionMenuUtil<string>(options, new Option<string>("Nee")).Create();
            }
            else
            {
                List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    ExperienceLogic.Unarchive(experienceId);
                    Console.Clear();
                    ColorConsole.WriteColorLine($"De Experience: {experience.Name} is gedearchiveerd!", Globals.SuccessColor);
                    Thread.Sleep(4000);
                    ExperienceDetails.Start(experienceId);
                }),
                new Option<string>("Nee", () => {
                    ExperienceDetails.Start(experienceId);
                }),
            };
                ColorConsole.WriteColorLine("De experience details zijn:", Globals.ExperienceColor);
                ColorConsole.WriteColorLine($"[Experience naam:] {experience.Name}", Globals.ExperienceColor);
                ColorConsole.WriteColorLine($"[Experience beschrijving:] {experience.Description}", Globals.ExperienceColor);
                ColorConsole.WriteColorLine($"[Experience intensiteit:] {experience.Intensity}", Globals.ExperienceColor);
                ColorConsole.WriteColorLine($"[Experience lengte (in minuten):] {experience.TimeLength}\n", Globals.ExperienceColor);
                ColorConsole.WriteColorLine($"Weet u zeker dat u de experience {experience.Name} wilt [dearchiveren]?", Globals.ColorInputcClarification);
                new SelectionMenuUtil<string>(options, new Option<string>("Nee")).Create();
            }
        }
    }
}