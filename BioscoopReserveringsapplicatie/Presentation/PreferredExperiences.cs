static class PreferredExperiences
{
    static private ExperiencesLogic ExperienceLogic = new ExperiencesLogic();

    public static void Start()
    {
        int experienceId = ShowExperiencesWithUserPreferences();

        if (experienceId != 0) ExperienceDetails.Start(experienceId);
    }

    private static int ShowExperiencesWithUserPreferences()
    {
        AccountModel? currentUser = AccountsLogic.CurrentAccount;

        if (currentUser == null)
        {
            Console.WriteLine("Geen gebruiker gevonden");
            return 0;
        }

        List<Option<int>> options = new List<Option<int>>();
        List<ExperiencesModel> experiences = ExperienceLogic.GetExperiencesByUserPreferences(currentUser);

        foreach (ExperiencesModel experience in experiences)
        {
            options.Add(new Option<int>(experience.Id, experience.Name));
        }

        options.Add(new Option<int>(0, "Terug", () => { Console.Clear(); Menu.Start(); }));

        int experienceId = SelectionMenu.Create(options, Print);

        Console.Clear();

        return experienceId;
    }

    private static void Print()
    {
        Console.WriteLine("Dit zijn jouw aanbevolen experiences op basis van jouw voorkeuren:");
    }
}
