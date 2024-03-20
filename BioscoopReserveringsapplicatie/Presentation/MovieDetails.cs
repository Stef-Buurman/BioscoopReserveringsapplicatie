static class MovieDetails
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();

    public static void Start(int movieId)
    {
        MovieModel movie = MoviesLogic.GetMovieById(movieId);

        if (movie != null)
        {
            Console.WriteLine("Movie details:");
            Console.WriteLine($"Title: {movie.Title}");
            Console.WriteLine($"Description: {movie.Description}");
            Console.WriteLine($"Genre: {movie.Genre}");
            Console.WriteLine($"Rating: {movie.Rating}");
        }
        else
        {
            Console.WriteLine("Movie not found.");
        }
    }
}
