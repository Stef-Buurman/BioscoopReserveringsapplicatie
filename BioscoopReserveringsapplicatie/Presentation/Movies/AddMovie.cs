namespace BioscoopReserveringsapplicatie
{
    static class AddMovie
    {
        private static MovieLogic MoviesLogic = new MovieLogic();
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

            if (MoviesLogic.Add(new MovieModel(MoviesLogic.GetNextId(), title, description, genres, rating, false)))
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

            ColorConsole.WriteColorLine("Film Toevoegen\n", Globals.TitleColor);
            string question = "Voer de film [titel] in: ";
            //ColorConsole.WriteColor("Voer de film [titel] in: ", Globals.ColorInputcClarification);

            title = ReadLineUtil.EnterValue(question, actionWhenEscapePressed);

            while (string.IsNullOrEmpty(title))
            {
                title = ReadLineUtil.EnterValue(question, actionWhenEscapePressed);
            }
        }

        private static void MovieDescription()
        {
            Console.Clear();
            PrintAddingMovie();
            ColorConsole.WriteColorLine("Film Toevoegen\n", Globals.TitleColor);
            string question = "Voer de film [beschrijving] in: ";
            //ColorConsole.WriteColor("Voer de film [beschrijving] in: ", Globals.ColorInputcClarification);
            description = ReadLineUtil.EnterValue(question, () => Start(_returnToTitle));

            while (string.IsNullOrEmpty(description))
            {
                description = ReadLineUtil.EnterValue(question, () => Start(_returnToTitle));
            }
        }

        private static void SelectMovieGenres()
        {
            Console.Clear();
            PrintAddingMovie();
            List<Genre> Genres = Globals.GetAllEnum<Genre>();
            List<Option<Genre>> availableGenres = new List<Option<Genre>>();

            foreach (Genre option in Genres)
            {
                availableGenres.Add(new Option<Genre>(option, option.GetDisplayName()));
            }

            genres = new SelectionMenuUtil2<Genre>(availableGenres, 9,
                    () => { Start(_returnToDescription); },
                    () => { Start(_returnToGenres);},
                    "Welke [genre(s)] hoort/horen bij deze film: ").CreateMultiSelect();
        }

        private static void SelectMovieRating()
        {
            Console.Clear();
            PrintAddingMovie();
            ColorConsole.WriteColorLine("Wat is uw [leeftijdscatagorie]: \n", Globals.ColorInputcClarification);

            List<AgeCategory> AgeCatagories = Globals.GetAllEnum<AgeCategory>();
            List<Option<AgeCategory>> options = new List<Option<AgeCategory>>();
            foreach (AgeCategory option in AgeCatagories)
            {
                options.Add(new Option<AgeCategory>(option, option.GetDisplayName()));
            }

            rating = new SelectionMenuUtil2<AgeCategory>(AgeCatagories, () => { genres = new List<Genre>(); Start(_returnToGenres); }, () => Start(_returnToRating)).Create();
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
            if(genres != null && genres.Count >= 1)
            {
                ColorConsole.WriteColorLine($"[Genres Film:] {string.Join(", ", genres)}", Globals.MovieColor);
            }
            if(rating != AgeCategory.Undefined)
            {  
                ColorConsole.WriteColorLine($"[Beschrijving Film:] {rating.GetDisplayName()}", Globals.MovieColor);
            }
            if(title != "" || description != "" || (genres != null && genres.Count >= 1) || rating != AgeCategory.Undefined)
            {
                ColorConsole.WriteColorLine("---------------------------------------------------------------", ConsoleColor.White);
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