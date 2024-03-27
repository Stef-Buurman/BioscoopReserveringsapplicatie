namespace BioscoopReserveringsapplicatie
{
    static class MovieEdit
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();

        public static void Start(int movieId)
        {
            MovieModel movie = MoviesLogic.GetMovieById(movieId);

            Console.WriteLine("Voer nieuwe filmdetails in (druk op Enter om de huidige te behouden):");

            Console.Write("Titel: ");
            string newTitle = EditDefaultValueUtil.EditDefaultValue(movie.Title);

            Console.Write("Beschrijving: ");
            string newDescription = EditDefaultValueUtil.EditDefaultValue(movie.Description);

            List<string> newGenres = new List<string>();
            List<string> availableGenres = new List<string> {
                "Horror", "Komedie", "Actie", "Drama", "Thriller", "Romantiek", "Sci-fi",
                "Fantasie", "Avontuur", "Animatie", "Misdaad", "Mysterie", "Familie",
                "Oorlog", "Geschiedenis", "Muziek", "Documentaire", "Westers", "TV-film"
            };
            Console.WriteLine(string.Join(", ", availableGenres) + "\n");

            for (int i = 0; i < 3; i++)
            {
                string genre = Console.ReadLine() ?? "";
                if (availableGenres.Contains(genre))
                {
                    newGenres.Add(genre);
                }
                else
                {
                    Console.WriteLine("Ongeldige genre, selecteer een genre uit de lijst.");
                    i--;
                }
            }

            Console.Write("Beoordeling: ");
            string newRating = EditDefaultValueUtil.EditDefaultValue(movie.Rating);

            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    if (MoviesLogic.EditMovie(movie.Id, newTitle, newDescription, newGenres, newRating))
                        {
                            Console.WriteLine("Filmdetails zijn bijgewerkt!");
                            MovieDetails.Start(movie.Id);
                        }
                        else
                        {
                            Console.WriteLine("Filmdetails konden niet worden bijgewerkt.");
                            MovieDetails.Start(movie.Id);
                        }
                }),
                new Option<string>("Nee", () => {Console.Clear(); MovieDetails.Start(movie.Id);}),
            };
            SelectionMenu.Create(options, () => Console.WriteLine($"Weet u zeker dat u de filmdetails van {movie.Title} wilt bewerken?"));
        }
    }
}