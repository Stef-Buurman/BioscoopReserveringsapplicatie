namespace BioscoopReserveringsapplicatie
{
    static class UserLogin
    {
        private static UserLogic _userLogic = new UserLogic();

        public static void Start()
        {
            _userLogic = new UserLogic();
            
            Console.Clear();

            ColorConsole.WriteColorLine("Loginpagina\n", Globals.TitleColor);
            string email = ReadLineUtil.EnterValue("Vul uw [e-mailadres] in: ", LandingPage.Start);
            string password = ReadLineUtil.EnterValue("Vul uw [wachtwoord] in: ", LandingPage.Start, true, false);

            if (_userLogic.CheckLogin(email, password) != null)
            {
                ColorConsole.WriteColorLine("\nU bent ingelogd.", Globals.SuccessColor);
                WaitUtil.WaitTime(2000);

                if (UserLogic.IsAdmin())
                {
                    AdminMenu.Start();
                }
                else
                {
                    ShowPromotion.Start();
                }
            }
            else
            {
                ColorConsole.WriteColorLine("\nEr is geen account gevonden met dat e-mailadres en wachtwoord.", Globals.ErrorColor);
                ColorConsole.WriteColorLine("Druk op [Esc] om de loginpagina te verlaten of op een [willekeurige toets] om het opnieuw te proberen.", Globals.ColorInputcClarification);

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