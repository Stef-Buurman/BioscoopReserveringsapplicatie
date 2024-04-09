namespace BioscoopReserveringsapplicatie
{
    static class MovieOverview
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();

        public static void Start()
        {
            int movieId = ShowAllMovies();

            if (movieId != 0)
            {
                MovieDetails.Start(movieId);
            }
        }

        private static int ShowAllMovies()
        {
            List<Option<int>> options = new List<Option<int>>();
            List<MovieModel> movies = MoviesLogic.GetAllMovies();

            foreach (MovieModel movie in movies)
            {
                options.Add(new Option<int>(movie.Id, movie.Title));
            }

            int movieId = SelectionMenuUtil.Create(options, 21, Print, () => { Console.Clear(); AdminMenu.Start(); });
            Console.Clear();
            return movieId;
        }

        private static void Print()
        {
            ColorConsole.WriteColorLine("[Dit zijn alle films die momenteel beschikbaar zijn:]", Globals.TitleColor);
        }
    }
}