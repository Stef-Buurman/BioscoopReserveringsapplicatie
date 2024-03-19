using System.Text.Json;
using Spectre.Console;

public class Program
{
    public static void Main(string[] args)
    {
        //AnsiConsole.Markup("[underline red]Hello[/] World!");
        //var calendar = new Calendar(2020, 10);
        //calendar.AddCalendarEvent(2020, 10, 11);
        //calendar.HighlightStyle(Style.Parse("yellow bold"));
        //AnsiConsole.Write(calendar);

        //AnsiConsole.Progress()
        //.Start(ctx =>
        //{
        //    // Define tasks
        //    var task1 = ctx.AddTask("[green]Reticulating splines[/]");
        //    var task2 = ctx.AddTask("[green]Folding space[/]");

        //    while (!ctx.IsFinished)
        //    {
        //        task1.Increment(1.5);
        //        task2.Increment(0.5);
        //    }
        //});
        //Console.WriteLine("Welcome to this amazing program");
        //Menu.Start();
        //Console.WriteLine(JsonSerializer.Serialize(ExperiencesAccess.LoadAll(), new JsonSerializerOptions { WriteIndented = true }));
        //ExperiencesLogic experiencesLogic = new ExperiencesLogic();
        //experiencesLogic.addExperience(new ExperiencesModel(0, "Test", 2, "Test", DateTime.Now, 5, 5));
        AddExperience.Start();
    }
}