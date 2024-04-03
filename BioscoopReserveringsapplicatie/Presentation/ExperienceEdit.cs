namespace BioscoopReserveringsapplicatie
{
    static class ExperienceEdit
    {
        static public ExperiencesLogic ExperiencesLogic = new();
        static public MoviesLogic MoviesLogic = new();

        public static void Start(int experienceId)
        {
            Console.Clear();

            ExperiencesModel experience = ExperiencesLogic.GetById(experienceId);

            Console.WriteLine("Voer nieuwe experience details in (druk op Enter om de huidige te behouden)");

            string newName = ExperienceName(experience);

            int selectedMovieId = SelectMovie();

            string newIntensity = SelectIntensity();

            int timeInInt = ExperienceLength(experience);

            string MovieName = MoviesLogic.GetMovieById(selectedMovieId).Title;


            List<Option<string>> saveOptions = new()
            {
                new ("Ja", () =>
                {
                    if (ExperiencesLogic.EditExperience(experienceId, newName, newIntensity, timeInInt, selectedMovieId))
                    {
                        ExperienceDetails.Start(experienceId);
                    }
                    else
                    {
                        List<Option<string>> errorOptions = new()
                        {
                            new("Back", () => { Console.Clear(); ExperienceDetails.Start(experience.Id); })
                        };
                        SelectionMenu.Create(errorOptions, () => Console.WriteLine("An error occurred while editing the experience. Please try again.\n"));
                    }
                }),
                new("No", () => { Console.Clear(); ExperienceDetails.Start(experience.Id); })
            };
            SelectionMenu.Create(saveOptions, () => Print(newName, MovieName, newIntensity, timeInInt));
        }

        public static string ExperienceName(ExperiencesModel experience)
        {
            while (true)
            {
                Console.Write("Voer de experience naam in: ");
                string newName = EditDefaultValueUtil.EditDefaultValue(experience.Name);
                if (!string.IsNullOrEmpty(newName))
                {
                    return newName;
                }
                Console.WriteLine("Invalid input. Please enter a valid name.");
            }
        }

        public static int SelectMovie()
        {
            while (true)
            {
                Console.WriteLine("Selecteer de film die bij de experience hoort");
                List<MovieModel> movies = MoviesLogic.GetAllMovies();
                Dictionary<string, int> movieDictionary = new();
                foreach (MovieModel movie in movies)
                {
                    movieDictionary.Add(movie.Title, movie.Id);
                }
                string selectedMovieTitle = SelectionMenu.Create(movieDictionary.Keys.ToList(), () => ColorConsole.WriteColorLine("Kies uw [film]: \n", Globals.ColorInputcClarification));
                if (movieDictionary.ContainsKey(selectedMovieTitle))
                {
                    return movieDictionary[selectedMovieTitle];
                }
                Console.WriteLine("Invalid input. Please select a valid movie.");
            }
        }

        public static string SelectIntensity()
        {
            while (true)
            {
                Console.Write("Selecteer de intensiteit van de experience");
                List<string> options = new() { "Laag", "Medium", "Hoog" };
                string newIntensity = SelectionMenu.Create(options, () => ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification));
                if (options.Contains(newIntensity))
                {
                    return newIntensity;
                }
                Console.WriteLine("Invalid input. Please select a valid intensity.");
            }
        }

        public static int ExperienceLength(ExperiencesModel experience)
        {
            while (true)
            {
                Console.Clear();
                Console.Write("Voer de lengte van de experience in (in minuten)");
                string timeInStr = EditDefaultValueUtil.EditDefaultValue(experience.TimeLength.ToString());
                if (int.TryParse(timeInStr, out int timeInInt))
                {
                    return timeInInt;
                }
                Console.WriteLine("Invalid input. Please enter a valid number.");
            }
        }

        public static void Print(string newName, string selectedMovieTitle, string newIntensity, int timeInInt)
        {
            Console.WriteLine("Dit zijn de nieuwe experience details:");
            Console.WriteLine($"Experience naam: {newName}");
            Console.WriteLine($"Film gekoppeld aan experience: {selectedMovieTitle}");
            Console.WriteLine($"Experience intensiteit: {newIntensity}");
            Console.WriteLine($"Experience tijdsduur: {timeInInt} minutes\n");

            Console.WriteLine($"Weet u zeker dat u de aanpassingen op {newName} wilt opslaan?");
        }
    }
}