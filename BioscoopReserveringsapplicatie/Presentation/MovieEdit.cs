namespace BioscoopReserveringsapplicatie
{
    static class MovieEdit
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();

        public static void Start(int movieId)
        {
            MovieModel movie = MoviesLogic.GetMovieById(movieId);

            Console.WriteLine("Enter new movie details (press Enter to keep current):");

            Console.Write("Title: ");
            string newTitle = EditDefaultValueUtil.EditDefaultValue(movie.Title);

            Console.Write("Description: ");
            string newDescription = EditDefaultValueUtil.EditDefaultValue(movie.Description);

        List<string> newGenres = new List<string>();
        List<string> availableGenres = new List<string> { "Horror", "Comedy", "Action", "Drama", "Thriller", "Romance", "Sci-fi", "Fantasy", "Adventure", "Animation", "Crime", "Mystery", "Family", "War", "History", "Music", "Documentary", "Western", "TV Movie" };
        Console.WriteLine("Choose up to 3 genres from the following list:");
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
                Console.WriteLine("Invalid genre, please select from the list.");
                i--;
            }
        }

            Console.Write("Rating: ");
            string newRating = EditDefaultValueUtil.EditDefaultValue(movie.Rating);

            var options = new List<Option<string>>
            {
                new Option<string>("Edit movie", () => {
                    if (MoviesLogic.EditMovie(movie.Id, newTitle, newDescription, newGenres, newRating))
                        {
                            Console.WriteLine("Movie details updated successfully!");
                            MovieDetails.Start(movie.Id);
                        }
                        else
                        {
                            Console.WriteLine("Movie details could not be updated.");
                            MovieDetails.Start(movie.Id);
                        }
                }),
                new Option<string>("Cancel", () => {Console.Clear(); MovieDetails.Start(movie.Id);}),
            };
            SelectionMenu.Create(options, () => Console.WriteLine($"Are you sure you want to edit movie details of {movie.Title}?"));
        }
    }
}