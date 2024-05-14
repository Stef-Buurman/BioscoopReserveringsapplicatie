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
                userName = ReadLineUtil.EnterValue("Vul uw [naam] in: ", LandingPage.Start);
            }

            string userEmail = "";

            if (user != null && user.EmailAddress != "")
            {
                userEmail = user.EmailAddress;
            }
            else
            {
                if (errorMessage != null) ColorConsole.WriteColorLine($"Vul uw [naam] in: {userName}", Globals.ColorInputcClarification);
                userEmail = ReadLineUtil.EnterValue("Vul uw [e-mail] in: ", LandingPage.Start, false, false);
                errorMessage = null;
            }

            if (errorMessage != null)
            {
                ColorConsole.WriteColorLine($"Vul uw [naam] in: {userName}", Globals.ColorInputcClarification);
                ColorConsole.WriteColorLine($"Vul uw [e-mail] in: {userEmail}", Globals.ColorInputcClarification);
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
                Thread.Sleep(2000);
                UserLogin.Start();
            }
        }
    }
}