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
            ColorConsole.WriteColorLine("[Add Experience]\n", Globals.TitleColor);
            ColorConsole.WriteColor($"What is the [name] of the experience?: ", Globals.ColorInputcClarification);
            string name = Console.ReadLine() ?? "";

            int filmId = AskForMovie();

            ColorConsole.WriteColor($"What is the [intensity]?: ", Globals.ColorInputcClarification);
            string intensity = Console.ReadLine() ?? "";

            ColorConsole.WriteColor($"What is the [time length]? (in minutes): ", Globals.ColorInputcClarification);
            string timeLengthStr = Console.ReadLine() ?? "";
            while (!int.TryParse(timeLengthStr, out int _))
            {
                Console.WriteLine("Please enter a valid time!");
                ColorConsole.WriteColor($"What is the [time length]? (minutes): ", Globals.ColorInputcClarification);
                timeLengthStr = Console.ReadLine() ?? "";
            }
            int timeLength = Convert.ToInt32(timeLengthStr);
            ExperiencesModel newExperience = new ExperiencesModel(0, name, filmId, intensity, timeLength);
            if (experiencesLogic.AddExperience(newExperience))
            {
                Console.Clear();
                ColorConsole.WriteColorLine("[The experience has been added succesfully.]", ConsoleColor.Green);
                ColorConsole.WriteColorLine("\n[The experience details are:]", Globals.TitleColor);
                Console.WriteLine($"Experience name: {name}");
                Console.WriteLine($"Movie: {moviesLogic.GetMovieById(filmId).Title}");
                Console.WriteLine($"Experience intensity: {intensity}");
                Console.WriteLine($"Experience length (minutes): {timeLength}");
            }
            else
            {
                Console.Clear();
                ColorConsole.WriteColorLine("[An error occurred while adding the experience.]", ConsoleColor.Red);
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

        private static void PrintTitle() => Console.WriteLine("Which movie do you want to add?");
    }
}