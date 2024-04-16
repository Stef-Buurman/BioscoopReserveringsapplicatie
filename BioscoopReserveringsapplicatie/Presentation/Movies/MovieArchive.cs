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
                    Console.Clear();
                    ColorConsole.WriteColorLine($"De Film: {movie.Title} is gearchiveerd!", Globals.SuccessColor);
                    Thread.Sleep(4000);
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
            ColorConsole.WriteColorLine("[Film details]", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film titel: ]{title}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film beschrijving: ]{description}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film genre(s): ]{string.Join(", ", genres)}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film kijkwijzer ]{rating.GetDisplayName()}\n", Globals.MovieColor);
            ColorConsole.WriteColorLine($"Weet u zeker dat u de film {title} wilt [archiveren]?", Globals.ColorInputcClarification);
        }
    }
}
