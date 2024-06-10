namespace BioscoopReserveringsapplicatie
{
    static class ExperienceEdit
    {
        static public ExperienceLogic ExperiencesLogic = new ExperienceLogic();
        static public MovieLogic MoviesLogic = new MovieLogic();
        private static ExperienceModel _experience;
        private static string _newName = "";
        private static string _newDescription = "";
        private static int _selectedMovieId = 0;
        private static Intensity _newIntensity = Intensity.Undefined;
        private static int _timeInInt = 0;
        private static int _experienceId;

        public static void Start(int experienceId)
        {
            _experienceId = experienceId;
            _experience = ExperiencesLogic.GetById(experienceId);

            if (_experience == null) return;
            if (_newName == "") _newName = _experience.Name;
            if (_newDescription == "") _newDescription = _experience.Description;
            if (_selectedMovieId == 0) _selectedMovieId = _experience.FilmId;
            if (_newIntensity == Intensity.Undefined) _newIntensity = _experience.Intensity;
            if (_timeInInt == 0) _timeInInt = _experience.TimeLength;

            PrintEditedList();
            ColorConsole.WriteColorLine("\nWat wilt u aanpassen van deze experience?", Globals.TitleColor);

            List<Option<string>> editOptions = new List<Option<string>>()
            {
                new Option<string>("Naam", () => { ExperienceName(); }),
                new Option<string>("Beschrijving", () => { ExperienceDescription(); }),
                new Option<string>("Film", () => { SelectMovie(); }),
                new Option<string>("Intensiteit", () => { SelectIntensity(); }),
                new Option<string>("Tijdsduur", () => { ExperienceLength(); }),
                new Option<string>("Opslaan", () => { SaveExperience(); }, Globals.SaveColor),
                new Option<string>("Terug", () => { ReadLineUtil.EscapeKeyPressed(GoBackToDetails, () => Start(experienceId)); }, Globals.GoBackColor)
            };

            new SelectionMenuUtil<string>(editOptions, new Option<string>("Naam")).Create();
        }

        private static void SaveExperience()
        {
            string MovieName = MoviesLogic.GetById(_selectedMovieId).Title;
            Console.Clear();
            List<Option<string>> saveOptions = new List<Option<string>>()
            {
                new Option<string> ("Ja", () =>
                {
                    if (ExperiencesLogic.Edit(_experienceId, _newName, _newDescription, _newIntensity, _timeInInt, _selectedMovieId))
                    {
                        GoBackToDetails();
                    }
                    else
                    {
                        List<Option<string>> errorOptions = new List<Option<string>>()
                        {
                            new Option<string>("Terug", () => { GoBackToDetails(); })
                        };
                        ColorConsole.WriteColorLine("Error. Probeer het opnieuw \n",Globals.ErrorColor);
                        new SelectionMenuUtil<string>(errorOptions).Create();
                    }
                }),
                new Option<string>("Nee, pas de experience verder aan", () => { Start(_experience.Id); }),
                new Option<string>("Nee, stop met aanpassen", () => { GoBackToDetails(); })
            };
            ColorConsole.WriteColorLine("Dit zijn de nieuwe experience details:", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience naam:] {_newName}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience beschrijving:] {_newDescription}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Film gekoppeld aan experience:] {MovieName}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience intensiteit:] {_newIntensity}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience tijdsduur:] {_timeInInt} minuten", Globals.ExperienceColor);
            HorizontalLine.Print();
            ColorConsole.WriteColorLine($"Weet u zeker dat u de Experience details van {_newName} wilt bewerken?\n", Globals.ColorInputcClarification);

            new SelectionMenuUtil<string>(saveOptions, new Option<string>("Nee, pas de experience verder aan")).Create();
        }

        private static void ExperienceName()
        {
            PrintEditedList();
            _newName = ReadLineUtil.EditValue(_newName, "Voer de experience [naam] in: ", () => Start(_experienceId));
            while (string.IsNullOrEmpty(_newName))
            {
                PrintEditedList();
                ColorConsole.WriteColorLine("\nNaam mag niet leeg zijn.", Globals.ErrorColor);
                _newName = ReadLineUtil.EditValue(_newName, "Voer de experience [naam] in: ", () => Start(_experienceId));
            }
            Start(_experienceId);
        }

        private static void ExperienceDescription()
        {
            PrintEditedList();
            _newDescription = ReadLineUtil.EditValue(_newDescription, "Voer de experience [beschrijving] in: ", () => Start(_experienceId));
            while (string.IsNullOrEmpty(_newDescription))
            {
                PrintEditedList();
                ColorConsole.WriteColorLine("\nBeschrijving mag niet leeg zijn.", Globals.ErrorColor);
                _newDescription = ReadLineUtil.EditValue(_newDescription, "Voer de experience [beschrijving] in: ", () => Start(_experienceId));
            }
            Start(_experienceId);
        }

        private static void SelectMovie()
        {
            PrintEditedList();
            ColorConsole.WriteColorLine("Selecteer de [film] die bij de experience hoort\n", Globals.ColorInputcClarification);
            List<MovieModel> movies = MoviesLogic.GetAll();
            List<Option<int>> MovieOptions = new List<Option<int>>();
            foreach (MovieModel movie in movies)
            {
                MovieOptions.Add(new Option<int>(movie.Id, movie.Title));
            }
            int top = Console.GetCursorPosition().Top;
            _selectedMovieId = new SelectionMenuUtil<int>(MovieOptions,
                () => Start(_experienceId),
                () => SelectMovie(),
                new Option<int>(_selectedMovieId)).Create();
            while (!ExperiencesLogic.ValidateMovieId(_selectedMovieId))
            {
                Console.SetCursorPosition(0, top);
                ColorConsole.WriteColorLine("Ongeldige Input. Probeer het opnieuw.", Globals.ErrorColor);
                _selectedMovieId = new SelectionMenuUtil<int>(MovieOptions, () => Start(_experienceId), () => SelectMovie()).Create();
            }
            Start(_experienceId);
        }

        private static void SelectIntensity()
        {
            PrintEditedList();
            ColorConsole.WriteColorLine("Selecteer de [intensiteit] van de experience", Globals.ColorInputcClarification);
            List<Intensity> options = new List<Intensity> { Intensity.Low, Intensity.Medium, Intensity.High };
            int top = Console.GetCursorPosition().Top;
            _newIntensity = new SelectionMenuUtil<Intensity>(options,
                () => Start(_experienceId),
                () => SelectIntensity(),
                new Option<Intensity>(_newIntensity)).Create();
            while (!ExperiencesLogic.ValidateExperienceIntensity(_newIntensity))
            {
                Console.SetCursorPosition(0, top);
                ColorConsole.WriteColorLine("Ongeldige input. Probeer het opnieuw", Globals.ErrorColor);
                _newIntensity = new SelectionMenuUtil<Intensity>(options, () => Start(_experienceId), () => SelectIntensity()).Create();
            }
            Start(_experienceId);
        }

        private static void ExperienceLength()
        {
            PrintEditedList();
            List<int> intList = Enumerable.Range(1, 100).ToList();
            intList.Reverse();
            SelectionMenuUtil<int> selection = new SelectionMenuUtil<int>(intList, 1,
                () => Start(_experienceId),
                () => ExperienceLength(),
                false, "Voer de [lengte] van de experience in (in minuten): ",
                new Option<int>(_timeInInt));
            _timeInInt = selection.Create();
            while (!ExperiencesLogic.ValidateExperienceTimeLength(_timeInInt))
            {
                _timeInInt = selection.Create();
            }
            Start(_experienceId);
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
            if (_newDescription != "")
            {
                ColorConsole.WriteColorLine($"[Beschrijving experience:] {_newDescription}", Globals.ExperienceColor);
            }
            if (_selectedMovieId != 0)
            {
                ColorConsole.WriteColorLine($"[Film titel:] {MoviesLogic.GetById(_selectedMovieId).Title}", Globals.ExperienceColor);
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
                HorizontalLine.Print();
            }
        }

        private static void GoBackToDetails()
        {
            _newName = "";
            _newDescription = "";
            _selectedMovieId = 0;
            _newIntensity = Intensity.Undefined;
            _timeInInt = 0;
            Console.Clear();
            ExperienceDetails.Start(_experience.Id);
        }
    }
}