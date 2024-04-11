
namespace BioscoopReserveringsapplicatie
{
    static class ExperienceEdit
    {
        static public ExperiencesLogic ExperiencesLogic = new ExperiencesLogic();
        static public MoviesLogic MoviesLogic = new MoviesLogic();
        private static Action actionWhenEscapePressed = ExperienceOverview.Start;

        public static void Start(int experienceId)
        {
            actionWhenEscapePressed = () => ExperienceDetails.Start(experienceId);
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
                        SelectionMenuUtil.Create(errorOptions, () => ColorConsole.WriteColorLine("Error. Probeer het opnieuw \n",Globals.ErrorColor));
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
                ColorConsole.WriteColor("Voer de experience naam in: ", Globals.ColorInputcClarification);
            }, actionWhenEscapePressed);
            while (string.IsNullOrEmpty(newName))
            {
                ColorConsole.WriteColorLine("Naam mag niet leeg zijn.", Globals.ErrorColor);
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
                ColorConsole.WriteColorLine("Ongeldige Input. Probeer het opnieuw.", Globals.ErrorColor);
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
                ColorConsole.WriteColor("Ongeldige input. Probeer het opnieuw", Globals.ErrorColor);
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
                ColorConsole.WriteColor("Voer de lengte van de experience in (in minuten): ", Globals.ColorInputcClarification);
            }, actionWhenEscapePressed);
            while (!ExperiencesLogic.ValidateExperienceTimeLength(timeInString))
            {
                ColorConsole.WriteColorLine("Ongeldige invoer. Voer een geldig getal in.", Globals.ErrorColor);
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
            ColorConsole.WriteColorLine("Dit zijn de nieuwe experience details:",Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience naam:] {newName}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Film gekoppeld aan experience:] {selectedMovieTitle}",Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience intensiteit:] {newIntensity}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience tijdsduur:] {timeInInt} minuten\n", Globals.ExperienceColor);

            ColorConsole.WriteColorLine($"Weet u zeker dat u de aanpassingen op {newName} wilt [opslaan?]", Globals.ColorInputcClarification);
        }
    }
}