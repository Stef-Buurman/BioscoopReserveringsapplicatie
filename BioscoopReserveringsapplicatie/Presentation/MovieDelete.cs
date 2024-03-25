namespace BioscoopReserveringsapplicatie
{
    static class MovieDelete
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();

        public static void Start(int movieId)
        {
            MovieModel movie = MoviesLogic.GetMovieById(movieId);

            var options = new List<Option<string>>
            {
                new Option<string>("Delete movie", () => {
                    MoviesLogic.RemoveMovie(movieId);
                    Console.WriteLine($"Movie {movie.Title} has been deleted.");
                    Console.Clear();
                    MovieOverview.Start();
                }),
                new Option<string>("Cancel", () => {
                    Console.WriteLine($"Movie {movie.Title} has not been deleted.");
                    Console.Clear();
                    MovieDetails.Start(movieId);
                }),
            };
            SelectionMenu.Create(options, () => Console.WriteLine($"Are you sure you want to delete the movie {movie.Title}?"));
        }
    }
}
