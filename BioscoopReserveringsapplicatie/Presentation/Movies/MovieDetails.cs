namespace BioscoopReserveringsapplicatie
{
    static class MovieDetails
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();
        private static MovieModel? movie;

        public static void Start(int movieId)
        {
            movie = MoviesLogic.GetMovieById(movieId);
            
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Bewerk film", () => MovieEdit.Start(movie.Id)),
                new Option<string>("Archiveer film", () => MovieDelete.Start(movie.Id)),
                new Option<string>("Terug", () => {Console.Clear(); MovieOverview.Start();}),
            };
            SelectionMenuUtil.Create(options, Print);
        }

        private static void Print()
        {
            if (movie != null) Console.WriteLine($"De film details zijn:\nFilm titel: {movie.Title} \nFilm beschrijving: {movie.Description} \nFilm genre(s): {string.Join(", ", movie.Genres)} \nFilm kijkwijzer: {movie.AgeCategory} \n\nWat zou je willen doen?");
        }
    }
}
