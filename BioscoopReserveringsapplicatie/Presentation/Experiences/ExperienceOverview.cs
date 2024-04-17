namespace BioscoopReserveringsapplicatie
{
    static class ExperienceOverview
    {
        private static ExperiencesLogic ExperiencesLogic = new ExperiencesLogic();
        private static MoviesLogic MoviesLogic = new MoviesLogic();
        private static Func<ExperienceModel, string[]> experienceDataExtractor = ExtractExperienceData;

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
                "Gearchiveerd"
            };

            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, experiences, experienceDataExtractor);

            foreach (ExperienceModel experience in experiences)
            {
                MovieModel movie = MoviesLogic.GetMovieById(experience.FilmId);

                string experienceName = experience.Name;
                if (experienceName.Length > 25)
                {
                    experienceName = experienceName.Substring(0, 25) + "...";
                }

                string genres = string.Join(",", movie.Genres);
                if (genres.Length > 25)
                {
                    genres = genres.Substring(0, 25) + "...";
                }
                string experienceInfo = string.Format("{0,-" + (columnWidths[0] + 1) + "} {1,-" + (columnWidths[1] + 1) + "} {2,-" + (columnWidths[2] + 1) +"} {3,-" + (columnWidths[3] + 1) +"} {4,-" + columnWidths[4] + "}",
                experienceName, genres, movie.AgeCategory.GetDisplayName(), experience.Intensity, experience.Archived ? "Ja" : "Nee");
                options.Add(new Option<int>(experience.Id, experienceInfo));
            }
            ColorConsole.WriteLineInfo("*Klik op escape om dit onderdeel te verlaten*\n");
            ColorConsole.WriteColorLine("Dit zijn alle experiences die momenteel beschikbaar zijn:", Globals.TitleColor);
            Print();
            int experienceId = new SelectionMenuUtil2<int>(options,
                () =>
                {
                    Start();
                },
                () =>
                {
                    ShowExperiences(experiences);
                }, showEscapeabilityText:false).Create();
            Console.Clear();
            ShowExperienceDetails(experienceId);
            return experienceId;
        }

        private static void ShowAllArchivedExperiences()
        {
            List<ExperienceModel> archivedExperiences = ExperiencesLogic.GetAllArchivedExperiences();

            if (archivedExperiences.Count == 0) PrintWhenNoExperiencesFound("Er zijn geen gearchiveerde experiences gevonden.");
            else ShowExperiences(archivedExperiences);
        }

        private static void ShowAllActiveExperiences()
        {
            List<ExperienceModel> activeExperiences = ExperiencesLogic.GetAllActiveExperiences();

            if (activeExperiences.Count == 0) PrintWhenNoExperiencesFound("Er zijn geen actieve experiences gevonden.");
            else ShowExperiences(activeExperiences);
        }

        private static void ShowAllExperiences()
        {
            List<ExperienceModel> allExperiences = ExperiencesLogic.GetExperiences();

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
            List<string> columnHeaders = new List<string>
            {
                "Experience Naam  ",
                "Genres",
                "Leeftijdscategorie",
                "Intensiteit",
                "Gearchiveerd"
            };

            List<ExperienceModel> allExperiences = ExperiencesLogic.GetExperiences();
            // Bereken de breedte voor elke kolom op basis van de koptekst en experiences
            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, allExperiences, experienceDataExtractor);

            // Print de kop van de tabel
            Console.Write("".PadRight(3));
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write(columnHeaders[i].PadRight(columnWidths[i] + 2));
            }
            Console.WriteLine();

            // Print de "----"-lijn tussen de kop en de data
            Console.Write("".PadRight(3));
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write(new string('-', columnWidths[i]));
                if (i < columnHeaders.Count - 1) Console.Write("  ");
            }
            Console.WriteLine();
        }

        private static string[] ExtractExperienceData(ExperienceModel experience)
        {
            MovieModel movie = MoviesLogic.GetMovieById(experience.FilmId);

            string[] experienceInfo = {
                experience.Name,
                string.Join(",", movie.Genres),
                movie.AgeCategory.GetDisplayName(),
                experience.Intensity.GetDisplayName(),
                experience.Archived ? "Ja" : "Nee"
            };
            return experienceInfo;
        }
    }
}
