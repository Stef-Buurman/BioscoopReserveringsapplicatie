﻿using System;
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
                Print(name, filmId, intensity, timeLength);
                new SelectionMenuUtil2<string>(options).Create();
            }
            else
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", WhatToDoWhenGoBack),
                };
                ColorConsole.WriteColorLine("Er is een error opgetreden tijdens het toevoegen van de experience.", Globals.ErrorColor);
                new SelectionMenuUtil2<string>(options);
            }
        }

        private static void WriteTitle() => ColorConsole.WriteColorLine("Experience Toevoegen\n", Globals.TitleColor);

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
                    ColorConsole.WriteColorLine("Voer alstublieft een geldige naam in!", Globals.ErrorColor);
                    ColorConsole.WriteColor($"Wat is de [naam] van de experience?: ", Globals.ColorInputcClarification);
                },
                WhatToDoWhenGoBack);
            }
            return name;
        }

        private static Intensity AskForExperienceIntensity(Action functionToShow)
        {
            List<Intensity> intensityenum = Globals.GetAllEnum<Intensity>();
            List<Option<Intensity>> intensityOption = new List<Option<Intensity>>();
            functionToShow();
            ColorConsole.WriteColorLine("Welke [intensiteit] wilt u? ", Globals.ColorInputcClarification);
            Intensity intensity = new SelectionMenuUtil2<Intensity>(intensityOption, 15, WhatToDoWhenGoBack, () => { }).Create();
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
                    ColorConsole.WriteColorLine("Voer alstublieft een geldige tijdsduur in!", Globals.ErrorColor);
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
                if (!movie.Archived)
                {
                    movieOptions.Add(new Option<int>(movie.Id, movie.Title));

                }
            }
            functionToShow();
            ColorConsole.WriteColorLine("Welke [film] wilt u toevoegen?", Globals.ColorInputcClarification);
            int movieId = new SelectionMenuUtil2<int>(movieOptions, 15, WhatToDoWhenGoBack, () => { }).Create();
            Console.Clear();
            return movieId;
        }

        private static void Print(string name, int filmId, Intensity intensity, int timeLength)
        {
            ColorConsole.WriteColorLine("De experience is succesvol toegevoegd.", Globals.SuccessColor);
            ColorConsole.WriteColorLine("\nDe details van de experience zijn:", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience naam:] {name}",Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Film gekoppeld aan experience:] {moviesLogic.GetMovieById(filmId).Title}",Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience intensiteit:] {intensity}",Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience lengte (minuten):] {timeLength}\n",Globals.ExperienceColor);
        }
    }
}