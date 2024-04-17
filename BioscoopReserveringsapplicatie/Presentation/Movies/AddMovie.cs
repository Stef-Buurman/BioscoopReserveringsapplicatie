namespace BioscoopReserveringsapplicatie
{
    static class AddMovie
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();
        private static Action actionWhenEscapePressed = AdminMenu.Start;

        private static string title = "";
        private static string description = "";
        private static List<Genre> genres = new List<Genre>();
        private static AgeCategory rating = AgeCategory.Undefined;

        private static string _returnToTitle = "Title";
        private static string _returnToDescription = "Description";
        private static string _returnToGenres = "Genres";
        private static string _returnToRating = "Rating";


        public static void Start(string returnTo = "")
        {
            Console.Clear();

            if(returnTo == "" || returnTo == _returnToTitle)
            {
                MovieName();
                returnTo = "";
            }

            if(returnTo == "" || returnTo == _returnToDescription)
            {
                MovieDescription();
                returnTo = "";
            }

            if(returnTo == "" || returnTo == _returnToGenres)
            {
                SelectMovieGenres();
                returnTo = "";
            }

            if(returnTo == "" || returnTo == _returnToRating)
            {
                SelectMovieRating();
                returnTo = "";
            }

            if (MoviesLogic.AddMovie(title, description, genres, rating))
            {
                Console.Clear();
                Print();
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", () => {Console.Clear(); AdminMenu.Start();}),
                };
                new SelectionMenuUtil2<string>(options, () => Start(_returnToRating), Print).Create();
            }
            else
            {
                Console.Clear();
                Print();
                Console.WriteLine("Er is een fout opgetreden tijdens het toevoegen van de film. Probeer het opnieuw.\n");
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", () => {Console.Clear(); AdminMenu.Start();}),
                };
                new SelectionMenuUtil2<string>(options).Create();
            }
        }

        private static void MovieName()
        {
            Console.Clear();
            PrintAddingMovie();
            
            title = ReadLineUtil.EnterValue(() =>
            {
                ColorConsole.WriteColorLine("Film Toevoegen\n", Globals.TitleColor);
                ColorConsole.WriteColor("Voer de film [titel] in: ", Globals.ColorInputcClarification);
            }, actionWhenEscapePressed);

            while (string.IsNullOrEmpty(title))
            {
                title = ReadLineUtil.EnterValue(() =>
                {
                    ColorConsole.WriteColorLine("Film Toevoegen\n", Globals.TitleColor);
                    ColorConsole.WriteColor("Voer de film [titel] in: ", Globals.ColorInputcClarification);
                }, actionWhenEscapePressed);
            }
        }

        private static void MovieDescription()
        {
            Console.Clear();
            PrintAddingMovie();

            description = ReadLineUtil.EnterValue(() =>
            {
                ColorConsole.WriteColorLine("Film Toevoegen\n", Globals.TitleColor);
                ColorConsole.WriteColor("Voer de film [beschrijving] in: ", Globals.ColorInputcClarification);
            }, () => Start(_returnToTitle));

            while (string.IsNullOrEmpty(description))
            {
                description = ReadLineUtil.EnterValue(() =>
                {
                    ColorConsole.WriteColorLine("Film Toevoegen\n", Globals.TitleColor);
                    ColorConsole.WriteColor("Voer de film [beschrijving] in: ", Globals.ColorInputcClarification);
                }, () => Start(_returnToTitle));
            }
        }

        private static void SelectMovieGenres()
        {
            List<Genre> availableGenres = Globals.GetAllEnum<Genre>();
            while (genres.Count < 3)
            {
                Genre genre;

                Console.Clear();
                PrintAddingMovie();
                ColorConsole.WriteColorLine("Kies film genres", Globals.TitleColor);
                ColorConsole.WriteColorLine("U kunt maximaal 3 verschillende genres kiezen.\n", Globals.TitleColor);
                ColorConsole.WriteColorLine("Kies een [genre]: \n", Globals.ColorInputcClarification);

                genre = new SelectionMenuUtil2<Genre>(availableGenres,() => Start(_returnToDescription), () => Start(_returnToGenres)).Create();

                if (genre != default && availableGenres.Contains(genre))
                {
                    availableGenres.Remove(genre);
                    genres.Add(genre);
                }
                else
                {
                    ColorConsole.WriteColorLine("Error. Probeer het opnieuw.", Globals.ErrorColor);
                }
            }
        }

        private static void SelectMovieRating()
        {
            Console.Clear();
            PrintAddingMovie();
            ColorConsole.WriteColorLine("Wat is uw [leeftijdscatagorie]: \n", Globals.ColorInputcClarification);

            List<AgeCategory> AgeGenres = Globals.GetAllEnum<AgeCategory>();
            List<string> EnumDescription = AgeGenres.Select(o => o.GetDisplayName()).ToList();
            string selectedDescription = new SelectionMenuUtil2<string>(EnumDescription, () => { genres = new List<Genre>(); Start(_returnToGenres); }, () => Start(_returnToRating)).Create();
            rating = AgeGenres.First(o => o.GetDisplayName() == selectedDescription);
        }

        private static void PrintAddingMovie()
        {
            if(title != "")
            {
                ColorConsole.WriteColorLine("[Huidige Film Details]", Globals.MovieColor);
                ColorConsole.WriteColorLine($"[Naam Film:] {title}", Globals.MovieColor);
            }
            if(description != "")
            {
                ColorConsole.WriteColorLine($"[Beschrijving Film:] {description}", Globals.MovieColor);
            }
            if(genres.Count >= 1)
            {
                ColorConsole.WriteColorLine($"[Genres Film:] {string.Join(", ", genres)}", Globals.MovieColor);
            }
            if(rating != AgeCategory.Undefined)
            {  
                ColorConsole.WriteColorLine($"[Beschrijving Film:] {rating.GetDisplayName()}", Globals.MovieColor);
            }
        }

        private static void Print()
        {
            ColorConsole.WriteColorLine("De film is toegevoegd!\n", Globals.MovieColor);
            ColorConsole.WriteColorLine("[Film details]", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film titel: ]{title}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film beschrijving: ]{description}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film genre(s): ]{string.Join(", ", genres)}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film kijkwijzer ]{rating.GetDisplayName()}\n", Globals.MovieColor);
        }
    }
}