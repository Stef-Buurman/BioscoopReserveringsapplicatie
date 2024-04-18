namespace BioscoopReserveringsapplicatie
{
    static class ExperienceDetails
    {
        static private ExperiencesLogic ExperienceLogic = new ExperiencesLogic();
        private static ExperiencesModel? experience;
        static private MoviesLogic MoviesLogic = new MoviesLogic();
        private static MovieModel? movie;

        public static void Start(int experienceId)
        {
            Console.Clear();
            if (UserLogic.CurrentUser != null && !UserLogic.CurrentUser.IsAdmin)
            {
                UserPreview(experienceId);
            }
            else if (UserLogic.CurrentUser != null && UserLogic.CurrentUser.IsAdmin)
            {
                AdminPreview(experienceId);
            }
        }

        private static void UserPreview(int experienceId)
        {
            experience = ExperienceLogic.GetById(experienceId);

            movie = MoviesLogic.GetMovieById(experience.FilmId);

            var options = new List<Option<string>>
            {
                new Option<string>("Reserveer experience", () => ExperienceReservation.Start(experienceId)),
                new Option<string>("Terug", () => PreferredExperiences.Start()),
            };

            Print();
            new SelectionMenuUtil2<string>(options).Create();
        }

        private static void AdminPreview(int experienceId)
        {
            experience = ExperienceLogic.GetById(experienceId);

            List<Option<string>> options;

            movie = MoviesLogic.GetMovieById(experience.FilmId);

            if (experience.Archived)
            {
                options = new List<Option<string>>
                {
                    new Option<string>("Experience bewerken", () => ExperienceEdit.Start(experienceId)),
                    new Option<string>("Terug", () => {Console.Clear(); ExperienceOverview.Start();}),
                };
            }
            else
            {
                options = new List<Option<string>>
                {
                    new Option<string>("Experience bewerken", () => ExperienceEdit.Start(experienceId)),
                    new Option<string>("Experience inplannen", () => ScheduleExperience.Start(experienceId)),
                    new Option<string>("Experience archiveren", () => ExperienceArchive.Start(experienceId)),
                    new Option<string>("Terug", () => {Console.Clear(); ExperienceOverview.Start();}),
                };
            }

            Print();
            new SelectionMenuUtil2<string>(options).Create();
        }

        private static void Print()
        {
            if (experience != null)
            {
                ColorConsole.WriteColorLine("Experience details", Globals.ExperienceColor);
                ColorConsole.WriteColorLine($"[Naam experience: ]{experience.Name}", Globals.ExperienceColor);
                ColorConsole.WriteColorLine($"[Intensiteit experience: ]{experience.Intensity}", Globals.ExperienceColor);
                ColorConsole.WriteColorLine($"[Tijdsduur experience: ]{experience.TimeLength} minuten\n", Globals.ExperienceColor);
                ColorConsole.WriteColorLine($"Film details", Globals.MovieColor);
                ColorConsole.WriteColorLine($"[Film Titel: ]{movie.Title}", Globals.MovieColor);
                ColorConsole.WriteColorLine($"[Film beschrijving: ]{movie.Description}", Globals.MovieColor);
                ColorConsole.WriteColorLine($"[Film genre(s): ]{string.Join(", ", movie.Genres)}", Globals.MovieColor);
                ColorConsole.WriteColorLine($"[Film kijkwijzer: ]{movie.AgeCategory.GetDisplayName()}\n\n", Globals.MovieColor);
                Console.WriteLine("Wat wil je doen?");
            }
        }
    }
}