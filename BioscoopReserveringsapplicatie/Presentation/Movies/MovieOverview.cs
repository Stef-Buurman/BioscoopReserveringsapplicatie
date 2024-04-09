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
                SelectionMenuUtil.Create(options, () => Console.WriteLine($"Kies een categorie om te bekijken:"));
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

            options.Add(new Option<int>(0, "Terug", () => { Console.Clear(); Start(); }));

            int movieId = SelectionMenuUtil.Create(options, Print);
            Console.Clear();
            ShowMovieDetails(movieId);
            return movieId;
        }

        private static int ShowAllArchivedMovies()
        {
            List<MovieModel> archivedMovies = MoviesLogic.GetAllArchivedMovies();
            return ShowMovies(archivedMovies);
        }

        private static int ShowAllActiveMovies()
        {
            List<MovieModel> activeMovies = MoviesLogic.GetAllActiveMovies();
            return ShowMovies(activeMovies);
        }

        private static int ShowAllMovies()
        {
            List<MovieModel> allMovies = MoviesLogic.GetAllMovies();
            return ShowMovies(allMovies);
        }

        private static void Print()
        {
            Console.WriteLine("Kies een film om de details te bekijken:");
        }
    }
}