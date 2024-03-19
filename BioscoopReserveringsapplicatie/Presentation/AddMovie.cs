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

            var table = new Table();
            table.Border(TableBorder.Rounded);
            table.Width(50);

            table.AddColumn("Field");
            table.AddColumn("Value");

            table.AddRow("Title", title);
            table.AddRow("Description", description);
            table.AddRow("Genre", genre);
            table.AddRow("Rating", rating);

            AnsiConsole.Write(table);
        }
        else
        {
            Console.WriteLine("An error occurred while adding the movie.");
        }
    }
}