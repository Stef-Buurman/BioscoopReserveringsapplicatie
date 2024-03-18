static class AddMovie
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();

    public static void Start()
    {
        Console.WriteLine("Let's add a movie");

        Console.WriteLine("Enter title");

        string title = Console.ReadLine() ?? "";

        Console.WriteLine("Enter description");

        string description = Console.ReadLine() ?? "";

        Console.WriteLine("Enter genre(s)");

        string genre = Console.ReadLine() ?? "";

        Console.WriteLine("Enter rating(s)");

        string rating = Console.ReadLine() ?? "";

        MoviesLogic.AddMovie(title, description, genre, rating);
    }
}