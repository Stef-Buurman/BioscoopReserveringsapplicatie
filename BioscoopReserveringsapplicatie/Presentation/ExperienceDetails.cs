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
                // new Option<string>("Koop tickets"),
                new Option<string>("Terug", () => PreferredExperiences.Start()),
            };

            SelectionMenu.Create(options, Print);
        }

        private static void AdminPreview(int experienceId)
        {
            experience = ExperienceLogic.GetById(experienceId);

            movie = MoviesLogic.GetMovieById(experience.FilmId);

            var options = new List<Option<string>>
            {
                new Option<string>("Bewerk experience", () => ExperienceEdit.Start(experienceId)),
                new Option<string>("Archiveer experience", () => ExperienceDelete.Start(experienceId)),
                new Option<string>("Terug", () => {Console.Clear(); ExperienceOverview.Start();}),
            };

            SelectionMenu.Create(options, Print);
        }

        private static void Print()
        {
            if (experience != null) Console.WriteLine($"De experience details zijn:\nNaam experience: {experience.Name} \nIntensiteit experience: {experience.Intensity} \nTijdsduur experience: {experience.TimeLength} minuten \nFilm titel: {movie.Title} \nFilm beschrijving: {movie.Description} \nFilm genre(s): {string.Join(", ", movie.Genres)} \nFilm kijkwijzer: {movie.Rating} \n\nWat wil je doen?");
        }
    }
}