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

            Console.Write("Genre: ");
            string newGenre = EditDefaultValueUtil.EditDefaultValue(movie.Genre);

            Console.Write("Rating: ");
            string newRating = EditDefaultValueUtil.EditDefaultValue(movie.Rating);

            var options = new List<Option<string>>
            {
                new Option<string>("Edit movie", () => {
                    if (MoviesLogic.EditMovie(movie.Id, newTitle, newDescription, newGenre, newRating))
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