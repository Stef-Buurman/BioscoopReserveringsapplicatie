namespace BioscoopReserveringsapplicatie
{
    static class AddExperience
    {
        private static ExperienceLogic experiencesLogic = new ExperienceLogic();
        private static MovieLogic MoviesLogic = new MovieLogic();
        private static string _newName = "";
        private static string _newDescription = "";
        private static int _selectedMovieId = 0;
        private static Intensity _Intensity = Intensity.Undefined;
        private static int _timeInInt = 0;

        private static string _returnToName = "Name";
        private static string _returnToDescription = "Description";
        private static string _returnToMovie = "Movie";
        private static string _returnToIntensity = "Intensity";
        private static string _returnToLength = "Length";
        public static void Start(string returnTo = "")
        {
            Console.Clear();
            if (returnTo == "" || returnTo == _returnToName)
            {
                AskForExperienceName();
                returnTo = "";
            }
            if (returnTo == "" || returnTo == _returnToDescription)
            {
                AskForExperienceDescription();
                returnTo = "";
            }
            if (returnTo == "" || returnTo == _returnToMovie)
            {
                AskForMovie();
                returnTo = "";
            }
            if (returnTo == "" || returnTo == _returnToIntensity)
            {
                AskForExperienceIntensity();
                returnTo = "";
            }
            if (returnTo == "" || returnTo == _returnToLength)
            {
                AskForExperienceTimeLength();
                returnTo = "";
            }
            Print(_newName, _newDescription, _selectedMovieId, _Intensity, _timeInInt);
            ExperienceModel newExperience = new ExperienceModel(_newName, _newDescription, _selectedMovieId, _Intensity, _timeInInt,  Status.Active);
            List<Option<string>> options = new List<Option<string>>
            {
            new Option<string>("Opslaan en verlaten", () => 
            {
                if (experiencesLogic.Add(newExperience))
                {
                    ExperienceOverview.Start();
                }
                else
                {
                    Console.Clear();
                    ColorConsole.WriteColorLine("Er is een fout opgetreden tijdens het toevoegen van de Experience . Probeer het opnieuw.\n", Globals.ErrorColor);
                    Start(_returnToLength);
                }
            }),
            new Option<string>("Verder gaan met aanpassen", () => { Start(_returnToLength); }),
            new Option<string>("Verlaten zonder op te slaan", () => { ExperienceOverview.Start(); }),
            };

            new SelectionMenuUtil<string>(options).Create();
        }

        private static void WriteTitle() => ColorConsole.WriteColorLine("Experience Toevoegen\n", Globals.TitleColor);

        private static void WhatToDoWhenGoBack() => ExperienceOverview.Start();

        private static void AskForExperienceName()
        {
            PrintEditedList();
            WriteTitle();

            _newName = ReadLineUtil.EditValue(_newName, "Wat is de [naam] van de experience?: ", WhatToDoWhenGoBack);

            while (!experiencesLogic.ValidateExperienceName(_newName))
            {
                Console.Clear();
                WriteTitle();
                ColorConsole.WriteColorLine("Voer alstublieft een geldige naam in!", Globals.ErrorColor);
                _newName = ReadLineUtil.EditValue(_newName, "Wat is de [naam] van de experience?: ", WhatToDoWhenGoBack);
            }
        }

        private static void AskForExperienceDescription()
        {
            PrintEditedList();
            WriteTitle();
            _newDescription = ReadLineUtil.EditValue(_newDescription, "Wat is de [beschrijving] van de experience?: ", () => Start(_returnToName));

            while (!experiencesLogic.ValidateExperienceDescription(_newDescription))
            {
                Console.Clear();
                WriteTitle();
                ColorConsole.WriteColorLine("Voer alstublieft een geldige beschrijving in!", Globals.ErrorColor);
                _newDescription = ReadLineUtil.EditValue(_newDescription, "Wat is de [beschrijving] van de experience?: ", () => Start(_returnToName));
            }
        }

        private static void AskForExperienceIntensity()
        {
            PrintEditedList();
            List<Intensity> intensityenum = Globals.GetAllEnum<Intensity>();
            List<Option<Intensity>> intensityOption = new List<Option<Intensity>>();
            foreach (Intensity intensity in intensityenum)
            {
                intensityOption.Add(new Option<Intensity>(intensity, intensity.ToString()));
            }
            WriteTitle();
            ColorConsole.WriteColorLine("Welke [intensiteit] wilt u? ", Globals.ColorInputcClarification);
            _Intensity = new SelectionMenuUtil<Intensity>(intensityOption, 15, WhatToDoWhenGoBack, () => Start(_returnToIntensity)).Create();
        }

        private static void AskForExperienceTimeLength()
        {
            PrintEditedList();
            List<int> intList = Enumerable.Range(1, 100).ToList();
            SelectionMenuUtil<int> selection = new SelectionMenuUtil<int>(intList, 1, () => Start(_returnToMovie), () => Start(_returnToLength), false, "Wat is de [tijdsduur]? (in minuten): ");
            _timeInInt = selection.Create();
            while (!experiencesLogic.ValidateExperienceTimeLength(_timeInInt))
            {
                _timeInInt = selection.Create();
            }
        }

        private static void AskForMovie()
        {
            PrintEditedList();
            List<MovieModel> movies = MoviesLogic.GetAll();
            List<Option<int>> movieOptions = new List<Option<int>>();
            foreach (MovieModel movie in movies)
            {
                if (movie.Status == Status.Active)
                {
                    movieOptions.Add(new Option<int>(movie.Id, movie.Title));

                }
            }
            WriteTitle();
            ColorConsole.WriteColorLine("Welke [film] wilt u toevoegen?", Globals.ColorInputcClarification);
            _selectedMovieId = new SelectionMenuUtil<int>(movieOptions, 15, () => Start(_returnToName), () => Start(_returnToMovie)).Create();
            Console.Clear();
        }

        private static void Print(string name, string description, int filmId, Intensity intensity, int timeLength)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("\nDe details van de experience zijn:", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience naam:] {name}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience beschrijving:] {description}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Film gekoppeld aan experience:] {MoviesLogic.GetById(filmId).Title}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience intensiteit:] {intensity}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience lengte (minuten):] {timeLength}\n", Globals.ExperienceColor);
            HorizontalLine.Print();
            ColorConsole.WriteColorLine($"Wilt u deze [Experience] toevoegen?", Globals.ColorInputcClarification);
        }

        private static void PrintEditedList()
        {
            Console.Clear();
            if (_newName != "")
            {
                ColorConsole.WriteColorLine("[Huidige Experience Details]", Globals.ExperienceColor);
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
            if (_Intensity != Intensity.Undefined)
            {
                ColorConsole.WriteColorLine($"[Intensiteit experience:] {_Intensity}", Globals.ExperienceColor);
            }
            if (_timeInInt != 0)
            {
                ColorConsole.WriteColorLine($"[Tijdsduur experience:] {_timeInInt} minuten", Globals.ExperienceColor);
            }
            if (_newName != "" || _selectedMovieId != 0 || _Intensity != Intensity.Undefined || _timeInInt != 0)
            {
                HorizontalLine.Print();
            }
        }
    }
}