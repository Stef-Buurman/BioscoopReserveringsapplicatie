namespace BioscoopReserveringsapplicatie
{
    static class PreferredExperiences
    {
        private static ExperiencesLogic ExperienceLogic = new ExperiencesLogic();
        private static MoviesLogic MoviesLogic = new MoviesLogic();
        private static Func<ExperienceModel, string[]> experienceDataExtractor = ExtractExperienceData;

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

            Console.Clear();

            List<Option<int>> options = new List<Option<int>>();

            List<string> columnHeaders = new List<string>
            {
                "Experience Naam  ",
                "Film Naam",
                "Genres",
                "Leeftijdscategorie",
                "Tijdsduur",
                "Intensiteit",
            };

            List<ExperienceModel> experiences = ExperienceLogic.GetExperiencesByUserPreferences(UserLogic.CurrentUser);

            if (experiences.Count > 0)
            {
                int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, experiences, experienceDataExtractor);

                foreach (ExperienceModel experience in experiences)
                {
                    MovieModel movie = MoviesLogic.GetMovieById(experience.FilmId);

                    string experienceName = experience.Name;
                    if (experienceName.Length > 25)
                    {
                        experienceName = experienceName.Substring(0, 25) + "...";
                    }

                    if (movie.Title.Length > 25)
                    {
                        movie.Title = movie.Title.Substring(0, 25) + "...";
                    }

                    string genres = string.Join(",", movie.Genres);
                    if (genres.Length > 25)
                    {
                        genres = genres.Substring(0, 25) + "...";
                    }
                    string experienceInfo = string.Format("{0,-" + (columnWidths[0] + 1) + "} {1,-" + (columnWidths[1] + 1) + "} {2,-" + (columnWidths[2] + 1) + "} {3,-" + (columnWidths[3] + 1) + "} {4,-" + (columnWidths[4] + 1) + "} {5,-" + (columnWidths[5] + 1) + "}",
                    experienceName, movie.Title, genres, movie.AgeCategory.GetDisplayName(), experience.TimeLength + " minuten", experience.Intensity.GetDisplayName());
                    options.Add(new Option<int>(experience.Id, experienceInfo));
                }
                ColorConsole.WriteLineInfo("*Klik op escape om dit onderdeel te verlaten*\n");
                ColorConsole.WriteColorLine("Dit zijn uw aanbevolen experiences op basis van uw voorkeuren:", Globals.TitleColor);
                Print();
                int experienceId = new SelectionMenuUtil2<int>(options,
                    () =>
                    {
                        UserMenu.Start();
                    },
                    () =>
                    {
                        ShowExperiencesWithUserPreferences();
                    }, showEscapeabilityText: false).Create();
                Console.Clear();
                return experienceId;
            }
            else
            {
                ColorConsole.WriteColorLine("Er zijn geen experiences gevonden op basis van uw voorkeuren.", Globals.ErrorColor);
                ColorConsole.WriteColorLine("Druk op een [toets] om terug te gaan naar het gebruikersmenu.", Globals.ColorInputcClarification);
                Console.ReadKey();
                UserMenu.Start();
                return 0;
            }
        }

        private static void Print()
        {
            // Defineer de kolom koppen voor de tabel
            List<string> columnHeaders = new List<string>
            {
                "Experience Naam  ",
                "Film Naam",
                "Genres",
                "Leeftijdscategorie",
                "Tijdsduur",
                "Intensiteit",
            };

            List<ExperienceModel> allExperiences = ExperienceLogic.GetExperiencesByUserPreferences(UserLogic.CurrentUser);
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
                movie.Title,
                string.Join(",", movie.Genres),
                movie.AgeCategory.GetDisplayName(),
                experience.TimeLength + " minuten",
                experience.Intensity.GetDisplayName(),
            };
            return experienceInfo;
        }
    }
}
