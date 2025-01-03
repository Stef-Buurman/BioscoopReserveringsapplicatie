namespace BioscoopReserveringsapplicatie
{
    static class MovieArchive
    {
        private static MovieLogic MoviesLogic = new MovieLogic();

        public static void Start(int movieId)
        {
            Console.Clear();
            MovieModel movie = MoviesLogic.GetById(movieId);
            if (movie == null)
            {
                ColorConsole.WriteColorLine("Er is geen film gevonden.", Globals.ErrorColor);
                WaitUtil.WaitTime(2000);
                MovieOverview.Start();
                return;
            }
            Print(movie.Title, movie.Description, movie.Genres, movie.AgeCategory, movie.Status);

            if (movie.Status == Status.Active)
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Ja", () => {
                        MoviesLogic.Archive(movieId);
                        Console.Clear();
                        ColorConsole.WriteColorLine($"De Film: {movie.Title} is gearchiveerd!", Globals.SuccessColor);
                        WaitUtil.WaitTime(4000);
                        MovieOverview.Start();
                    }),
                    new Option<string>("Nee", () => {
                        MovieDetails.Start(movieId);
                    }),
                };
                new SelectionMenuUtil<string>(options, new Option<string>("Nee")).Create();
            }
            else
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Ja", () => {
                        MoviesLogic.Unarchive(movieId);
                        Console.Clear();
                        ColorConsole.WriteColorLine($"De Film: {movie.Title} is gedearchiveerd!", Globals.SuccessColor);
                        WaitUtil.WaitTime(4000);
                        MovieOverview.Start();
                    }),
                    new Option<string>("Nee", () => {
                        MovieDetails.Start(movieId);
                    }),
                };
                new SelectionMenuUtil<string>(options, new Option<string>("Nee")).Create();
            }
        }

        private static void Print(string title, string description, List<Genre> genres, AgeCategory rating, Status status)
        {
            ColorConsole.WriteColorLine("[Film details]", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film titel: ]{title}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film beschrijving: ]{description}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film genre(s): ]{string.Join(", ", genres)}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film leeftijdscategorie ]{rating.GetDisplayName()}\n", Globals.MovieColor);
            if (status == Status.Active)
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
