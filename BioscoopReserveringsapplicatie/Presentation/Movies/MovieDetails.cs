namespace BioscoopReserveringsapplicatie
{
    static class MovieDetails
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();
        private static MovieModel? movie;

        public static void Start(int movieId)
        {
            Console.Clear();
            movie = MoviesLogic.GetMovieById(movieId);
            List<Option<string>> options;

            if (movie.Archived)
            {
                options = new List<Option<string>>
                {
                    new Option<string>("Bewerk film", () => MovieEdit.Start(movie.Id)),
                    new Option<string>("Dearchiveer film", () => MovieArchive.Start(movie.Id, false)),
                    new Option<string>("Terug", () => {Console.Clear(); MovieOverview.Start();}),
                };
            }
            else
            {
                options = new List<Option<string>>
                {
                    new Option<string>("Bewerk film", () => MovieEdit.Start(movie.Id)),
                    new Option<string>("Archiveer film", () => MovieArchive.Start(movie.Id, true)),
                    new Option<string>("Terug", () => {Console.Clear(); MovieOverview.Start();}),
                };
            }
            Print();
            new SelectionMenuUtil2<string>(options).Create();
        }

        private static void Print()
        {
            if (movie != null)
            {
                ColorConsole.WriteColorLine("[Film details]", Globals.MovieColor);
                ColorConsole.WriteColorLine($"[Film titel: ]{movie.Title}", Globals.MovieColor);
                ColorConsole.WriteColorLine($"[Film beschrijving: ]{movie.Description}", Globals.MovieColor);
                ColorConsole.WriteColorLine($"[Film genre(s): ]{string.Join(", ", movie.Genres)}", Globals.MovieColor);
                ColorConsole.WriteColorLine($"[Film kijkwijzer ]{movie.AgeCategory.GetDisplayName()}\n", Globals.MovieColor);
            }
        }
    }
}
