namespace BioscoopReserveringsapplicatie
{
    static class UserMenu
    {
        private static UserModel? CurrentUser = UserLogic.CurrentUser;
        public static void Start()
        {
            if (CurrentUser != null)
            {
                Console.Clear();
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Experienceoverzicht", () => PreferredExperiences.Start()),
                    new Option<string>("Mijn account", () => Profile.Start()),
                    new Option<string>("Uitloggen", () => UserLogic.LogOut()),
                };
                SelectionMenu.Create(options, () => Console.WriteLine($"Welkom {CurrentUser.FullName}!\n"));
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Geen gebruiker gevonden.");
            }
        }
    }
}