namespace BioscoopReserveringsapplicatie
{
    static class UserLogin
    {
        private static UserLogic _userLogic = new UserLogic();

        public static void Start()
        {
            Console.Clear();

            ColorConsole.WriteColorLine("Loginpagina\n", Globals.TitleColor);
            string email = ReadLineUtil.EnterValue("Vul uw [e-mailadres] in: ", LandingPage.Start);
            string password = ReadLineUtil.EnterValue("Vul uw [wachtwoord] in: ", LandingPage.Start, true, false);

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