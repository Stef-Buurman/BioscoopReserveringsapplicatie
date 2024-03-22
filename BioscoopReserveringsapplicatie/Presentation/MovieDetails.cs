static class MovieDetails
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();

    public static void Start(int movieId)
    {
        MovieModel movie = MoviesLogic.GetMovieById(movieId);

        var options = new List<Option<string>>
            {
                new Option<string>("Edit movie", () => MovieEdit.Start(movie.Id)),
                new Option<string>("Delete movie", () => MovieDelete.Start(movie.Id)),
                new Option<string>("Back", () => {Console.Clear(); MovieOverview.Start();}),
            };
        SelectionMenu.Create(options, $"Movie details:\nTitle: {movie.Title} \nDescription: {movie.Description} \nGenre: {movie.Genre} \nRating: {movie.Rating} \n\nWhat would you like to do?");
    }
}
