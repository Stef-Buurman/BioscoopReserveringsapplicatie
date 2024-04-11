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
                    Console.Write("Naam: ");
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
                        Console.WriteLine($"Naam: {userName}");
                    }
                    else
                    {
                        Console.WriteLine("Naam: ");
                    }
                    Console.Write("Email: ");
                }, LandingPage.Start
                );
            }

            string userPassword = ReadLineUtil.EnterValue(true, () =>
            {
                PrintTitleAndErrorWhenExist(errorMessage);
                if (userName != "")
                {
                    Console.WriteLine($"Naam: {userName}");
                }
                else
                {
                    Console.WriteLine("Naam: ");
                }
                if (userEmail != "")
                {
                    Console.WriteLine($"Email: {userEmail}");
                }
                else
                {
                    Console.WriteLine("Email: ");
                }
                Console.Write("Wachtwoord: ");
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