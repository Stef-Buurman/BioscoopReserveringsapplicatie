using Spectre.Console;

public class Program
{
    public static void Main(string[] args)
    {
        AnsiConsole.Markup("[underline red]Hello[/] World!");
        var calendar = new Calendar(2020, 10);
        calendar.AddCalendarEvent(2020, 10, 11);
        calendar.HighlightStyle(Style.Parse("yellow bold"));
        AnsiConsole.Write(calendar);
    }
}