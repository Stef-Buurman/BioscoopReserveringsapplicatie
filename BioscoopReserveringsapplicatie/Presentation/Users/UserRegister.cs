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

            Result<UserModel> registrationResult = userLogic.RegisterNewUser(userName, userEmail, userPassword);

            if (!registrationResult.IsValid)
            {
                Start(registrationResult.ErrorMessage, registrationResult.Item);
            }
            else
            {
                Preferences.Start(registrationResult.Item);
                ColorConsole.WriteColorLine("\nU bent geregistreerd.", Globals.SuccessColor);
                WaitUtil.WaitTime(2000);
                UserLogin.Start();
            }
        }
    }
}