static class MovieDelete
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();

    public static void Start(int movieId)
    {
        MovieModel movie = MoviesLogic.GetMovieById(movieId);
        Console.Write($"Are you sure you want to delete the movie {movie.Title}? (Y/N)");
        string input = Console.ReadLine().ToUpper();
        if (input == "Y")
        {
            MoviesLogic.RemoveMovie(movieId);
            Console.WriteLine($"Movie {movie.Title} has been deleted.");
            Console.WriteLine("Press any key to return.");
            Console.ReadKey();
            Console.Clear();
            MovieOverview.Start();
        }
        else
        {
            Console.WriteLine($"Movie {movie.Title} has not been deleted.");
            Console.WriteLine("Press any key to return.");
            Console.ReadKey();
            Console.Clear();
            MovieDetails.Start(movieId);
        }
    }
}
