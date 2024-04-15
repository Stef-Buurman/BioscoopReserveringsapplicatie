namespace BioscoopReserveringsapplicatie
{
    static class MovieEdit
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();
        private static Action actionWhenEscapePressed = MovieOverview.Start;

        public static void Start(int movieId)
        {
            actionWhenEscapePressed = () => MovieDetails.Start(movieId);
            Console.Clear();

            MovieModel movie = MoviesLogic.GetMovieById(movieId);

            string newTitle = ReadLineUtil.EditValue(movie.Title, () =>
            {
                ColorConsole.WriteColorLine("Voer nieuwe filmdetails in:\n", Globals.TitleColor);
                ColorConsole.WriteColor("Voer de film [titel] in: ", Globals.ColorInputcClarification);
            },
            actionWhenEscapePressed,
            "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n");

            Console.Write("Voer de film beschrijving in: ");
            string newDescription = ReadLineUtil.EditValue(movie.Description, () =>
            {
                ColorConsole.WriteColorLine("Voer nieuwe filmdetails in:\n", Globals.TitleColor);
                ColorConsole.WriteColorLine($"Voer de film [titel] in: {newTitle}", Globals.ColorInputcClarification);
                ColorConsole.WriteColor("Voer de film [beschrijving] in: ", Globals.ColorInputcClarification);
            }, 
            actionWhenEscapePressed,
            "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n");

            List<Genre> genres = new List<Genre>();
            List<Genre> availableGenres = Globals.GetAllEnum<Genre>();
            bool firstTime = true;
            while (genres.Count < 3)
            {
                Genre genre;
                if (firstTime)
                {
                    genre = SelectionMenuUtil.Create(availableGenres, () =>
                    {
                        ColorConsole.WriteColorLine("Voer film genre in", Globals.TitleColor);
                        ColorConsole.WriteColorLine("U kunt maximaal 3 verschillende genres kiezen.\n", Globals.TitleColor);
                        ColorConsole.WriteColorLine("Kies een [genre]: \n", Globals.ColorInputcClarification);
                    }, actionWhenEscapePressed
                    );
                }
                else
                {
                    genre = SelectionMenuUtil.Create(availableGenres, () => ColorConsole.WriteColorLine($"Kies een [genre] \nDeze [genres] heeft u momenteel geselecteerd: [{string.Join(", ", genres)}]\n", Globals.ColorInputcClarification), actionWhenEscapePressed);
                }

                if (genre != default && availableGenres.Contains(genre))
                {
                    availableGenres.Remove(genre);
                    genres.Add(genre);
                }
                else
                {
                    ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                }
                firstTime = false;
            }


            List<AgeCategory> AgeGenres = Globals.GetAllEnum<AgeCategory>();
            List<string> EnumDescription = AgeGenres.Select(o => o.GetDisplayName()).ToList();
            string selectedDescription = SelectionMenuUtil.Create(EnumDescription, () => ColorConsole.WriteColorLine("Wat is uw [leeftijdscatagorie]: \n", Globals.ColorInputcClarification),actionWhenEscapePressed);
            AgeCategory newRating = AgeGenres.First(o => o.GetDisplayName() == selectedDescription);


            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    if (MoviesLogic.EditMovie(movie.Id, newTitle, newDescription, genres, newRating))
                        {
                            MovieDetails.Start(movie.Id);
                        }
                        else
                        {
                            List<Option<string>> options = new List<Option<string>>
                            {
                                new Option<string>("Terug", () => {Console.Clear(); MovieDetails.Start(movie.Id);}),
                            };
                            SelectionMenuUtil.Create(options, () => Console.WriteLine("Er is een fout opgetreden tijdens het bewerken van de film. Probeer het opnieuw.\n"));
                        }
                }),
                new Option<string>("Nee", () => {Console.Clear(); MovieDetails.Start(movie.Id);}),
            };
            SelectionMenuUtil.Create(options, () => Print(movie.Title, newTitle, newDescription, genres, newRating));
        }

        private static void Print(string currentTitle, string newTitle, string description, List<Genre> genres, AgeCategory rating)
        {
            ColorConsole.WriteColorLine("[De nieuwe film details:]", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film titel: ]{newTitle}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film beschrijving: ]{description}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film genre(s): ]{string.Join(", ", genres)}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film kijkwijzer ]{rating.GetDisplayName()}\n", Globals.MovieColor);
            ColorConsole.WriteColorLine($"Weet u zeker dat u de filmdetails van {currentTitle} wilt [bewerken]?", Globals.ColorInputcClarification);
        }
    }
}