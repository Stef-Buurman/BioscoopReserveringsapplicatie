namespace BioscoopReserveringsapplicatie
{
    static class UserLogin
    {
        static private UserLogic _userLogic = new UserLogic();

        public static void Start()
        {
            Console.Clear();

            string email = ReadLineUtil.EnterValue(false, () =>
            {
                ColorConsole.WriteColorLine("Loginpagina\n", Globals.TitleColor);
                Console.Write("Vul uw e-mailadres in: ");
            });
            Console.Write("Vul uw wachtwoord in: ");
            string password = ReadLineUtil.EnterValue(false, () =>
            {
                ColorConsole.WriteColorLine("Loginpagina\n", Globals.TitleColor);
                Console.WriteLine($"Vul uw e-mailadres in: {email}");
                Console.Write("Vul uw wachtwoord in: ");
            });

            if (_userLogic.CheckLogin(email, password) != null)
            {
                Console.WriteLine("U bent ingelogd.");
                Thread.Sleep(2000);

                if (UserLogic.CurrentUser.IsAdmin)
                {
                    AdminMenu.Start();
                }
                else
                {
                    UserMenu.Start();
                }
            }
            else
            {
                Console.WriteLine("Er is geen account gevonden met dat e-mailadres en wachtwoord.");
                Console.WriteLine("Druk op Esc om terug te gaan of op een willekeurige toets om het opnieuw te proberen.");

                ConsoleKeyInfo key = Console.ReadKey(true);

                if (key.Key == ConsoleKey.Escape)
                {
                    LandingPage.Start();
                }
                else
                {
                    Start();
                }
            }
        }
    }
}