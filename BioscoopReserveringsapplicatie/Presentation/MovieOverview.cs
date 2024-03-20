static class MovieOverview
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();

    public static void Start()
    {
        List<Option<string>> MoviesAsOptions = MoviesLogic.getAllMoviesAsOptions();

        if (MoviesAsOptions.Count > 0)
        {
            SelectionMenu.Create(MoviesAsOptions, "This are all movies currently available:");
        }
        else
        {
            Console.WriteLine("No movies found.");
        }
    }
}
