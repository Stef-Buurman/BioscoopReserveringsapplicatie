using System.Globalization;

namespace BioscoopReserveringsapplicatie
{
    public static class ScheduledExperiences
    {
        private static ExperienceLogic ExperienceLogic = new ExperienceLogic();
        private static MovieLogic MoviesLogic = new MovieLogic();
        private static Func<ExperienceModel, string[]> experienceDataExtractor = ExtractExperienceData;

        public static void Start(DateTime? date = null)
        {
            ShowExperiences(date);
        }

        private static void ShowExperienceDetails(int experienceId, DateTime date)
        {
            if (experienceId != 0)
            {
                ScheduledExperienceDetails.Start(experienceId, date);
            }
        }

        private static int ShowExperiences(DateTime? date = null)
        {
            if (UserLogic.CurrentUser == null)
            {
                Console.WriteLine("Geen gebruiker gevonden");
                return 0;
            }

            if (date == null)
            {
                date = DateTime.Now;
            }

            int currentWeek = ISOWeek.GetWeekOfYear((DateTime)date);
            int currentYear = date.Value.Year;

            DateTime firstDayOfWeek = ISOWeek.ToDateTime(currentYear, currentWeek, DayOfWeek.Monday);

            DateTime lastDayOfWeek = firstDayOfWeek.AddDays(6);

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

            List<ExperienceModel> experiences = ExperienceLogic.GetScheduledExperiences((DateTime)date);

            if (experiences.Count > 0)
            {
                int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, experiences, experienceDataExtractor);

                foreach (ExperienceModel experience in experiences)
                {
                    MovieModel movie = MoviesLogic.GetById(experience.FilmId);

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
                ColorConsole.WriteLineInfoHighlight("*Klik op [Escape] om terug te gaan*", Globals.ColorInputcClarification);

                ColorConsole.WriteLineInfoHighlight("*Klik op het [linkerpijltje] en [rechterpijltje] om door de weken te scrollen*\n", Globals.ColorInputcClarification);

                ColorConsole.WriteColorLine($"Week {currentWeek} - {firstDayOfWeek.ToString("dd-MM-yyyy")} - {lastDayOfWeek.ToString("dd-MM-yyyy")}\n", Globals.ColorInputcClarification);
                ColorConsole.WriteColorLine("Dit zijn de ingeplande experiences in deze week:\n", Globals.TitleColor);

                Print((DateTime)date);
                int experienceId = new SelectionMenuUtil<int>(options,
                    () =>
                    {
                        AdminMenu.Start();
                    },
                    () =>
                    {
                        ShowExperiences(date);
                    },
                    new List<KeyAction>()
                    {
                        new KeyAction(ConsoleKey.LeftArrow, () => {
                            if (firstDayOfWeek.AddDays(-7) >= DateTime.Now.AddDays(-7))
                            {
                            ShowExperiences(date.Value.AddDays(-7));
                            }}),
                        new KeyAction(ConsoleKey.RightArrow, () => {
                            if (lastDayOfWeek.AddDays(7).Year == currentYear && currentWeek <= ISOWeek.GetWeekOfYear(DateTime.Now) + 1){
                            ShowExperiences(date.Value.AddDays(7));
                            }}),
                    }, showEscapeabilityText: false).Create();

                ShowExperienceDetails(experienceId, (DateTime)date);
                return experienceId;
            }
            else
            {
                ColorConsole.WriteLineInfoHighlight("*Klik op [Escape] om terug te gaan*", Globals.ColorInputcClarification);

                ColorConsole.WriteLineInfoHighlight("*Klik op het [linkerpijltje] en [rechterpijltje] om door de weken te scrollen*\n", Globals.ColorInputcClarification); ;

                ColorConsole.WriteColorLine($"Week {currentWeek} - {firstDayOfWeek.ToString("dd-MM-yyyy")} - {lastDayOfWeek.ToString("dd-MM-yyyy")}\n", Globals.ColorInputcClarification);

                ColorConsole.WriteColorLine("Er zijn geen ingeplande experiences gevonden in deze week.", Globals.ErrorColor);

                while (true)
                {
                    ConsoleKeyInfo key = Console.ReadKey(true);

                    if (key.Key == ConsoleKey.LeftArrow)
                    {
                        if (firstDayOfWeek.AddDays(-7) >= DateTime.Now.AddDays(-7))
                        {
                            ShowExperiences(date.Value.AddDays(-7));
                            break;
                        }
                    }
                    else if (key.Key == ConsoleKey.RightArrow)
                    {
                        if (lastDayOfWeek.AddDays(7).Year == currentYear && currentWeek <= ISOWeek.GetWeekOfYear(DateTime.Now) + 1)
                        {
                            ShowExperiences(date.Value.AddDays(7));
                            break;
                        }
                    }
                    else if (key.Key == ConsoleKey.Escape)
                    {
                        ReadLineUtil.EscapeKeyPressed(() => { AdminMenu.Start(); }, () => { ShowExperiences(date); });
                        break;
                    }
                }

                return 0;
            }
        }

        private static void Print(DateTime date)
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

            List<ExperienceModel> allExperiences = ExperienceLogic.GetExperiencesByUserPreferences(UserLogic.CurrentUser, date);
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
            MovieModel movie = MoviesLogic.GetById(experience.FilmId);

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