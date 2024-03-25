using System;

static class AddExperience
{
    private static ExperiencesLogic experiencesLogic = new ExperiencesLogic();
    private static MoviesLogic moviesLogic = new MoviesLogic();
    public static void Start()
    {
        Console.Clear();
        ColorConsole.WriteColorLine("[Experience Toevoegen]\n", Globals.TitleColor);
        ColorConsole.WriteColor($"Wat is de [naam] van de experience?: ", Globals.ColorInputcClarification);
        string name = Console.ReadLine() ?? "";
        int filmId = AskForMovie();
        ColorConsole.WriteColor($"Wat is de [intensiteit]?: ", Globals.ColorInputcClarification);
        string intensityStr = Console.ReadLine() ?? "";
        while (!int.TryParse(intensityStr, out int _) || (int.TryParse(intensityStr, out int intensitInt) && (intensitInt < 0 || intensitInt > 10)))
        {
            Console.WriteLine("Voer alstublieft een geldige intensiteit in!");
            ColorConsole.WriteColor($"Wat is de [intensiteit]?: ", Globals.ColorInputcClarification);
            intensityStr = Console.ReadLine() ?? "";
        }
        int intensityInt = Convert.ToInt32(intensityStr);
        ColorConsole.WriteColor($"Wat is de [lengte]? (in minuten): ", Globals.ColorInputcClarification);
        string timeLengthStr = Console.ReadLine() ?? "";
        while (!int.TryParse(timeLengthStr, out int _))
        {
            Console.WriteLine("Voer alstublieft een geldige tijd in!");
            ColorConsole.WriteColor($"Wat is de [lengte]? (minuten): ", Globals.ColorInputcClarification);
            timeLengthStr = Console.ReadLine() ?? "";
        }
        int timeLength = Convert.ToInt32(timeLengthStr);
        ExperiencesModel newExperience = new ExperiencesModel(0, name, filmId, intensityInt, timeLength);
        if (experiencesLogic.AddExperience(newExperience))
        {
            Console.Clear();
            ColorConsole.WriteColorLine("[De film is succesvol toegevoegd.]", ConsoleColor.Green);
            ColorConsole.WriteColorLine("\n[De details van de film zijn:]", Globals.TitleColor);
            Console.WriteLine($"Experience naam: {name}");
            Console.WriteLine($"Film: {moviesLogic.GetMovieById(filmId).Title}");
            Console.WriteLine($"Experience intensiteit: {intensityInt}");
            Console.WriteLine($"Experience lengte (minuten): {timeLength}");
        }
        else
        {
            Console.Clear();
            ColorConsole.WriteColorLine("[Er is een error opgetreden tijdens het toevoegen van de experience.]", ConsoleColor.Red);
        }
    }

    private static int AskForMovie()
    {
        List<MovieModel> movies = moviesLogic.GetAllMovies();
        List<Option<int>> movieOptions = new List<Option<int>>();
        foreach (MovieModel movie in movies)
        {
            movieOptions.Add(new Option<int>(movie.Id, movie.Title));
        }
        int movieId = SelectionMenu.Create(movieOptions, 10, PrintTitle);
        Console.Clear();
        return movieId;
    }

    private static void PrintTitle() => Console.WriteLine("Welke film wilt u toevoegen?");
}

