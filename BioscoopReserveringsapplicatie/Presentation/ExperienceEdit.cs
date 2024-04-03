
namespace BioscoopReserveringsapplicatie
{
    static class ExperienceEdit
    {
        static public ExperiencesLogic ExperiencesLogic = new();
        static public MoviesLogic MoviesLogic = new MoviesLogic();

        public static void Start(int experienceId)
        {
            Console.Clear();

            ExperiencesModel experience = ExperiencesLogic.GetById(experienceId);

            Console.WriteLine("Voer nieuwe experience details in (druk op Enter om de huidige te behouden)");

            string newName = ExperienceName(experience);

            int selectedMovieId = SelectMovie();

            Intensity newIntensity = SelectIntensity();

            int timeInInt = ExperienceLength(experience);

            string MovieName = MoviesLogic.GetMovieById(selectedMovieId).Title;

            List<Option<string>> saveOptions = new List<Option<string>>()
            {
                new Option<string> ("Ja", () =>
                {
                    if (ExperiencesLogic.EditExperience(experienceId, newName, newIntensity, timeInInt, selectedMovieId))
                    {
                        ExperienceDetails.Start(experienceId);
                    }
                    else
                    {
                        List<Option<string>> errorOptions = new List<Option<string>>()
                        {
                            new Option<string>("Terug", () => { Console.Clear(); ExperienceDetails.Start(experience.Id); })
                        };
                        SelectionMenu.Create(errorOptions, () => Console.WriteLine("Error. Probeer het opnieuw \n"));
                    }
                }),
                new Option<string>("Nee", () => { Console.Clear(); ExperienceDetails.Start(experience.Id); })
            };
            SelectionMenu.Create(saveOptions, () => Print(newName, MovieName, newIntensity, timeInInt));
        }

        public static string ExperienceName(ExperiencesModel experience)
        {
        {
            Console.Clear();
            Console.Write("Voer de experience naam in: ");
            string newName = EditDefaultValueUtil.EditDefaultValue(experience.Name);
            while (string.IsNullOrEmpty(newName))
            {
                Console.WriteLine("Naam mag niet leeg zijn.");
                newName = EditDefaultValueUtil.EditDefaultValue(newName);
            }
            return newName;
            }
        }

        public static int SelectMovie()
        {
            Console.WriteLine("Selecteer de film die bij de experience hoort");
            List<MovieModel> movies = MoviesLogic.GetAllMovies();
            List<int> moviesId = movies.Select(movie => movie.Id).ToList();
            int selectedMovieId = SelectionMenu.Create(moviesId, () => ColorConsole.WriteColorLine("Kies uw [film]: \n", Globals.ColorInputcClarification));
            while (!ExperiencesLogic.ValidateMovieId(selectedMovieId))
            {
                Console.WriteLine("Ongeldige Input. Probeer het opnieuw.");
                selectedMovieId = SelectionMenu.Create(moviesId, () => ColorConsole.WriteColorLine("Kies uw [film]: \n", Globals.ColorInputcClarification));
            }
            return selectedMovieId;
        }

        public static Intensity SelectIntensity()
        {
            Console.Write("Selecteer de intensiteit van de experience");
            List<Intensity> options = Globals.GetAllEnum<Intensity>();
            Intensity newIntensity = SelectionMenu.Create(options, () => ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification));
            while (!ExperiencesLogic.ValidateExperienceIntensity(newIntensity))
            {
                Console.Write("Ongeldige input. Probeer het opnieuw");
                newIntensity = SelectionMenu.Create(options, () => ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification));
            }
            return newIntensity;
        }

        public static int ExperienceLength(ExperiencesModel experience)
        {
            Console.Clear();
            Console.Write("Voer de lengte van de experience in (in minuten)");
            string timeInString = EditDefaultValueUtil.EditDefaultValue(experience.TimeLength.ToString());
            while (!ExperiencesLogic.ValidateExperienceTimeLength(timeInString))
            {
                Console.WriteLine("Ongeldige invoer. Voer een geldig getal in.");
                timeInString = EditDefaultValueUtil.EditDefaultValue(experience.TimeLength.ToString());
            }
            return int.Parse(timeInString);
        }
            

        private static void Print(string newName, string selectedMovieTitle, Intensity newIntensity, int timeInInt)
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