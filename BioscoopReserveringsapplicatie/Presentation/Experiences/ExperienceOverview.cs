namespace BioscoopReserveringsapplicatie
{
    static class ExperienceOverview
    {
        private static ExperiencesLogic ExperiencesLogic = new ExperiencesLogic();
        private static MoviesLogic MoviesLogic = new MoviesLogic();

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

        private static int ShowExperiences(List<ExperiencesModel> experiences)
        {
            Console.Clear();
            List<Option<int>> options = new List<Option<int>>();

            foreach (ExperiencesModel experience in experiences)
            {
                MovieModel movie = ExperiencesLogic.GetMovieById(experience.FilmId);
                string experienceInfo = $"{experience.Name,-15} {string.Join(",", movie.Genres),-21} {movie.AgeCategory.GetDisplayName(),-19} {experience.Intensity,-12} {(experience.Archived ? "Ja" : "Nee")}";
                options.Add(new Option<int>(experience.Id, experienceInfo));
            }
            ColorConsole.WriteColorLine("Dit zijn alle experiences die momenteel beschikbaar zijn:", Globals.TitleColor);
            int experienceId = new SelectionMenuUtil2<int>(options,
                () =>
                {
                    Start();
                }, 
                () => 
                {
                    ShowExperiences(experiences);
                }).Create();
            Console.Clear();
            ShowExperienceDetails(experienceId);
            return experienceId;
        }

        private static void ShowAllArchivedExperiences()
        {
            List<ExperiencesModel> archivedExperiences = ExperiencesLogic.GetAllArchivedExperiences();

            if (archivedExperiences.Count == 0) PrintWhenNoExperiencesFound("Er zijn geen gearchiveerde experiences gevonden.");
            else ShowExperiences(archivedExperiences);
        }

        private static void ShowAllActiveExperiences()
        {
            List<ExperiencesModel> activeExperiences = ExperiencesLogic.GetAllActiveExperiences();

            if (activeExperiences.Count == 0) PrintWhenNoExperiencesFound("Er zijn geen actieve experiences gevonden.");
            else ShowExperiences(activeExperiences);
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

        // private static void Print()
        // {
        //     Console.WriteLine("Dit zijn alle experiences die momenteel beschikbaar zijn:\n");
        //     Console.WriteLine("Experience Naam   Genres                 Leeftijdscategorie  Intensiteit  Gearchiveerd");
        //     Console.WriteLine("----------------  ---------------------  ------------------  -----------  ------------");
        // }
        private static void Print()
        {
            List<string> columnHeaders = new List<string>
            {
                "Experience Naam",
                "Genres",
                "Leeftijdscategorie",
                "Intensiteit",
                "Gearchiveerd"
            };

            List<ExperiencesModel> allExperiences = ExperiencesLogic.GetExperiences();
            int[] columnWidths = CalculateColumnWidths(columnHeaders, allExperiences);

            Console.WriteLine("Dit zijn alle experiences die momenteel beschikbaar zijn:\n");

            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write(columnHeaders[i].PadRight(columnWidths[i] + 2));
            }
            Console.WriteLine();

            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write(new string('-', columnWidths[i]));
                if (i < columnHeaders.Count - 1) Console.Write("  ");
            }
            Console.WriteLine();
        }

        private static int[] CalculateColumnWidths(List<string> columnHeaders, List<ExperiencesModel> experiences)
        {
            int[] columnWidths = new int[columnHeaders.Count];

            foreach (string header in columnHeaders)
            {
                int index = columnHeaders.IndexOf(header);
                columnWidths[index] = header.Length;
            }

            foreach (ExperiencesModel experience in experiences)
            {
                MovieModel movie = ExperiencesLogic.GetMovieById(experience.FilmId);

                string[] experienceInfo = {
                    experience.Name,
                    string.Join(",", movie.Genres),
                    movie.AgeCategory.ToString(),
                    movie.AgeCategory.GetDisplayName(),
                    experience.Archived ? "Ja" : "Nee"
                };

                for (int i = 0; i < experienceInfo.Length; i++)
                {
                    if (experienceInfo[i].Length > columnWidths[i])
                    {
                        columnWidths[i] = experienceInfo[i].Length;
                    }
                }
            }
            return columnWidths;
        }
    }
}
