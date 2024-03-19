using Spectre.Console;

static class AddMovie
{
    static private MoviesLogic MoviesLogic = new MoviesLogic();

    public static void Start()
    {
        var title = AnsiConsole.Ask<string>("Enter movie [blue]title[/]:");

        var description = AnsiConsole.Ask<string>("Enter movie [blue]description[/]:");

        var genre = AnsiConsole.Ask<string>("Enter movie [blue]genre[/]:");

        var rating = AnsiConsole.Ask<string>("Enter movie [blue]rating[/]:");

        if (MoviesLogic.AddMovie(title, description, genre, rating))
        {
            Console.WriteLine("The movie has been added succesfully.");
        }
        else
        {
            Console.WriteLine("An error occurred while adding the movie.");
        }
    }
}