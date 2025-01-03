namespace BioscoopReserveringsapplicatie
{
    static class AddMovie
    {
        private static MovieLogic MoviesLogic = new MovieLogic();
        private static Action actionWhenEscapePressed = MovieOverview.Start;

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
            ClearFields();

            if (returnTo == "" || returnTo == _returnToTitle)
            {
                MovieName();
                returnTo = "";
            }

            if (returnTo == "" || returnTo == _returnToDescription)
            {
                MovieDescription();
                returnTo = "";
            }

            if (returnTo == "" || returnTo == _returnToGenres)
            {
                SelectMovieGenres();
                returnTo = "";
            }

            if (returnTo == "" || returnTo == _returnToRating)
            {
                SelectMovieRating();
                returnTo = "";
            }
            Print();
            List<Option<string>> options = new List<Option<string>>
            {
            new Option<string>("Opslaan en terug naar overzicht", () => 
            {
                if (MoviesLogic.Add(new MovieModel(MoviesLogic.GetNextId(), title, description, genres, rating, Status.Active)))
                {
                    ClearFields();
                    
                    ColorConsole.WriteColorLine("\nFilm is toegevoegd!\n", Globals.SuccessColor);

                    Thread.Sleep(1500);

                    MovieOverview.Start();
                }
                else
                {
                    Console.Clear();
                    ColorConsole.WriteColorLine("Er is een fout opgetreden tijdens het toevoegen van de film. Probeer het opnieuw.\n", Globals.ErrorColor);
                    Start(_returnToRating);
                }
            }),
                new Option<string>("Verder gaan met aanpassen", () => { Start(_returnToTitle); }),
                new Option<string>("Verlaten zonder op te slaan", () => {ClearFields(); MovieOverview.Start(); }),
            };

            new SelectionMenuUtil<string>(options).Create();
        }

        private static void MovieName()
        {
            PrintAddingMovie();
            string question = "Voer de film [titel] in: ";

            title = ReadLineUtil.EditValue(title, question, actionWhenEscapePressed);
            while (string.IsNullOrEmpty(title))
            {
                PrintAddingMovie();
                ColorConsole.WriteColorLine("Voer alstublieft een geldige naam in!", Globals.ErrorColor);
                title = ReadLineUtil.EditValue(title, question, actionWhenEscapePressed);
            }
        }

        private static void MovieDescription()
        {
            PrintAddingMovie();
            string question = "Voer de film [beschrijving] in: ";
            description = ReadLineUtil.EditValue(description, question, () => Start(_returnToTitle));

            while (string.IsNullOrEmpty(description))
            {
                PrintAddingMovie();
                ColorConsole.WriteColorLine("Voer alstublieft een geldige beschrijving in!", Globals.ErrorColor);
                description = ReadLineUtil.EditValue(description, question, () => Start(_returnToTitle));
            }
        }

        private static void SelectMovieGenres()
        {
            PrintAddingMovie();
            List<Genre> Genres = Globals.GetAllEnum<Genre>();
            List<Option<Genre>> availableGenres = new List<Option<Genre>>();

            foreach (Genre option in Genres)
            {
                availableGenres.Add(new Option<Genre>(option, option.GetDisplayName()));
            }

            genres = new SelectionMenuUtil<Genre>(availableGenres, 9,
                    () => { Start(_returnToDescription); },
                    () => { Start(_returnToGenres); },
                    "Welke [genre(s)] hoort/horen bij deze film: ").CreateMultiSelect(out List<Option<Genre>> selectedGenres);
        }

        private static void SelectMovieRating()
        {
            PrintAddingMovie();
            ColorConsole.WriteColorLine("Wat is uw [leeftijdscategorie]: \n", Globals.ColorInputcClarification);

            List<AgeCategory> AgeCatagories = Globals.GetAllEnum<AgeCategory>();
            List<Option<AgeCategory>> options = new List<Option<AgeCategory>>();
            foreach (AgeCategory option in AgeCatagories)
            {
                options.Add(new Option<AgeCategory>(option, option.GetDisplayName()));
            }

            rating = new SelectionMenuUtil<AgeCategory>(options, 
                () => { genres = new List<Genre>(); Start(_returnToGenres); }, 
                () => Start(_returnToRating), 
                new Option<AgeCategory>(rating)).Create();
        }

        private static void PrintAddingMovie()
        {
            Console.Clear();
            if (title != "")
            {
                ColorConsole.WriteColorLine("[Huidige Film Details]", Globals.MovieColor);
                ColorConsole.WriteColorLine($"[Naam Film:] {title}", Globals.MovieColor);
            }
            if (description != "")
            {
                ColorConsole.WriteColorLine($"[Beschrijving Film:] {description}", Globals.MovieColor);
            }
            if (genres != null && genres.Count >= 1)
            {
                ColorConsole.WriteColorLine($"[Genres Film:] {string.Join(", ", genres)}", Globals.MovieColor);
            }
            if (rating != AgeCategory.Undefined)
            {
                ColorConsole.WriteColorLine($"[Beschrijving Film:] {rating.GetDisplayName()}", Globals.MovieColor);
            }
            if (title != "" || description != "" || (genres != null && genres.Count >= 1) || rating != AgeCategory.Undefined)
            {
                HorizontalLine.Print();
            }
            ColorConsole.WriteColorLine("Film Toevoegen\n", Globals.TitleColor);
        }

        private static void Print()
        {
            Console.Clear();
            ColorConsole.WriteColorLine("[Film details]", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film titel: ]{title}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film beschrijving: ]{description}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film genre(s): ]{string.Join(", ", genres)}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film leeftijdscategorie ]{rating.GetDisplayName()}\n", Globals.MovieColor);
            HorizontalLine.Print();
            ColorConsole.WriteColorLine($"Wilt u deze [Film] toevoegen?", Globals.ColorInputcClarification);
        }
        
        public static void ClearFields()
        {
            title = "";
            description = "";
            genres = new List<Genre>();
            rating = AgeCategory.Undefined;
        }
    }
}