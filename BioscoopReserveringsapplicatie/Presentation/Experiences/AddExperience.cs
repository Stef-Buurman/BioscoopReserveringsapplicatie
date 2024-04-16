﻿using System;
namespace BioscoopReserveringsapplicatie
{
    static class AddExperience
    {
        private static ExperiencesLogic experiencesLogic = new ExperiencesLogic();
        private static MoviesLogic MoviesLogic = new MoviesLogic();
        private static string _newName = "";
        private static int _selectedMovieId = 0;
        private static Intensity _Intensity = Intensity.Undefined;
        private static int _timeInInt = 0;

        private static string _returnToName = "Name";
        private static string _returnToMovie = "Movie";
        private static string _returnToIntensity = "Intensity";
        private static string _returnToLength = "Length";
        public static void Start(string returnTo = "")
        {
            Console.Clear();
            if(returnTo == "" || returnTo == _returnToName)
            {
                AskForExperienceName();
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

            ExperiencesModel newExperience = new ExperiencesModel(_newName, _selectedMovieId, _Intensity, _timeInInt, archived: false);
            if (experiencesLogic.AddExperience(newExperience))
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", WhatToDoWhenGoBack),
                };
                Print(_newName, _selectedMovieId, _Intensity, _timeInInt);
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

        private static void AskForExperienceName()
        {
            PrintEditedList();
            _newName = ReadLineUtil.EnterValue(
                () =>
                {
                    WriteTitle();
                    ColorConsole.WriteColor($"Wat is de [naam] van de experience?: ", Globals.ColorInputcClarification);
                },
                WhatToDoWhenGoBack);
            while (!experiencesLogic.ValidateExperienceName(_newName))
            {
                _newName = ReadLineUtil.EnterValue(
                () =>
                {
                    WriteTitle();
                    ColorConsole.WriteColorLine("Voer alstublieft een geldige naam in!", Globals.ErrorColor);
                    ColorConsole.WriteColor($"Wat is de [naam] van de experience?: ", Globals.ColorInputcClarification);
                },
                WhatToDoWhenGoBack);
            }
        }

        private static void AskForExperienceIntensity()
        {
            PrintEditedList();
            List<Intensity> intensityenum = Globals.GetAllEnum<Intensity>();
            List<Option<Intensity>> intensityOption = new List<Option<Intensity>>();
            WriteTitle();
            ColorConsole.WriteColorLine("Welke [intensiteit] wilt u? ", Globals.ColorInputcClarification);
            _Intensity = new SelectionMenuUtil2<Intensity>(intensityOption, 15, WhatToDoWhenGoBack, () => Start(_returnToIntensity)).Create();
        }

        private static void AskForExperienceTimeLength()
        {
            List<int> intList = Enumerable.Range(1, 100).ToList();
            SelectionMenuUtil2<int> selection = new SelectionMenuUtil2<int>(intList, 1, () => Start(_returnToMovie), () => Start(_returnToLength), false, "Wat is de [tijdsduur]? (in minuten): ");
            _timeInInt = selection.Create();
            while (!experiencesLogic.ValidateExperienceTimeLength(_timeInInt))
            {
                _timeInInt = selection.Create();
            }
        }

        private static void AskForMovie()
        {
            PrintEditedList();
            List<MovieModel> movies = MoviesLogic.GetAllMovies();
            List<Option<int>> movieOptions = new List<Option<int>>();
            foreach (MovieModel movie in movies)
            {
                if (!movie.Archived)
                {
                    movieOptions.Add(new Option<int>(movie.Id, movie.Title));

                }
            }
            WriteTitle();
            ColorConsole.WriteColorLine("Welke [film] wilt u toevoegen?", Globals.ColorInputcClarification);
            _selectedMovieId = new SelectionMenuUtil2<int>(movieOptions, 15, () => Start(_returnToName), () => Start(_returnToMovie)).Create();
            Console.Clear();
        }

        private static void Print(string name, int filmId, Intensity intensity, int timeLength)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("De experience is succesvol toegevoegd.", Globals.SuccessColor);
            ColorConsole.WriteColorLine("\nDe details van de experience zijn:", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience naam:] {name}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Film gekoppeld aan experience:] {MoviesLogic.GetMovieById(filmId).Title}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience intensiteit:] {intensity}", Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"[Experience lengte (minuten):] {timeLength}\n", Globals.ExperienceColor);
        }

        private static void PrintEditedList()
        {
            Console.Clear();
            if (_newName != "")
            {
                ColorConsole.WriteColorLine("[Huidige Experience Details]", Globals.ExperienceColor);
                ColorConsole.WriteColorLine($"[Naam experience:] {_newName}", Globals.ExperienceColor);
            }
            if (_selectedMovieId != 0)
            {
                ColorConsole.WriteColorLine($"[Film titel:] {MoviesLogic.GetMovieById(_selectedMovieId).Title}", Globals.ExperienceColor);
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
                ColorConsole.WriteColorLine("---------------------------------------------------------------", ConsoleColor.White);
            }
        }
    }
}