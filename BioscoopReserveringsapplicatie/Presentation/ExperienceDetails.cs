static class ExperienceDetails
{
    static private ExperiencesLogic ExperienceLogic = new ExperiencesLogic();
    private static ExperiencesModel? experience;
    static private MoviesLogic MoviesLogic = new MoviesLogic();
    private static MovieModel? movie;

    public static void Start(int experienceId)
    {
        experience = ExperienceLogic.GetById(experienceId);

        movie = MoviesLogic.GetMovieById(experience.FilmId);

        var options = new List<Option<string>>
            {
                new Option<string>("Koop tickets"),
                new Option<string>("Terug"),
            };

        SelectionMenu.Create(options, Print);
    }

    private static void Print()
    {
        if (experience != null) Console.WriteLine($"Experience details:\nNaam experience: {experience.Name} \nIntensiteit: {experience.Intensity} \nTijdsduur: {experience.TimeLength} \nTitel: {movie.Title} \nOmschrijving: {movie.Description} \nGenre(s): {string.Join(", ", movie.Genres)} \nKijkwijzer: {movie.Rating} \n\nWat wil je doen?");
    }
}
