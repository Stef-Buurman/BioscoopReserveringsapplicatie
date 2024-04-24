
namespace BioscoopReserveringsapplicatie
{
    static class ExperienceEdit
    {
        static public ExperiencesLogic ExperiencesLogic = new ExperiencesLogic();
        static public MoviesLogic MoviesLogic = new MoviesLogic();
        private static Action actionWhenEscapePressed = ExperienceOverview.Start;
        private static ExperienceModel _experience;
        private static string _newName = "";
        private static int _selectedMovieId = 0;
        private static Intensity _newIntensity = Intensity.Undefined;
        private static int _timeInInt = 0;
        private static int _experienceId;

        private static string _returnToName = "Name";
        private static string _returnToMovie = "Movie";
        private static string _returnToIntensity = "Intensity";
        private static string _returnToLength = "Length";

        public static void Start(int experienceId, string returnTo = "")
        {
            actionWhenEscapePressed = () => ExperienceDetails.Start(experienceId);
            Console.Clear();
            _experienceId = experienceId;

            _experience = ExperiencesLogic.GetById(experienceId);

            if (_experience == null) return;
            if (_newName == "") _newName = _experience.Name;
            if (_selectedMovieId == 0) _selectedMovieId = _experience.FilmId;
            if (_newIntensity == Intensity.Undefined) _newIntensity = _experience.Intensity;
            if (_timeInInt == 0) _timeInInt = _experience.TimeLength;

            if (returnTo == "" || returnTo == _returnToName)
            {
                ExperienceName();
                returnTo = "";
            }
            if (returnTo == "" || returnTo == _returnToMovie)
            {
                SelectMovie();
                returnTo = "";
            }
            if (returnTo == "" || returnTo == _returnToIntensity)
            {
                SelectIntensity();
                returnTo = "";
            }
            if (returnTo == "" || returnTo == _returnToLength)
            {
                ExperienceLength();
                returnTo = "";
            }

            string MovieName = MoviesLogic.GetMovieById(_selectedMovieId).Title;
            Console.Clear();
            List<Option<string>> saveOptions = new List<Option<string>>()
            {
                new Option<string> ("Ja", () =>
                {
                    if (ExperiencesLogic.EditExperience(experienceId, _newName, _newIntensity, _timeInInt, _selectedMovieId))
                    {
                        ExperienceDetails.Start(experienceId);
                    }
                    else
                    {
                        List<Option<string>> errorOptions = new List<Option<string>>()
                        {
                            new Option<string>("Terug", () => { Console.Clear(); ExperienceDetails.Start(_experience.Id); })
                        };
                        ColorConsole.WriteColorLine("Error. Probeer het opnieuw \n",Globals.ErrorColor);
                        new SelectionMenuUtil2<string>(errorOptions).Create();
                    }
                }),
                new Option<string>("Nee, pas de experience verder aan", () => { Start(_experience.Id, _returnToLength); }),
                new Option<string>("Nee, stop met aanpassen", () => { Console.Clear(); ExperienceDetails.Start(_experience.Id); })
            };
            ColorConsole.WriteColorLine("Dit zijn de nieuwe experience details:", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience naam:] {_newName}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Film gekoppeld aan experience:] {MovieName}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience intensiteit:] {_newIntensity}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience tijdsduur:] {_timeInInt} minuten\n", Globals.ExperienceColor);
            ColorConsole.WriteColorLine("---------------------------------------------------------------", ConsoleColor.White);
            ColorConsole.WriteColorLine($"Weet u zeker dat u de aanpassingen op {_newName} wilt opslaan?\n", Globals.ColorInputcClarification);

            new SelectionMenuUtil2<string>(saveOptions).Create();
        }

        public static void ExperienceName()
        {
            PrintEditedList();

            _newName = ReadLineUtil.EditValue(_newName, () =>
            {
                PrintEditedList();
                ColorConsole.WriteColorLine("\nVoer nieuwe experience details in (druk op Enter om de huidige te behouden)", Globals.TitleColor);
                ColorConsole.WriteColor("Voer de experience [naam] in: ", Globals.ColorInputcClarification);
            }, actionWhenEscapePressed);
            while (string.IsNullOrEmpty(_newName))
            {
                _newName = ReadLineUtil.EditValue(_newName, () =>
                {
                    PrintEditedList();
                    ColorConsole.WriteColorLine("\nNaam mag niet leeg zijn.", Globals.ErrorColor);
                    ColorConsole.WriteColorLine("Voer nieuwe experience details in (druk op Enter om de huidige te behouden)", Globals.TitleColor);
                    ColorConsole.WriteColor("Voer de experience [naam] in: ", Globals.ColorInputcClarification);
                }, actionWhenEscapePressed);
            }
        }

        public static void SelectMovie()
        {
            PrintEditedList();
            ColorConsole.WriteColorLine("\nSelecteer de film die bij de experience hoort", Globals.TitleColor);
            List<MovieModel> movies = MoviesLogic.GetAllMovies();
            List<Option<int>> MovieOptions = new List<Option<int>>();
            foreach (MovieModel movie in movies)
            {
                MovieOptions.Add(new Option<int>(movie.Id, movie.Title));
            }
            ColorConsole.WriteColorLine("Kies uw [film]: ", Globals.ColorInputcClarification);
            int top = Console.GetCursorPosition().Top;
            _selectedMovieId = new SelectionMenuUtil2<int>(MovieOptions, 
                () => Start(_experienceId, _returnToName), 
                () => Start(_experienceId, _returnToMovie),
                new Option<int>(_selectedMovieId)).Create();
            while (!ExperiencesLogic.ValidateMovieId(_selectedMovieId))
            {
                Console.SetCursorPosition(0, top);
                ColorConsole.WriteColorLine("Ongeldige Input. Probeer het opnieuw.", Globals.ErrorColor);
                _selectedMovieId = new SelectionMenuUtil2<int>(MovieOptions, () => Start(_experienceId, _returnToName), () => Start(_experienceId, _returnToMovie)).Create();
            }
        }

        public static void SelectIntensity()
        {
            PrintEditedList();

            ColorConsole.WriteColorLine("\nSelecteer de intensiteit van de experience", Globals.TitleColor);
            List<Intensity> options = Globals.GetAllEnum<Intensity>();
            int top = Console.GetCursorPosition().Top;
            _newIntensity = new SelectionMenuUtil2<Intensity>(options, 
                () => Start(_experienceId, _returnToMovie), 
                () => Start(_experienceId, _returnToIntensity),
                new Option<Intensity>(_newIntensity)).Create();
            while (!ExperiencesLogic.ValidateExperienceIntensity(_newIntensity))
            {
                Console.SetCursorPosition(0, top);
                ColorConsole.WriteColorLine("Ongeldige input. Probeer het opnieuw", Globals.ErrorColor);
                _newIntensity = new SelectionMenuUtil2<Intensity>(options, () => Start(_experienceId, _returnToMovie), () => Start(_experienceId, _returnToIntensity)).Create();
            }
        }

        public static void ExperienceLength()
        {
            PrintEditedList();
            ColorConsole.WriteColorLine("\nVoer nieuwe experience details in (druk op Enter om de huidige te behouden)\n", Globals.TitleColor);
            List<int> intList = Enumerable.Range(1, 100).ToList();
            SelectionMenuUtil2<int> selection = new SelectionMenuUtil2<int>(intList, 1, 
                () => Start(_experienceId, _returnToMovie), 
                () => Start(_experienceId, _returnToLength), 
                false, "Voer de [lengte] van de experience in (in minuten): ",
                new Option<int>(_timeInInt));
            _timeInInt = selection.Create();
            while (!ExperiencesLogic.ValidateExperienceTimeLength(_timeInInt))
            {
                _timeInInt = selection.Create();
            }
        }

        private static void PrintEditedList()
        {
            Console.Clear();
            if (_newName != "" || _selectedMovieId != 0 || _newIntensity != Intensity.Undefined || _timeInInt != 0)
            {
                ColorConsole.WriteColorLine("[Huidige Experience Details]", Globals.ExperienceColor);
            }
            if (_newName != "")
            {
                ColorConsole.WriteColorLine($"[Naam experience:] {_newName}", Globals.ExperienceColor);
            }
            if (_selectedMovieId != 0)
            {
                ColorConsole.WriteColorLine($"[Film titel:] {MoviesLogic.GetMovieById(_selectedMovieId).Title}", Globals.ExperienceColor);
            }
            if (_newIntensity != Intensity.Undefined)
            {
                ColorConsole.WriteColorLine($"[Intensiteit experience:] {_newIntensity}", Globals.ExperienceColor);
            }
            if (_timeInInt != 0)
            {
                ColorConsole.WriteColorLine($"[Tijdsduur experience:] {_timeInInt} minuten", Globals.ExperienceColor);
            }
            if (_newName != "" || _selectedMovieId != 0 || _newIntensity != Intensity.Undefined || _timeInInt != 0)
            {
                ColorConsole.WriteColorLine("---------------------------------------------------------------", ConsoleColor.White);
            }
        }
    }
}