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
            }
            else
            {
                userName = ReadLineUtil.EnterValue("Vul uw [volledige naam] in: ", LandingPage.Start);
            }

            string userEmail = "";

            if (user != null && user.EmailAddress != "")
            {
                userEmail = user.EmailAddress;
            }
            else
            {
                if (errorMessage != null) ColorConsole.WriteColorLine($"Vul uw [volledige naam] in: {userName}", Globals.ColorInputcClarification);
                userEmail = ReadLineUtil.EnterValue("Vul uw [e-mailadres] in: ", LandingPage.Start, false, false);
                errorMessage = null;
            }

            if (errorMessage != null)
            {
                ColorConsole.WriteColorLine($"Vul uw [volledige naam] in: {userName}", Globals.ColorInputcClarification);
                ColorConsole.WriteColorLine($"Vul uw [e-mailadres] in: {userEmail}", Globals.ColorInputcClarification);
            }

            string userPassword = ReadLineUtil.EnterValue("Vul uw [wachtwoord] in: ", LandingPage.Start, true, false);
            string confirmPassword = ReadLineUtil.EnterValue("Bevestig uw [wachtwoord]: ", LandingPage.Start, true, false);
            while (userPassword != confirmPassword)
            {
                ColorConsole.WriteColorLine("Wachtwoorden komen niet overeen. Probeer het opnieuw.", Globals.ErrorColor);
                userPassword = ReadLineUtil.EnterValue("Vul uw [wachtwoord] in: ", LandingPage.Start, true, false);
                confirmPassword = ReadLineUtil.EnterValue("Bevestig uw [wachtwoord]: ", LandingPage.Start, true, false);
            }

            Result<UserModel> registrationResult = userLogic.RegisterNewUser(userName, userEmail, userPassword);

            if (!registrationResult.IsValid)
            {
                Start(registrationResult.ErrorMessage, registrationResult.Item);
            }
            else
            {
                Console.Clear();
                PrintNameEmail(registrationResult.Item);
                WaitUtil.WaitTime(4000);
                Preferences.Start(registrationResult.Item);
                ColorConsole.WriteColorLine("\nU bent geregistreerd.", Globals.SuccessColor);
                WaitUtil.WaitTime(2000);
                UserLogin.Start();
            }
        }

        public static void PrintNameEmail(UserModel user)
        {
            ColorConsole.WriteColorLine("Uw gegevens", Globals.UserColor);
            ColorConsole.WriteColorLine($"[Naam: ]{user.FullName}", Globals.UserColor);
            ColorConsole.WriteColorLine($"[Email: ]{user.EmailAddress}\n", Globals.UserColor);
            ColorConsole.WriteColorLine("Registratie is gelukt, u wordt nu doorverwezen om uw voorkeuren in te stellen.", Globals.SuccessColor);
        }
    }
}