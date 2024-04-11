namespace BioscoopReserveringsapplicatie
{
    static class MovieOverview
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();

        public static void Start()
        {
            Console.Clear();
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Alle active films", () => ShowAllActiveMovies()),
                new Option<string>("Alle gearchiveerde films", () => ShowAllArchivedMovies()),
                new Option<string>("Alle films", () => ShowAllMovies()),
                new Option<string>("Terug", () => AdminMenu.Start()),
            };
            SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Kies een categorie om te bekijken: \n", Globals.TitleColor));
        }

        private static void ShowMovieDetails(int movieId)
        {
            if (movieId != 0)
            {
                MovieDetails.Start(movieId);
            }
        }

        private static int ShowMovies(List<MovieModel> movies)
        {
            List<Option<int>> options = new List<Option<int>>();

            foreach (MovieModel movie in movies)
            {
                options.Add(new Option<int>(movie.Id, movie.Title));
            }

            int movieId = SelectionMenuUtil.Create(options, 21, Print, () => { Console.Clear(); AdminMenu.Start(); });
            Console.Clear();
            ShowMovieDetails(movieId);
            return movieId;
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
            ColorConsole.WriteColorLine("Dit zijn alle films die momenteel beschikbaar zijn:", Globals.TitleColor);
        }
    }
}