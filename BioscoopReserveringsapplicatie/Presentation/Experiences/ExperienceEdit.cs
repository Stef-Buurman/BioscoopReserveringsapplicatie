
namespace BioscoopReserveringsapplicatie
{
    static class ExperienceEdit
    {
        static public ExperiencesLogic ExperiencesLogic = new ExperiencesLogic();
        static public MoviesLogic MoviesLogic = new MoviesLogic();
        private static Action actionWhenEscapePressed = ExperienceOverview.Start;

        public static void Start(int experienceId)
        {
            Console.Clear();

            ExperiencesModel experience = ExperiencesLogic.GetById(experienceId);

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
                        SelectionMenuUtil.Create(errorOptions, () => Console.WriteLine("Error. Probeer het opnieuw \n"));
                    }
                }),
                new Option<string>("Nee", () => { Console.Clear(); ExperienceDetails.Start(experience.Id); })
            };
            SelectionMenuUtil.Create(saveOptions, () => Print(newName, MovieName, newIntensity, timeInInt));
        }

        public static string ExperienceName(ExperiencesModel experience)
        {
            Console.Clear();

            string newName = ReadLineUtil.EditValue(experience.Name, () =>
            {
                Console.WriteLine("Voer nieuwe experience details in (druk op Enter om de huidige te behouden)");
                Console.Write("Voer de experience naam in: ");
            }, actionWhenEscapePressed);
            while (string.IsNullOrEmpty(newName))
            {
                Console.WriteLine("Naam mag niet leeg zijn.");
                newName = ReadLineUtil.EditValue(newName, () =>
                {
                    Console.WriteLine("Voer nieuwe experience details in (druk op Enter om de huidige te behouden)");
                    Console.Write("Voer de experience naam in: ");
                }, actionWhenEscapePressed);
            }
            return newName;
        }

        public static int SelectMovie()
        {
            Console.WriteLine("Selecteer de film die bij de experience hoort");
            List<MovieModel> movies = MoviesLogic.GetAllMovies();
            List<int> moviesId = movies.Select(movie => movie.Id).ToList();
            int selectedMovieId = SelectionMenuUtil.Create(moviesId, () => ColorConsole.WriteColorLine("Kies uw [film]: \n", Globals.ColorInputcClarification), actionWhenEscapePressed);
            while (!ExperiencesLogic.ValidateMovieId(selectedMovieId))
            {
                Console.WriteLine("Ongeldige Input. Probeer het opnieuw.");
                selectedMovieId = SelectionMenuUtil.Create(moviesId, () => ColorConsole.WriteColorLine("Kies uw [film]: \n", Globals.ColorInputcClarification), actionWhenEscapePressed);
            }
            return selectedMovieId;
        }

        public static Intensity SelectIntensity()
        {
            Console.Write("Selecteer de intensiteit van de experience");
            List<Intensity> options = Globals.GetAllEnum<Intensity>();
            Intensity newIntensity = SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification), actionWhenEscapePressed);
            while (!ExperiencesLogic.ValidateExperienceIntensity(newIntensity))
            {
                Console.Write("Ongeldige input. Probeer het opnieuw");
                newIntensity = SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Kies uw [intensiteit]: \n", Globals.ColorInputcClarification), actionWhenEscapePressed);
            }
            return newIntensity;
        }

        public static int ExperienceLength(ExperiencesModel experience)
        {
            Console.Clear();

            string timeInString = ReadLineUtil.EditValue(experience.TimeLength.ToString(), () =>
            {
                Console.WriteLine("Voer nieuwe experience details in (druk op Enter om de huidige te behouden)");
                Console.Write("Voer de lengte van de experience in (in minuten): ");
            }, actionWhenEscapePressed);
            while (!ExperiencesLogic.ValidateExperienceTimeLength(timeInString))
            {
                Console.WriteLine("Ongeldige invoer. Voer een geldig getal in.");
                timeInString = ReadLineUtil.EditValue(experience.TimeLength.ToString(), () =>
                {
                    Console.WriteLine("Voer nieuwe experience details in (druk op Enter om de huidige te behouden)");
                    Console.Write("Voer de lengte van de experience in (in minuten): ");
                }, actionWhenEscapePressed);
            }
            return int.Parse(timeInString);
        }


        private static void Print(string newName, string selectedMovieTitle, Intensity newIntensity, int timeInInt)
        {
            Console.WriteLine("Dit zijn de nieuwe experience details:");
            Console.WriteLine($"Experience naam: {newName}");
            Console.WriteLine($"Film gekoppeld aan experience: {selectedMovieTitle}");
            Console.WriteLine($"Experience intensiteit: {newIntensity}");
            Console.WriteLine($"Experience tijdsduur: {timeInInt} minuten\n");

            Console.WriteLine($"Weet u zeker dat u de aanpassingen op {newName} wilt opslaan?");
        }
    }
}