namespace BioscoopReserveringsapplicatie
{
    static class UserLogin
    {
        static private UserLogic _userLogic = new UserLogic();

        public static void Start()
        {
            Console.Clear();

            string email = ReadLineUtil.EnterValue(true, () =>
            {
                ColorConsole.WriteColorLine("Loginpagina\n", Globals.TitleColor);
                ColorConsole.WriteColor("Vul uw [e-mailadres] in: ", Globals.ColorInputcClarification);
            }, LandingPage.Start);
            Console.Write("Vul uw wachtwoord in: ");
            string password = ReadLineUtil.EnterValue(true, () =>
            {
                ColorConsole.WriteColorLine("Loginpagina\n", Globals.TitleColor);
                ColorConsole.WriteColorLine($"Vul uw [e-mailadres] in: {email}", Globals.ColorInputcClarification);
                ColorConsole.WriteColor("Vul uw [wachtwoord] in: ", Globals.ColorInputcClarification);
            }, LandingPage.Start);

            if (_userLogic.CheckLogin(email, password) != null)
            {
                ColorConsole.WriteColorLine("\nU bent ingelogd.", Globals.SuccessColor);
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
                ColorConsole.WriteColorLine("\nEr is geen account gevonden met dat e-mailadres en wachtwoord.", Globals.ErrorColor);
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