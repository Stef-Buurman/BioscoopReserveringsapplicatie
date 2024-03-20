static class MovieDelete
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();

    public static void Start(int movieId)
    {
        MovieModel movie = MoviesLogic.GetMovieById(movieId);

    }
}
