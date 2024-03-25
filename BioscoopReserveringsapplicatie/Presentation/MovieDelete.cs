namespace BioscoopReserveringsapplicatie
{
    static class MovieDelete
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();

        public static void Start(int movieId)
        {
            MovieModel movie = MoviesLogic.GetMovieById(movieId);

            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Verwijder film", () => {
                    MoviesLogic.RemoveMovie(movieId);
                    Console.WriteLine($"Film {movie.Title} is verwijderd.");
                    Console.Clear();
                    MovieOverview.Start();
                }),
                new Option<string>("Annuleer", () => {
                    Console.WriteLine($"Bent u zeker dat u de film {movie.Title} wilt verwijderen?");
                    Console.Clear();
                    MovieDetails.Start(movieId);
                }),
            };
            SelectionMenu.Create(options, () => Console.WriteLine($"Weet u zeker dat u de film {movie.Title} wilt verwijderen?"));
        }
    }
}
