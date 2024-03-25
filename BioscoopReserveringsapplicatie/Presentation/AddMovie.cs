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

        if (MoviesLogic.AddMovie(title, description, genre, rating))
        {
            Console.WriteLine("\nDe film is succesvol toegevoegd.");
            Console.WriteLine("\nDe details van de film zijn:");
            Console.WriteLine($"Film titel: {title}");
            Console.WriteLine($"Film beschrijving: {description}");
            Console.WriteLine($"Film genre: {genre}");
            Console.WriteLine($"Film beoordeling: {rating}");

        }
        else
        {
            Console.WriteLine("Er is een error opgetreden tijdens het toevoegen van de film.");
        }
    }
}