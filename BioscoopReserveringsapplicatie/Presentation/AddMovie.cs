namespace BioscoopReserveringsapplicatie
{
    static class AddMovie
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();

        public static void Start()
        {
            Console.WriteLine("Voer de film titel in:");
            string title = Console.ReadLine() ?? "";

            Console.WriteLine("Voer de film beschrijving in:");
            string description = Console.ReadLine() ?? "";

            Console.WriteLine("Voer de film genre in:");
            List<string> genres = new List<string>();
            List<string> availableGenres = new List<string> { "Horror", "Comedy", "Action", "Drama", "Thriller", "Romance", "Sci-fi", "Fantasy", "Adventure", "Animation", "Crime", "Mystery", "Family", "War", "History", "Music", "Documentary", "Western", "TV Movie" };
            Console.WriteLine("Kies maximaal 3 genres uit de volgende lijst:");
            Console.WriteLine(string.Join(", ", availableGenres) + "\n");

            for (int i = 0; i < 3; i++)
            {
                string genre = Console.ReadLine() ?? "";
                if (availableGenres.Contains(genre))
                {
                    genres.Add(genre);
                }
                else
                {
                    Console.WriteLine("Ongeldige genre, selecteer een genre uit de lijst.");
                    i--;
                }
            }

            Console.WriteLine("Voer de film beoordeling in:");
            string rating = Console.ReadLine() ?? "";

            if (MoviesLogic.AddMovie(title, description, genres, rating))
            {
                Console.WriteLine("\nThe movie has been added succesfully.");
                Console.WriteLine("\nThe movie details are:");
                Console.WriteLine($"Movie title: {title}");
                Console.WriteLine($"Movie description: {description}");
                Console.WriteLine($"Movie genre: {string.Join(", ", genres)}");
                Console.WriteLine($"Movie rating: {rating}");

            }
            else
            {
                Console.WriteLine("An error occurred while adding the movie.");
            }
        }
    }
}