namespace BioscoopReserveringsapplicatie
{
    static class MovieOverview
    {
        private static MoviesLogic MoviesLogic = new MoviesLogic();

        public static void Start()
        {
            Console.Clear();
            ColorConsole.WriteColorLine("Kies een categorie om te bekijken: \n", Globals.TitleColor);

            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Film toevoegen", () => AddMovie.Start()),
                new Option<string>("Alle active films", () => ShowAllActiveMovies()),
                new Option<string>("Alle gearchiveerde films", () => ShowAllArchivedMovies()),
                new Option<string>("Alle films", () => ShowAllMovies()),
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
            ColorConsole.WriteColorLine("Dit zijn alle films die momenteel beschikbaar zijn:", Globals.TitleColor);

            List<Option<string>> options = new List<Option<string>>();

            foreach (MovieModel movie in movies)
            {
                options.Add(new Option<string>(movie.Title, () => ShowMovieDetails(movie.Id)));
            }

            new SelectionMenuUtil2<string>(options, () => AdminMenu.Start(), () => ShowMovies(movies)).Create();
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
    }
}