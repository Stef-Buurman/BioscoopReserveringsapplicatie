namespace BioscoopReserveringsapplicatie
{
    static class UserRegister
    {
        static private UserLogic userLogic = new UserLogic();

        public static void Start(string? errorMessage = null, UserModel? user = null)
        {
            Console.Clear();
            PrintTitleAndErrorWhenExist(errorMessage);

            string userName;

            if (user != null && user.FullName != "")
            {
                userName = user.FullName;
            }
            else
            {
                userName = ReadLineUtil.EnterValue(true, () =>
                {
                    PrintTitleAndErrorWhenExist(errorMessage);
                    ColorConsole.WriteColor("Vul uw [naam] in: ", Globals.ColorInputcClarification);
                }, LandingPage.Start);
            }

            string userEmail;

            if (user != null && user.EmailAddress != "")
            {
                userEmail = user.EmailAddress;
            }
            else
            {
                userEmail = ReadLineUtil.EnterValue(true, () =>
                {
                    PrintTitleAndErrorWhenExist(errorMessage);
                    if (userName != "")
                    {
                        ColorConsole.WriteColorLine($"Vul uw [naam] in: {userName}", Globals.ColorInputcClarification);
                    }
                    else
                    {
                        ColorConsole.WriteColorLine("Vul uw [naam] in: ", Globals.ColorInputcClarification);
                    }
                    ColorConsole.WriteColor("Vul uw [e-mail] in: ", Globals.ColorInputcClarification);
                }, LandingPage.Start
                );
            }

            string userPassword = ReadLineUtil.EnterValue(true, () =>
            {
                PrintTitleAndErrorWhenExist(errorMessage);
                if (userName != "")
                {
                    ColorConsole.WriteColorLine($"Vul uw [naam] in: {userName}", Globals.ColorInputcClarification);
                }
                else
                {
                    ColorConsole.WriteColorLine("Vul uw [naam] in: ", Globals.ColorInputcClarification);
                }
                if (userEmail != "")
                {
                    ColorConsole.WriteColorLine($"Vul uw [e-mail] in: {userEmail}", Globals.ColorInputcClarification);
                }
                else
                {
                    ColorConsole.WriteColorLine("Vul uw [e-mail] in: ", Globals.ColorInputcClarification);
                }
                ColorConsole.WriteColor("Vul uw [wachtwoord] in: ", Globals.ColorInputcClarification);
            }, LandingPage.Start
            );

            RegistrationResult registrationResult = userLogic.RegisterNewUser(userName, userEmail, userPassword);

            if (!registrationResult.IsValid)
            {
                Start(registrationResult.ErrorMessage, registrationResult.User);
            }
            else
            {
                Preferences.Start(registrationResult.User);
                ColorConsole.WriteColorLine("U bent geregistreerd.", Globals.SuccessColor);
                Thread.Sleep(2000);
                UserLogin.Start();
            }
        }

        private static void PrintTitleAndErrorWhenExist(string? errorMessage)
        {
            ColorConsole.WriteColorLine("Registratiepagina\n", Globals.TitleColor);
            if (errorMessage != null)
            {
                Console.WriteLine(errorMessage);
            }
        }
    }
}