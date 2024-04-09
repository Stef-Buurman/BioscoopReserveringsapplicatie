namespace BioscoopReserveringsapplicatie
{
    static class MovieArchive
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();

        public static void Start(int movieId)
        {
            MovieModel movie = MoviesLogic.GetMovieById(movieId);
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    MoviesLogic.Archive(movieId);
                    MovieOverview.Start();
                }),
                new Option<string>("Nee", () => {
                    MovieDetails.Start(movieId);
                }),
            };
            SelectionMenuUtil.Create(options, () => Print(movie.Title, movie.Description, movie.Genres, movie.AgeCategory));
        }

        private static void Print(string title, string description, List<Genre> genres, AgeCategory rating)
        {
            Console.WriteLine("De film details zijn:");
            Console.WriteLine($"Film titel: {title}");
            Console.WriteLine($"Film beschrijving: {description}");
            Console.WriteLine($"Film genre(s): {string.Join(", ", genres)}");
            Console.WriteLine($"Film kijkwijzer: {rating}\n");

            Console.WriteLine($"Weet u zeker dat u de film {title} wilt archiveren?");
        }
    }
}
