static class MovieEdit
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();

    public static void Start(int movieId)
    {
        MovieModel movie = MoviesLogic.GetMovieById(movieId);

        Console.WriteLine("Voer nieuwe filmdetails in (druk op Enter om de huidige te behouden):");

        Console.Write("Titel: ");
        string newTitle = EditDefaultValueUtil.EditDefaultValue(movie.Title);

        Console.Write("Beschrijving: ");
        string newDescription = EditDefaultValueUtil.EditDefaultValue(movie.Description);

        Console.Write("Genre: ");
        string newGenre = EditDefaultValueUtil.EditDefaultValue(movie.Genre);

        Console.Write("Beoordeling: ");
        string newRating = EditDefaultValueUtil.EditDefaultValue(movie.Rating);

        var options = new List<Option<string>>
            {
                new Option<string>("Bewerk film", () => {
                    if (MoviesLogic.EditMovie(movie.Id, newTitle, newDescription, newGenre, newRating))
                        {
                            Console.WriteLine("Filmdetails zijn succesvol bijgewerkt!");
                            MovieDetails.Start(movie.Id);
                        }
                        else
                        {
                            Console.WriteLine("Filmdetails konden niet worden bijgewerkt.");
                            MovieDetails.Start(movie.Id);
                        }
                }),
                new Option<string>("Annuleren", () => {Console.Clear(); MovieDetails.Start(movie.Id);}),
            };
        SelectionMenu.Create(options, () => Console.WriteLine($"Weet u zeker dat u de filmdetails van {movie.Title} wilt bewerken?"));
    }
}
