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
            string name = AskForExperienceName(() => WriteTitle());
            int filmId = AskForMovie(() => WriteTitle());
            Intensity intensity = AskForExperienceIntensity(() => WriteTitle());
            int timeLength = AskForExperienceTimeLength(() => WriteTitle());

            ExperiencesModel newExperience = new ExperiencesModel(name, filmId, intensity, timeLength, archived: false);
            if (experiencesLogic.AddExperience(newExperience))
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", WhatToDoWhenGoBack),
                };
                SelectionMenu.Create(options, () => Print(name, filmId, intensity, timeLength));
            }
            else
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", WhatToDoWhenGoBack),
                };
                SelectionMenu.Create(options, () => ColorConsole.WriteColorLine("[Er is een error opgetreden tijdens het toevoegen van de experience.]", ConsoleColor.Red));
            }
        }

        private static void WriteTitle() => ColorConsole.WriteColorLine("[Experience Toevoegen]\n", Globals.TitleColor);

        private static void WhatToDoWhenGoBack()
        {
            Console.Clear();
            AdminMenu.Start();
        }

        private static string AskForExperienceName(Action functionToShow)
        {
            string name = ReadLineUtil.EnterValue(
                () =>
                {
                    functionToShow();
                    ColorConsole.WriteColor($"Wat is de [naam] van de experience?: ", Globals.ColorInputcClarification);
                },
                WhatToDoWhenGoBack);
            while (!experiencesLogic.ValidateExperienceName(name))
            {
                name = ReadLineUtil.EnterValue(
                () =>
                {
                    functionToShow();
                    Console.WriteLine("Voer alstublieft een geldige naam in!");
                    ColorConsole.WriteColor($"Wat is de [naam] van de experience?: ", Globals.ColorInputcClarification);
                },
                WhatToDoWhenGoBack);
            }
            return name;
        }

        private static Intensity AskForExperienceIntensity(Action functionToShow)
        {

            string intensitystr = ReadLineUtil.EnterValue(
                () =>
                {
                    functionToShow();
                    ColorConsole.WriteColor($"Wat is de [intensiteit]? ", Globals.ColorInputcClarification);
                },
                WhatToDoWhenGoBack);
            Intensity intensity;
            while (!Enum.TryParse(intensitystr, out intensity) && !experiencesLogic.ValidateExperienceIntensity(intensity))
            {
                intensitystr = ReadLineUtil.EnterValue(
                () =>
                {
                    functionToShow();
                    Console.WriteLine("Voer alstublieft een geldige intensiteit in!");
                    ColorConsole.WriteColor($"Wat is de [intensiteit]? ", Globals.ColorInputcClarification);
                },
                WhatToDoWhenGoBack);
            }
            return intensity;
        }

        private static int AskForExperienceTimeLength(Action functionToShow)
        {
            string timeLengthStr = ReadLineUtil.EnterValue(
                () =>
                {
                    functionToShow();
                    ColorConsole.WriteColor($"Wat is de [tijdsduur]? (in minuten): ", Globals.ColorInputcClarification);
                },
                WhatToDoWhenGoBack);
            while (!experiencesLogic.ValidateExperienceTimeLength(timeLengthStr))
            {
                timeLengthStr = ReadLineUtil.EnterValue(
                () =>
                {
                    functionToShow();
                    Console.WriteLine("Voer alstublieft een geldige tijdsduur in!");
                    ColorConsole.WriteColor($"Wat is de [tijdsduur]? (in minuten): ", Globals.ColorInputcClarification);
                },
                WhatToDoWhenGoBack);
            }
            return Convert.ToInt32(timeLengthStr);
        }

        private static int AskForMovie(Action functionToShow)
        {
            List<MovieModel> movies = moviesLogic.GetAllMovies();
            List<Option<int>> movieOptions = new List<Option<int>>();
            foreach (MovieModel movie in movies)
            {
                movieOptions.Add(new Option<int>(movie.Id, movie.Title));
            }
            int movieId = SelectionMenu.Create(movieOptions, 10, () =>
            {
                functionToShow();
                Console.WriteLine("Welke film wilt u toevoegen?");
            }
            );
            Console.Clear();
            return movieId;
        }

        private static void Print(string name, int filmId, Intensity intensity, int timeLength)
        {
            ColorConsole.WriteColorLine("[De experience is succesvol toegevoegd.]", ConsoleColor.Green);
            ColorConsole.WriteColorLine("\n[De details van de experience zijn:]", Globals.TitleColor);
            Console.WriteLine($"Experience naam: {name}");
            Console.WriteLine($"Film: {moviesLogic.GetMovieById(filmId).Title}");
            Console.WriteLine($"Experience intensiteit: {intensity}");
            Console.WriteLine($"Experience lengte (minuten): {timeLength}\n");
        }
    }
}