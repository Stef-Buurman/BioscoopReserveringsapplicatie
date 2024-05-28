namespace BioscoopReserveringsapplicatie
{
    static class MovieOverview
    {
        private static MovieLogic MoviesLogic = new MovieLogic();
        private static Func<MovieModel, string[]> movieDataExtractor = ExtractMovieData;

        public static void Start()
        {
            Console.Clear();
            ShowAllMovies();

            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Film toevoegen", () => AddMovie.Start()),
                new Option<string>("Alle actieve films bekijken", () => ShowAllActiveMovies()),
                new Option<string>("Alle gearchiveerde films bekijken", () => ShowAllArchivedMovies()),
                new Option<string>("Alle films bekijken", () => ShowAllMovies()),
                new Option<string>("Terug", () => AdminMenu.Start()),
            };
            new SelectionMenuUtil<string>(options).Create();
        }

        private static void ShowMovieDetails(int movieId)
        {
            if (movieId != 0)
            {
                MovieDetails.Start(movieId);
            }
        }

        private static void ShowMovies(List<MovieModel> movies)
        {
            Console.Clear();
            List<Option<int>> options = new List<Option<int>>();

            List<string> columnHeaders = new List<string>
            {
                "Film Naam",
                "Genres",
                "Leeftijdscategorie",
                "Status"
            };

            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, movies, movieDataExtractor);

            foreach (MovieModel movie in movies)
            {
                string movieTitle = movie.Title;
                if (movieTitle.Length > 25)
                {
                    movieTitle = movieTitle.Substring(0, 25) + "...";
                }

                string genres = string.Join(",", movie.Genres);
                if (genres.Length > 25)
                {
                    genres = genres.Substring(0, 25) + "...";
                }
                string movieInfo = string.Format("{0,-" + (columnWidths[0] + 2) + "} {1,-" + (columnWidths[1] + 2) + "} {2,-" + (columnWidths[2] + 2) +"} {3,-" + columnWidths[3] +"}",
                movieTitle, genres, movie.AgeCategory.GetDisplayName(), movie.Status.GetDisplayName());
                options.Add(new Option<int>(movie.Id, movieInfo));
            }
            ColorConsole.WriteLineInfoHighlight("*Klik op een film om de details te bekijken*", Globals.ColorInputcClarification);
            ColorConsole.WriteLineInfoHighlight("*Klik op [Escape] om terug te gaan*", Globals.ColorInputcClarification);
            ColorConsole.WriteLineInfoHighlight("*Klik op [T] om een film toe te voegen*", Globals.ColorInputcClarification);
            ColorConsole.WriteLineInfoHighlight("*Klik op [1] om alle films te tonen*", Globals.ColorInputcClarification);
            ColorConsole.WriteLineInfoHighlight("*Klik op [2] om alle active films te tonen*", Globals.ColorInputcClarification);
            ColorConsole.WriteLineInfoHighlight("*Klik op [3] om alle gearchiveerde films te tonen*\n", Globals.ColorInputcClarification);
            ColorConsole.WriteColorLine("Dit zijn alle films die momenteel bestaan:\n", Globals.TitleColor);
            Print();
            int movieId = new SelectionMenuUtil<int>(options,
                () =>
                {
                    AdminMenu.Start();
                }, 
                () => 
                {
                    ShowMovies(movies);
                },
                new List<KeyAction>()
                {
                    new KeyAction(ConsoleKey.T, () => AddMovie.Start()),
                    new KeyAction(ConsoleKey.D1, () => ShowAllMovies()),
                    new KeyAction(ConsoleKey.D2, () => ShowAllActiveMovies()),
                    new KeyAction(ConsoleKey.D3, () => ShowAllArchivedMovies()),
                }, showEscapeabilityText:false).Create();
            Console.Clear();
            ShowMovieDetails(movieId);
        }

        private static void ShowAllArchivedMovies()
        {
            List<MovieModel> archivedMovies = MoviesLogic.GetAllArchivedMovies();

            if (archivedMovies.Count == 0) PrintWhenNoMoviesFound("Er zijn geen gearchiveerde films gevonden.", "archived");
            ShowMovies(archivedMovies);
        }

        private static void ShowAllActiveMovies()
        {
            List<MovieModel> activeMovies = MoviesLogic.GetAllActiveMovies();

            if (activeMovies.Count == 0) PrintWhenNoMoviesFound("Er zijn geen actieve films gevonden.", "active");
            ShowMovies(activeMovies);
        }

        private static void ShowAllMovies()
        {
            List<MovieModel> allMovies = MoviesLogic.GetAll();

            if (allMovies.Count == 0) PrintWhenNoMoviesFound("Er zijn geen films gevonden.", "all");
            ShowMovies(allMovies);
        }

        private static void PrintWhenNoMoviesFound(string notFoundMessage, string filterType)
        {
            if(filterType == "all")
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Ja", () => {
                       AddMovie.Start();
                    }),
                    new Option<string>("Nee", () => {
                        AdminMenu.Start();
                    }),
                };
                Console.WriteLine(notFoundMessage);
                Console.WriteLine();
                Console.WriteLine("Wil je een film aanmaken?");
                new SelectionMenuUtil<string>(options, new Option<string>("Nee")).Create();
            }
            else
            {
                Console.Clear();
                Console.WriteLine(notFoundMessage);
                WaitUtil.WaitTime(500);
                Console.WriteLine("Terug naar movie overzicht...");
                WaitUtil.WaitTime(1500);
                Start();
            }
        }

        private static void Print()
        {
            List<string> columnHeaders = new List<string>
            {
                "Film Naam",
                "Genres",
                "Leeftijdscategorie",
                "Status"
            };

            List<MovieModel> allMovies = MoviesLogic.GetAll();
            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, allMovies, movieDataExtractor);

            Console.Write("".PadRight(3));
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write(columnHeaders[i].PadRight(columnWidths[i] + 3));
            }
            Console.WriteLine();

            Console.Write("".PadRight(3));
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write("".PadRight(columnWidths[i], '-').PadRight(columnWidths[i] + 3));
            }
            Console.WriteLine();
        }

        private static string[] ExtractMovieData(MovieModel movie)
        {
            string[] movieInfo = {
                movie.Title,
                string.Join(",", movie.Genres),
                movie.AgeCategory.GetDisplayName(),
                movie.Status.GetDisplayName()
            };
            return movieInfo;
        }
    }
}