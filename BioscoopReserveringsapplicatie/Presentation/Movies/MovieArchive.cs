namespace BioscoopReserveringsapplicatie
{
    static class MovieArchive
    {
        private static MovieLogic MoviesLogic = new MovieLogic();

        public static void Start(int movieId, bool archive)
        {
            MovieModel movie = MoviesLogic.GetById(movieId);
            Console.Clear();
            Print(movie.Title, movie.Description, movie.Genres, movie.AgeCategory, archive);

            if (archive)
            {
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
                new SelectionMenuUtil2<string>(options).Create();
            }
            else
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Ja", () => {
                        MoviesLogic.Unarchive(movieId);
                        Console.Clear();
                        ColorConsole.WriteColorLine($"De Film: {movie.Title} is gedearchiveerd!", Globals.SuccessColor);
                        Thread.Sleep(4000);
                        MovieOverview.Start();
                    }),
                    new Option<string>("Nee", () => {
                        MovieDetails.Start(movieId);
                    }),
                };
                new SelectionMenuUtil2<string>(options).Create();
            }
        }

        private static void Print(string title, string description, List<Genre> genres, AgeCategory rating, bool archive)
        {
            ColorConsole.WriteColorLine("[Film details]", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film titel: ]{title}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film beschrijving: ]{description}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film genre(s): ]{string.Join(", ", genres)}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film kijkwijzer ]{rating.GetDisplayName()}\n", Globals.MovieColor);
            if (archive)
            {
                ColorConsole.WriteColorLine($"Weet u zeker dat u de film {title} wilt [archiveren]?", Globals.ColorInputcClarification);
            }
            else
            {
                ColorConsole.WriteColorLine($"Weet u zeker dat u de film {title} wilt [dearchiveren]?", Globals.ColorInputcClarification);
            }
        }
    }
}
