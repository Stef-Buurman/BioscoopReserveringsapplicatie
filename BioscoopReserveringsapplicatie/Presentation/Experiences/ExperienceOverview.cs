namespace BioscoopReserveringsapplicatie
{
    static class ExperienceOverview
    {
        private static ExperienceLogic ExperiencesLogic = new ExperienceLogic();
        private static MovieLogic MoviesLogic = new MovieLogic();
        private static Func<ExperienceModel, string[]> experienceDataExtractor = ExtractExperienceData;

        public static void Start()
        {
            Console.Clear();
            ShowAllExperiences();
        }

        private static void ShowExperienceDetails(int experienceId)
        {
            if (experienceId != 0)
            {
                ExperienceDetails.Start(experienceId);
            }
        }

        private static int ShowExperiences(List<ExperienceModel> experiences)
        {
            Console.Clear();
            List<Option<int>> options = new List<Option<int>>();

            List<string> columnHeaders = new List<string>
            {
                "Experience Naam  ",
                "Genres",
                "Leeftijdscategorie",
                "Intensiteit",
                "Status"
            };

            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, experiences, experienceDataExtractor);

            foreach (ExperienceModel experience in experiences)
            {
                MovieModel movie = MoviesLogic.GetById(experience.FilmId);

                string experienceName = experience.Name;
                if (experienceName.Length > 25)
                {
                    experienceName = experienceName.Substring(0, 25) + "...";
                }

                string genres = string.Join(",", movie.Genres);
                if (genres.Length > 25)
                {
                    genres = genres.Substring(0, 23) + "...";
                }
                string experienceInfo = string.Format("{0,-" + (columnWidths[0] + 2) + "} {1,-" + (columnWidths[1] + 2) + "} {2,-" + (columnWidths[2] + 2) + "} {3,-" + (columnWidths[3] + 2) + "} {4,-" + columnWidths[4] + "}",
                experienceName, genres, movie.AgeCategory.GetDisplayName(), experience.Intensity.GetDisplayName(), experience.Status.GetDisplayName());
                options.Add(new Option<int>(experience.Id, experienceInfo));
            }
            ColorConsole.WriteLineInfoHighlight("*Klik op [Enter] om de details van een experience te bekijken*", Globals.ColorInputcClarification);
            ColorConsole.WriteLineInfoHighlight("*Klik op [Escape] om terug te gaan*", Globals.ColorInputcClarification);
            ColorConsole.WriteLineInfoHighlight("*Klik op [T] om een experience toe te voegen*", Globals.ColorInputcClarification);
            ColorConsole.WriteLineInfoHighlight("*Klik op [1] om alle experiences te tonen*", Globals.ColorInputcClarification);
            ColorConsole.WriteLineInfoHighlight("*Klik op [2] om alle active experiences te tonen*", Globals.ColorInputcClarification);
            ColorConsole.WriteLineInfoHighlight("*Klik op [3] om alle gearchiveerde experiences te tonen*\n", Globals.ColorInputcClarification);
            ColorConsole.WriteColorLine("Dit zijn alle experiences die momenteel bestaan:\n", Globals.TitleColor);
            Print(columnHeaders, columnWidths);
            int experienceId = new SelectionMenuUtil<int>(options,
                () =>
                {
                    AdminMenu.Start();
                },
                () =>
                {
                    ShowExperiences(experiences);
                },
                new List<KeyAction>()
                {
                    new KeyAction(ConsoleKey.T, () => AddExperience.Start()),
                    new KeyAction(ConsoleKey.D1, () => ShowAllExperiences()),
                    new KeyAction(ConsoleKey.D2, () => ShowAllActiveExperiences()),
                    new KeyAction(ConsoleKey.D3, () => ShowAllArchivedExperiences()),
                },showEscapeabilityText: false).Create();
            Console.Clear();
            ShowExperienceDetails(experienceId);
            return experienceId;
        }

        private static void ShowAllArchivedExperiences()
        {
            List<ExperienceModel> archivedExperiences = ExperiencesLogic.GetAllArchivedExperiences();

            if (archivedExperiences.Count == 0) PrintWhenNoExperiencesFound("Er zijn geen gearchiveerde experiences gevonden.", "archived");
            ShowExperiences(archivedExperiences);
        }

        private static void ShowAllActiveExperiences()
        {
            List<ExperienceModel> activeExperiences = ExperiencesLogic.GetAllActiveExperiences();

            if (activeExperiences.Count == 0) PrintWhenNoExperiencesFound("Er zijn geen actieve experiences gevonden.", "active");
            else ShowExperiences(activeExperiences);
        }

        private static void ShowAllExperiences()
        {
            List<ExperienceModel> allExperiences = ExperiencesLogic.GetAll();

            if (allExperiences.Count == 0) PrintWhenNoExperiencesFound("Er zijn geen experiences gevonden.", "all");
            else ShowExperiences(allExperiences);
        }

        private static void PrintWhenNoExperiencesFound(string notFoundMessage, String filterType)
        {
            if(filterType == "all")
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Ja", () => {
                       AddExperience.Start();
                    }),
                    new Option<string>("Nee", () => {
                        AdminMenu.Start();
                    }),
                };
                Console.WriteLine(notFoundMessage);
                Console.WriteLine();
                Console.WriteLine("Wil je een experience aanmaken?");
                new SelectionMenuUtil<string>(options, new Option<string>("Nee")).Create();
            }
            else
            {
                Console.Clear();
                Console.WriteLine(notFoundMessage);
                WaitUtil.WaitTime(500);
                Console.WriteLine("Terug naar alle experience overzicht...");
                WaitUtil.WaitTime(1500);
                Start();
            }
        }

        private static void Print(List<string> columnHeaders, int[] columnWidths)
        {
            // Print de kop van de tabel
            Console.Write("".PadRight(3));
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write(columnHeaders[i].PadRight(columnWidths[i] + 3));
            }
            Console.WriteLine();
            
            // Print de "----"-lijn tussen de kop en de data
            Console.Write("".PadRight(3));
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write("".PadRight(columnWidths[i], '-').PadRight(columnWidths[i] + 3));
            }
            Console.WriteLine();
        }

        private static string[] ExtractExperienceData(ExperienceModel experience)
        {
            MovieModel movie = MoviesLogic.GetById(experience.FilmId);

            string[] experienceInfo = {
                experience.Name,
                string.Join(",", movie.Genres),
                movie.AgeCategory.GetDisplayName(),
                experience.Intensity.GetDisplayName(),
                experience.Status.GetDisplayName()
            };
            return experienceInfo;
        }
    }
}
