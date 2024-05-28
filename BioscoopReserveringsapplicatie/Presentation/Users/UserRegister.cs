namespace BioscoopReserveringsapplicatie
{
    static class UserRegister
    {
        private static UserLogic userLogic = new UserLogic();

        public static void Start(string? errorMessage = null, UserModel? user = null)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("Registratiepagina\n", Globals.TitleColor);
            if (errorMessage != null)
            {
                Console.WriteLine(errorMessage);
            }

            string userName;

            if (user != null && user.FullName != "")
            {
                userName = user.FullName;
                if (errorMessage != null) ColorConsole.WriteColorLine($"Vul uw [volledige naam] in: {userName}", Globals.ColorInputcClarification);
            }
            else
            {
                userName = ReadLineUtil.EnterValue("Vul uw [volledige naam] in: ", LandingPage.Start);
            }

            string userEmail = "";

            if (user != null && user.EmailAddress != "")
            {
                userEmail = user.EmailAddress;
                if (errorMessage != null) ColorConsole.WriteColorLine($"Vul uw [e-mailadres] in: {userEmail}", Globals.ColorInputcClarification);
            }
            else
            {
                userEmail = ReadLineUtil.EnterValue("Vul uw [e-mailadres] in: ", LandingPage.Start, false, false);
            }

            string userPassword = ReadLineUtil.EnterValue("Vul uw [wachtwoord] in: ", LandingPage.Start, true, false);
            string confirmPassword = ReadLineUtil.EnterValue("Vul nogmaals uw [wachtwoord] in: ", LandingPage.Start, true, false);

            Result<UserModel> registrationResult = userLogic.RegisterNewUser(userName, userEmail, userPassword, confirmPassword);

            if (!registrationResult.IsValid)
            {
                Start(registrationResult.ErrorMessage, registrationResult.Item);
            }
            else
            {
                ColorConsole.WriteColorLine("\nRegistratie succesvol, u wordt nu doorverwezen om uw accountvoorkeuren in te stellen.", Globals.SuccessColor);

                WaitUtil.WaitTime(4000);

                Preferences.Start(registrationResult.Item);

                ColorConsole.WriteColorLine("\nU bent klaar met het instellen van uw account en kunt nu inloggen.", Globals.SuccessColor);

                WaitUtil.WaitTime(4000);

                UserLogin.Start();
            }
        }
    }
}