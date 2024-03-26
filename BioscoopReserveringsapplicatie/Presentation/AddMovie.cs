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
                Console.WriteLine("\nDe film is toegevoegd.");
                Console.WriteLine("\nDe film deetails zijn:");
                Console.WriteLine($"Film titel: {title}");
                Console.WriteLine($"Film beschrijving: {description}");
                Console.WriteLine($"Film genre: {string.Join(", ", genres)}");
                Console.WriteLine($"Film beoordeling: {rating}");

            }
            else
            {
                Console.WriteLine("Er is een error opgetreden bij het toevoegen van de film.");
            }
        }
    }
}