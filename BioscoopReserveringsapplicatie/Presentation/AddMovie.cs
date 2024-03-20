static class AddMovie
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();

    public static void Start()
    {
        Console.WriteLine("Enter movie title:");
        string title = Console.ReadLine() ?? "";

        Console.WriteLine("Enter movie description:");
        string description = Console.ReadLine() ?? "";

        Console.WriteLine("Enter movie genre:");
        string genre = Console.ReadLine() ?? "";

        Console.WriteLine("Enter movie rating:");
        string rating = Console.ReadLine() ?? "";

        if (MoviesLogic.AddMovie(title, description, genre, rating))
        {
            Console.WriteLine("\nThe movie has been added succesfully.");
            Console.WriteLine("\nThe movie details are:");
            Console.WriteLine($"Movie title: {title}");
            Console.WriteLine($"Movie description: {description}");
            Console.WriteLine($"Movie genre: {genre}");
            Console.WriteLine($"Movie rating: {rating}");

        }
        else
        {
            Console.WriteLine("An error occurred while adding the movie.");
        }
    }
}