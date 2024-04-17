namespace BioscoopReserveringsapplicatie
{
    static class MovieEdit
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();
        private static Action actionWhenEscapePressed = MovieOverview.Start;
        private static MovieModel? movie = null;

        private static string newTitle = "";
        private static string newDescription = "";
        private static List<Genre> newGenres = new List<Genre>();
        private static AgeCategory newRating = AgeCategory.Undefined;

        private static string _returnToTitle = "Title";
        private static string _returnToDescription = "Description";
        private static string _returnToGenres = "Genres";
        private static string _returnToRating = "Rating";

        public static void Start(int movieId, string returnTo = "")
        {
            movie = MoviesLogic.GetMovieById(movieId);
            actionWhenEscapePressed = () => MovieDetails.Start(movieId);

            Console.Clear();

            if(returnTo == "" || returnTo == _returnToTitle)
            {
                MovieName();
                returnTo = "";
            }

            if(returnTo == "" || returnTo == _returnToDescription)
            {
                MovieDescription(movieId);
                returnTo = "";
            }

            if(returnTo == "" || returnTo == _returnToGenres)
            {
                SelectMovieGenres(movieId);
                returnTo = "";
            }

            if(returnTo == "" || returnTo == _returnToRating)
            {
                SelectMovieRating(movieId);
                returnTo = "";
            }
            
            
            Console.Clear();
            Print(movie.Title, newTitle, newDescription, newGenres, newRating);

            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    if (MoviesLogic.EditMovie(movieId, newTitle, newDescription, newGenres, newRating))
                        {
                            MovieDetails.Start(movieId);
                        }
                        else
                        {
                            List<Option<string>> options = new List<Option<string>>
                            {
                                new Option<string>("Terug", () => {Console.Clear(); MovieDetails.Start(movieId);}),
                            };
                            Console.WriteLine("Er is een fout opgetreden tijdens het bewerken van de film. Probeer het opnieuw.\n");
                            new SelectionMenuUtil2<string>(options).Create();
                        }
                }),
                new Option<string>("Nee, pas de film verder aan", () => {Start(movie.Id, _returnToRating);}),
                new Option<string>("Nee, stop met aanpassen", () => {Console.Clear(); MovieDetails.Start(movie.Id);})
            };
            new SelectionMenuUtil2<string>(options).Create();
        }

        private static void MovieName()
        {
            Console.Clear();
            PrintEditingMovie();
            
            newTitle = ReadLineUtil.EditValue(movie.Title, () =>
            {
                ColorConsole.WriteColorLine("Voer nieuwe filmdetails in:\n", Globals.TitleColor);
                ColorConsole.WriteColor("Voer de film [titel] in: ", Globals.ColorInputcClarification);
            },
            actionWhenEscapePressed,
            "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n");

            while (string.IsNullOrEmpty(newTitle))
            {
                newTitle = ReadLineUtil.EditValue(movie.Title, () =>
                {
                    ColorConsole.WriteColorLine("Voer nieuwe filmdetails in:\n", Globals.TitleColor);
                    ColorConsole.WriteColor("Voer de film [titel] in: ", Globals.ColorInputcClarification);
                },
                actionWhenEscapePressed,
                "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n");
                }
        }

        private static void MovieDescription(int movieId)
        {
            Console.Clear();
            PrintEditingMovie();

            Console.Write("Voer de film beschrijving in: ");
            newDescription = ReadLineUtil.EditValue(movie.Description, () =>
            {
                ColorConsole.WriteColorLine("Voer nieuwe filmdetails in:\n", Globals.TitleColor);
                ColorConsole.WriteColor("Voer de film [beschrijving] in: ", Globals.ColorInputcClarification);
            }, 
            () => Start(movieId, _returnToTitle),
            "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n");

            while (string.IsNullOrEmpty(newDescription))
            {
                Console.Write("Voer de film beschrijving in: ");
                newDescription = ReadLineUtil.EditValue(movie.Description, () =>
                {
                    ColorConsole.WriteColorLine("Voer nieuwe filmdetails in:\n", Globals.TitleColor);
                    ColorConsole.WriteColor("Voer de film [beschrijving] in: ", Globals.ColorInputcClarification);
                }, 
                () => Start(movieId, _returnToTitle),
                "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n");
            }

            newGenres = new List<Genre>();
        }

        private static void SelectMovieGenres(int movieId)
        {
            List<Genre> availableGenres = Globals.GetAllEnum<Genre>();
            while (newGenres.Count < 3)
            {
                Genre genre;

                Console.Clear();
                PrintEditingMovie();
                ColorConsole.WriteColorLine("Kies film genres", Globals.TitleColor);
                ColorConsole.WriteColorLine("U kunt maximaal 3 verschillende genres kiezen.\n", Globals.TitleColor);
                ColorConsole.WriteColorLine("Kies een [genre]: \n", Globals.ColorInputcClarification);

                genre = new SelectionMenuUtil2<Genre>(availableGenres, () => Start(movieId, _returnToDescription), () => Start(movieId, _returnToGenres)).Create();

                if (genre != default && availableGenres.Contains(genre))
                {
                    availableGenres.Remove(genre);
                    newGenres.Add(genre);
                }
                else
                {
                    ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                }
            }
        }

        private static void SelectMovieRating(int movieId)
        {
            Console.Clear();
            PrintEditingMovie();
            ColorConsole.WriteColorLine("Wat is uw [leeftijdscatagorie]: \n", Globals.ColorInputcClarification);

            List<AgeCategory> AgeCatagories = Globals.GetAllEnum<AgeCategory>();
            List<Option<AgeCategory>> options = new List<Option<AgeCategory>>();
            foreach (AgeCategory option in AgeCatagories)
            {
                options.Add(new Option<AgeCategory>(option, option.GetDisplayName()));
            }
            newRating = new SelectionMenuUtil2<AgeCategory>(options, 
                () => 
                { 
                    newGenres = new List<Genre>(); 
                    Start(movieId, _returnToGenres); 
                }, 
                () => Start(movieId, _returnToRating), 
                new Option<AgeCategory>(newRating)).Create();
        }

        private static void PrintEditingMovie()
        {
            if(newTitle != "")
            {
                ColorConsole.WriteColorLine("[Aangepaste Film Details]", Globals.MovieColor);
                ColorConsole.WriteColorLine($"[Naam Film:] {newTitle}", Globals.MovieColor);
            }
            if(newDescription != "")
            {
                ColorConsole.WriteColorLine($"[Beschrijving Film:] {newDescription}", Globals.MovieColor);
            }
            if(newGenres.Count >= 1)
            {
                ColorConsole.WriteColorLine($"[Genres Film:] {string.Join(", ", newGenres)}", Globals.MovieColor);
            }
            if(newRating != AgeCategory.Undefined)
            {  
                ColorConsole.WriteColorLine($"[Beschrijving Film:] {newRating.GetDisplayName()}", Globals.MovieColor);
            }
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