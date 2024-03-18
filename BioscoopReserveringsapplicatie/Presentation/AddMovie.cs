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

        MoviesLogic.AddMovie(title, description, genre, rating);
    }
}