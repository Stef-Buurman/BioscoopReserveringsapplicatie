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

        options.Add(new Option<int>(0, "Back", () => { Console.Clear(); AdminMenu.Start(); }));

        int movieId = SelectionMenu.Create(options, Print);
        Console.Clear();
        return movieId;
    }

    private static void Print()
    {
        Console.WriteLine("Dit zijn alle films die momenteel beschikbaar zijn:");
    }
}
