namespace BioscoopReserveringsapplicatie
{
    static class UserMenu
    {
        public static void Start()
        {
            if (UserLogic.CurrentUser != null)
            {
                Console.Clear();
                List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Experienceoverzicht", () => PreferredExperiences.Start()),
                // new Option<string>("Mijn account"),
                new Option<string>("Uitloggen", () => UserLogic.LogOut()),
            };
                SelectionMenu.Create(options, () => Console.WriteLine($"Welkom {UserLogic.CurrentUser.FullName}!\n"));
            }
            else
            {
                Console.Clear();
                Console.WriteLine("Geen gebruiker gevonden.");
            }
        }
    }
}