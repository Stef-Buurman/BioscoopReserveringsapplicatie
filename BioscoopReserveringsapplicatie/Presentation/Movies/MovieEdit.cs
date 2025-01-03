namespace BioscoopReserveringsapplicatie
{
    static class MovieEdit
    {
        private static MovieLogic MoviesLogic = new MovieLogic();
        private static MovieModel? movie = null;

        private static string newTitle = "";
        private static string newDescription = "";
        private static List<Genre> newGenres = new List<Genre>();
        private static AgeCategory newRating = AgeCategory.Undefined;
        private static List<Option<Genre>> selectedGenresInMenu = new List<Option<Genre>>();

        public static void Start(int movieId)
        {
            movie = MoviesLogic.GetById(movieId);
            if (movie == null) return;
            if (newTitle == "") newTitle = movie.Title;
            if (newDescription == "") newDescription = movie.Description;
            if (newGenres.Count == 0) newGenres = movie.Genres;
            if (newRating == AgeCategory.Undefined) newRating = movie.AgeCategory;

            PrintEditingMovie();
            ColorConsole.WriteColorLine("\nWat wilt u aanpassen van deze film?", Globals.TitleColor);

            List<Option<string>> editOptions = new List<Option<string>>()
            {
                new Option<string>("Naam", () => { MovieName(); }),
                new Option<string>("Beschrijving", () => { MovieDescription(); }),
                new Option<string>("Genres", () => {
                    selectedGenresInMenu = newGenres.ConvertAll(x => new Option<Genre>(x, x.GetDisplayName()));
                    SelectMovieGenres(); 
                }),
                new Option<string>("Leeftijdscatagorie", () => { SelectMovieRating(); }),
                new Option<string>("Opslaan", () => { SaveMovie(); }, Globals.SaveColor),
                new Option<string>("Terug", () => { ReadLineUtil.EscapeKeyPressed(GoBackToDetails, () => Start(movieId)); }, Globals.GoBackColor)
            };

            new SelectionMenuUtil<string>(editOptions, new Option<string>("Naam")).Create();
        }

        private static void SaveMovie()
        {
            Console.Clear();
            Print(movie.Title, newTitle, newDescription, newGenres, newRating);

            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    if (MoviesLogic.Edit(new MovieModel(movie.Id, newTitle, newDescription, newGenres, newRating)))
                        {
                            ColorConsole.WriteColorLine("\nFilm is aangepast!\n", Globals.SuccessColor);

                            Thread.Sleep(1500);
                            GoBackToDetails();
                        }
                        else
                        {
                            List<Option<string>> options = new List<Option<string>>
                            {
                                new Option<string>("Terug", () => GoBackToDetails()),
                            };
                            Console.WriteLine("Er is een fout opgetreden tijdens het bewerken van de film. Probeer het opnieuw.\n");
                            new SelectionMenuUtil<string>(options).Create();
                        }
                }),
                new Option<string>("Nee, pas de film verder aan", () => {Start(movie.Id);}),
                new Option<string>("Nee, stop met aanpassen", () => GoBackToDetails())
            };
            new SelectionMenuUtil<string>(options, new Option<string>("Nee, pas de film verder aan")).Create();
        }

        private static void MovieName()
        {
            Console.Clear();
            PrintEditingMovie();
            ColorConsole.WriteColorLine("Voer nieuwe filmdetails in:\n", Globals.TitleColor);
            string question = "Voer de film [titel] in: ";
            newTitle = ReadLineUtil.EditValue(newTitle, question,
            () => Start(movie.Id),
            "(druk op [Enter] om de huidige waarde te behouden en op [Esc] om terug te gaan)\n");

            while (string.IsNullOrEmpty(newTitle))
            {
                newTitle = ReadLineUtil.EditValue(newTitle, question,
                () => Start(movie.Id),
                "(druk op [Enter] om de huidige waarde te behouden en op [Esc] om terug te gaan)\n");
                }
            Start(movie.Id);
        }

        private static void MovieDescription()
        {
            Console.Clear();
            PrintEditingMovie();

            ColorConsole.WriteColorLine("Voer nieuwe filmdetails in:\n", Globals.TitleColor);
            string question = "Voer de film [beschrijving] in: ";
            newDescription = ReadLineUtil.EditValue(newDescription, question, 
            () => Start(movie.Id),
            "(druk op [Enter] om de huidige waarde te behouden en op [Esc] om terug te gaan)\n");

            while (string.IsNullOrEmpty(newDescription))
            {
                newDescription = ReadLineUtil.EditValue(newDescription, question, 
                () => Start(movie.Id),
                "(druk op [Enter] om de huidige waarde te behouden en op [Esc] om terug te gaan)\n");
            }
            Start(movie.Id);
        }

        private static void SelectMovieGenres()
        {
            Console.Clear();
            PrintEditingMovie();
            List<Genre> Genres = Globals.GetAllEnum<Genre>();
            List<Option<Genre>> availableGenres = new List<Option<Genre>>();
            List<Option<Genre>> selectedGenres = new List<Option<Genre>>();

            foreach (Genre option in Genres)
            {
                if (selectedGenresInMenu.ConvertAll(x => x.Value).Contains(option))
                {
                    selectedGenres.Add(new Option<Genre>(option, option.GetDisplayName()));
                }
                availableGenres.Add(new Option<Genre>(option, option.GetDisplayName()));
            }

            newGenres = new SelectionMenuUtil<Genre>(availableGenres, 9,
                    () => { Start(movie.Id); },
                    () => { SelectMovieGenres(); }, 
                    "Welke [genre(s)] hoort/horen bij deze film: ", selectedGenres).CreateMultiSelect(out selectedGenresInMenu);
            Start(movie.Id);
        }

        private static void SelectMovieRating()
        {
            Console.Clear();
            PrintEditingMovie();
            ColorConsole.WriteColorLine("Wat is uw [leeftijdscategorie]: \n", Globals.ColorInputcClarification);

            List<AgeCategory> AgeCatagories = Globals.GetAllEnum<AgeCategory>();
            List<Option<AgeCategory>> options = new List<Option<AgeCategory>>();
            foreach (AgeCategory option in AgeCatagories)
            {
                options.Add(new Option<AgeCategory>(option, option.GetDisplayName()));
            }
            newRating = new SelectionMenuUtil<AgeCategory>(options, 
                () => Start(movie.Id), 
                () => SelectMovieRating(), 
                new Option<AgeCategory>(newRating)).Create();
            Start(movie.Id);
        }

        private static void PrintEditingMovie()
        {
            Console.Clear();
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
                ColorConsole.WriteColorLine($"[Leeftijdscatagorie Film:] {newRating.GetDisplayName()}", Globals.MovieColor);
            }
            if (newTitle != "" || newDescription != "" || newGenres.Count >= 1 || newRating != AgeCategory.Undefined)
            {
                HorizontalLine.Print();
            }
        }

        private static void Print(string currentTitle, string newTitle, string description, List<Genre> genres, AgeCategory rating)
        {
            ColorConsole.WriteColorLine("[De nieuwe film details:]", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film titel: ]{newTitle}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film beschrijving: ]{description}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film genre(s): ]{string.Join(", ", genres)}", Globals.MovieColor);
            ColorConsole.WriteColorLine($"[Film leeftijdscategorie ]{rating.GetDisplayName()}", Globals.MovieColor);
            HorizontalLine.Print();
            ColorConsole.WriteColorLine($"Weet u zeker dat u de filmdetails van {currentTitle} wilt bewerken?\n", Globals.ColorInputcClarification);
        }

        private static void GoBackToDetails()
        {
            newTitle = "";
            newDescription = "";
            newGenres.Clear();
            newRating = AgeCategory.Undefined;
            selectedGenresInMenu.Clear();
            Console.Clear();
            MovieDetails.Start(movie.Id);
        }
    }
}