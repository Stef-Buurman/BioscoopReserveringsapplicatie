using System;
namespace BioscoopReserveringsapplicatie
{
    static class AddExperience
    {
        private static ExperiencesLogic experiencesLogic = new ExperiencesLogic();
        private static MoviesLogic moviesLogic = new MoviesLogic();
        public static void Start()
        {
            Console.Clear();
            ColorConsole.WriteColorLine("[Experience Toevoegen]\n", Globals.TitleColor);
            string name = AskForExperienceName();
            int filmId = AskForMovie();
            int intensityInt = AskForExperienceIntensity();
            int timeLength = AskForExperienceTimeLength();

            ExperiencesModel newExperience = new ExperiencesModel(name, filmId, $"{intensityInt}", timeLength);
            if (experiencesLogic.AddExperience(newExperience))
            {
                Console.Clear();
                ColorConsole.WriteColorLine("[De experience is succesvol toegevoegd.]", ConsoleColor.Green);
                ColorConsole.WriteColorLine("\n[De details van de experience zijn:]", Globals.TitleColor);
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

        private static string AskForExperienceName()
        {
            ColorConsole.WriteColor($"Wat is de [naam] van de experience?: ", Globals.ColorInputcClarification);
            string name = Console.ReadLine() ?? "";
            while (!experiencesLogic.ValidateExperienceName(name))
            {
                Console.WriteLine("Voer alstublieft een geldige naam in!");
                ColorConsole.WriteColor($"Wat is de [naam] van de experience?: ", Globals.ColorInputcClarification);
                name = Console.ReadLine() ?? "";
            }
            return name;
        }

        private static int AskForExperienceIntensity()
        {
            ColorConsole.WriteColor($"Wat is de [intensiteit]? ", Globals.ColorInputcClarification);
            string intensityStr = Console.ReadLine() ?? "";
            while (!int.TryParse(intensityStr, out int _) || (int.TryParse(intensityStr, out int intensitInt) && (intensitInt < 0 || intensitInt > 10)))
            {
                Console.WriteLine("Voer alstublieft een geldige intensiteit in!");
                ColorConsole.WriteColor($"Wat is de [intensiteit]? ", Globals.ColorInputcClarification);
                intensityStr = Console.ReadLine() ?? "";
            }
            return Convert.ToInt32(intensityStr);
        }

        private static int AskForExperienceTimeLength()
        {
            ColorConsole.WriteColor($"Wat is de [tijdsduur]? (in minuten): ", Globals.ColorInputcClarification);
            string timeLengthStr = Console.ReadLine() ?? "";
            while (!experiencesLogic.ValidateExperienceTimeLength(timeLengthStr))
            {
                Console.WriteLine("Voer alstublieft een geldige tijdsduur in!");
                ColorConsole.WriteColor($"Wat is de [tijdsduur]? (in minuten): ", Globals.ColorInputcClarification);
                timeLengthStr = Console.ReadLine() ?? "";
            }
            return Convert.ToInt32(timeLengthStr);
        }   

        private static int AskForMovie()
        {
            List<MovieModel> movies = moviesLogic.GetAllMovies();
            List<Option<int>> movieOptions = new List<Option<int>>();
            foreach (MovieModel movie in movies)
            {
                movieOptions.Add(new Option<int>(movie.Id, movie.Title));
            }
            int movieId = SelectionMenu.Create(movieOptions, 10, () => Console.WriteLine("Welke film wilt u toevoegen?"));
            Console.Clear();
            return movieId;
        }
    }
}