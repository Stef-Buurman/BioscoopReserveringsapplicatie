static class MovieDetails
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();
    private static MovieModel movie;

    public static void Start(int movieId)
    {
        movie = MoviesLogic.GetMovieById(movieId);

        var options = new List<Option<string>>
            {
                new Option<string>("Bewerk film", () => MovieEdit.Start(movie.Id)),
                new Option<string>("Verwijder film", () => MovieDelete.Start(movie.Id)),
                new Option<string>("Terug", () => {Console.Clear(); MovieOverview.Start();}),
            };
        SelectionMenu.Create(options, Print);
    }

    private static void Print()
    {
        if (movie != null) Console.WriteLine($"Film details:\nTitel: {movie.Title} \nBeschrijving: {movie.Description} \nGenre: {movie.Genre} \nBeoordeling: {movie.Rating} \n\nWat zou je willen doen?");
    }
}
