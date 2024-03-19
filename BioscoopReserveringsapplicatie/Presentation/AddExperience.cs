using System;
using Spectre.Console;

static class AddExperience
{
    private static ExperiencesLogic experiencesLogic = new ExperiencesLogic();
    public static void Start()
    {
        Console.Clear();
        AnsiConsole.Markup("[underline red]Add Experience[/]\n\n");
        var name = AnsiConsole.Ask<string>($"What is the [{Globals.ColorIputAskingValue}]name[/] of the experience?");
        var filmId = AskForFilm();
        var intensity = AnsiConsole.Ask<int>($"What is the [{Globals.ColorIputAskingValue}]intensity[/]?");
        var timeLength = AnsiConsole.Ask<int>($"What is the time [{Globals.ColorIputAskingValue}]length[/]? (in minutes)");
        ExperiencesModel newExperience = new ExperiencesModel(name, filmId, intensity, timeLength);
        experiencesLogic.addExperience(newExperience);
    }

    private static int AskForFilm()
    {
        return AnsiConsole.Ask<int>($"What is the [{Globals.ColorIputAskingValue}]film[/] id?");
    }
}

