namespace BioscoopReserveringsapplicatie
{
    static class MovieEdit
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();

        public static void Start(int movieId)
        {
            Console.Clear();
            
            MovieModel movie = MoviesLogic.GetMovieById(movieId);

            Console.WriteLine("Voer nieuwe filmdetails in (druk op Enter om de huidige te behouden):");

            Console.Write("Voer de film titel in: ");
            string newTitle = EditDefaultValueUtil.EditDefaultValue(movie.Title);

            Console.Write("Voer de film beschrijving in: ");
            string newDescription = EditDefaultValueUtil.EditDefaultValue(movie.Description);

            List<Genre> genres = new List<Genre>();
            List<Genre> availableGenres = Globals.GetAllGenres();
            bool firstTime = true;
            while (genres.Count < 3)
            {
                Genre genre;
                if (firstTime)
                {
                    genre = SelectionMenu.Create(availableGenres, () =>
                    {
                        ColorConsole.WriteColorLine("[Voer film genre in]", Globals.TitleColor);
                        ColorConsole.WriteColorLine("[U kunt maximaal 3 verschillende genres kiezen.]\n", Globals.TitleColor);
                        ColorConsole.WriteColorLine("Kies een [genre]: \n", Globals.ColorInputcClarification);
                    }
                    );
                }
                else
                {
                    genre = SelectionMenu.Create(availableGenres, () => ColorConsole.WriteColorLine("Kies uw favoriete [genre]: \n", Globals.ColorInputcClarification));
                }

                if (genre != default && availableGenres.Contains(genre))
                {
                    availableGenres.Remove(genre);
                    genres.Add(genre);
                }
                else
                {
                    Console.WriteLine("Error. Probeer het opnieuw.");
                }
                firstTime = false;
            }

            Console.Write("Voer de film kijkwijzer in: ");
            string newRating = EditDefaultValueUtil.EditDefaultValue(movie.Rating);

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
                            SelectionMenu.Create(options, () => Console.WriteLine("Er is een fout opgetreden tijdens het bewerken van de film. Probeer het opnieuw.\n"));
                        }
                }),
                new Option<string>("Nee", () => {Console.Clear(); MovieDetails.Start(movie.Id);}),
            };
            SelectionMenu.Create(options, () => Print(movie.Title, newTitle, newDescription, genres, newRating));
        }

        private static void Print(string currentTitle, string newTitle, string description, List<Genre> genres, string rating)
        {
            Console.WriteLine("De nieuwe film details zijn:");
            Console.WriteLine($"Film titel: {newTitle}");
            Console.WriteLine($"Film beschrijving: {description}");
            Console.WriteLine($"Film genre(s): {string.Join(", ", genres)}");
            Console.WriteLine($"Film kijkwijzer: {rating}\n");

            Console.WriteLine($"Weet u zeker dat u de filmdetails van {currentTitle} wilt bewerken?");
        }
    }
}