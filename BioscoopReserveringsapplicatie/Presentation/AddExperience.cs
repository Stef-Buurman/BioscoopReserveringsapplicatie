using System;

static class AddExperience
{
    private static ExperiencesLogic experiencesLogic = new ExperiencesLogic();
    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("Add Experience");
        Console.WriteLine($"What is the name of the experience?");
        string name = Console.ReadLine() ?? "";
        int filmId = AskForFilm();
        Console.WriteLine($"What is the intensity?");
        string intensityStr = Console.ReadLine() ?? "";
        while (!int.TryParse(intensityStr, out int _))
        {
            Console.WriteLine("Please enter a valid film id!");
            intensityStr = Console.ReadLine() ?? "";
        }
        int intensityInt = Convert.ToInt32(intensityStr);
        Console.WriteLine($"What is the time length? (in minutes)");
        string timeLengthStr = Console.ReadLine() ?? "";
        while (!int.TryParse(timeLengthStr, out int _))
        {
            Console.WriteLine("Please enter a valid film id!");
            timeLengthStr = Console.ReadLine() ?? "";
        }
        int timeLength = Convert.ToInt32(timeLengthStr);
        ExperiencesModel newExperience = new ExperiencesModel(0,name, filmId, intensityInt, timeLength);
        if (experiencesLogic.AddExperience(newExperience))
        {
            Console.WriteLine("Top");
        }
    }

    private static int AskForFilm()
    {
        Console.WriteLine("What is the film id?");
        string filmIdString = Console.ReadLine() ?? "";
        while (!int.TryParse(filmIdString, out int _))
        {
            Console.WriteLine("Please enter a valid film id!");
            filmIdString = Console.ReadLine() ?? "";
        }
        return Convert.ToInt32(filmIdString);
    }
}

