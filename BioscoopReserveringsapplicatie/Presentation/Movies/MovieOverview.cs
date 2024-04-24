namespace BioscoopReserveringsapplicatie
{
    static class MovieOverview
    {
        private static MoviesLogic MoviesLogic = new MoviesLogic();
        private static Func<MovieModel, string[]> movieDataExtractor = ExtractMovieData;

        public static void Start()
        {
            Console.Clear();
            ColorConsole.WriteColorLine("Kies een categorie: \n", Globals.TitleColor);

            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Film toevoegen", () => AddMovie.Start()),
                new Option<string>("Alle actieve films bekijken", () => ShowAllActiveMovies()),
                new Option<string>("Alle gearchiveerde films bekijken", () => ShowAllArchivedMovies()),
                new Option<string>("Alle films bekijken", () => ShowAllMovies()),
                new Option<string>("Terug", () => AdminMenu.Start()),
            };
            new SelectionMenuUtil2<string>(options).Create();
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
                "Gearchiveerd"
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
                movieTitle, genres, movie.AgeCategory.GetDisplayName(), movie.Archived ? "Ja" : "Nee");
                options.Add(new Option<int>(movie.Id, movieInfo));
            }
            ColorConsole.WriteLineInfo("*Klik op escape om dit onderdeel te verlaten*\n");
            ColorConsole.WriteColorLine("Dit zijn alle films die momenteel beschikbaar zijn:\n", Globals.TitleColor);
            Print();
            int movieId = new SelectionMenuUtil2<int>(options,
                () =>
                {
                    AdminMenu.Start();
                }, 
                () => 
                {
                    ShowMovies(movies);
                }, showEscapeabilityText:false).Create();
            Console.Clear();
            ShowMovieDetails(movieId);
        }

        private static void ShowAllArchivedMovies()
        {
            List<MovieModel> archivedMovies = MoviesLogic.GetAllArchivedMovies();

            if (archivedMovies.Count == 0) PrintWhenNoMoviesFound("Er zijn geen gearchiveerde movies gevonden.");
            ShowMovies(archivedMovies);
        }

        private static void ShowAllActiveMovies()
        {
            List<MovieModel> activeMovies = MoviesLogic.GetAllActiveMovies();

            if (activeMovies.Count == 0) PrintWhenNoMoviesFound("Er zijn geen actieve movies gevonden.");
            ShowMovies(activeMovies);
        }

        private static void ShowAllMovies()
        {
            List<MovieModel> allMovies = MoviesLogic.GetAllMovies();

            if (allMovies.Count == 0) PrintWhenNoMoviesFound("Er zijn geen movies gevonden.");
            ShowMovies(allMovies);
        }

        private static void PrintWhenNoMoviesFound(string whichMovies)
        {
            Console.Clear();
            Console.WriteLine(whichMovies);
            Thread.Sleep(500);
            Console.WriteLine("Terug naar movie overzicht...");
            Thread.Sleep(1500);
            Start();
        }

        private static void Print()
        {
            List<string> columnHeaders = new List<string>
            {
                "Film Naam",
                "Genres",
                "Leeftijdscategorie",
                "Gearchiveerd"
            };

            List<MovieModel> allMovies = MoviesLogic.GetAllMovies();
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
                movie.Archived ? "Ja" : "Nee"
            };
            return movieInfo;
        }
    }
}