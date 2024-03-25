namespace BioscoopReserveringsapplicatie
{
    static class AddMovie
    {
        static private MoviesLogic MoviesLogic = new MoviesLogic();

        public static void Start()
        {
            Console.WriteLine("Voer de filmtitel in:");
            string title = Console.ReadLine() ?? "";

            Console.WriteLine("Voer de film beschrijving in:");
            string description = Console.ReadLine() ?? "";

        Console.WriteLine("Voer de film genre in:");
        string genre = Console.ReadLine() ?? "";

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
                Console.WriteLine("Er is een error opgetreden tijdens het toevoegen van de film.");
            }
        }
    }
}