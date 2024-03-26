namespace BioscoopReserveringsapplicatie
{
    static class MovieDetails
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();
        private static MovieModel? movie;

        public static void Start(int movieId)
        {
            movie = MoviesLogic.GetMovieById(movieId);

            var options = new List<Option<string>>
            {
                new Option<string>("Edit movie", () => MovieEdit.Start(movie.Id)),
                new Option<string>("Delete movie", () => MovieDelete.Start(movie.Id)),
                new Option<string>("Back", () => {Console.Clear(); MovieOverview.Start();}),
            };
            SelectionMenu.Create(options, Print);
        }

        private static void Print()
        {
            if (movie != null) Console.WriteLine($"Movie details:\nTitle: {movie.Title} \nDescription: {movie.Description} \nGenre: {string.Join(", ", movie.Genres)} \nRating: {movie.Rating} \n\nWhat would you like to do?");
        }
    }
}
