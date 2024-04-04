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
                userName = ReadLineUtil.EnterValue(false, () =>
                {
                    PrintTitleAndErrorWhenExist(errorMessage);
                    Console.Write("Naam: ");
                });
            }

            string userEmail;

            if (user != null && user.EmailAddress != "")
            {
                userEmail = user.EmailAddress;
            }
            else
            {
                userEmail = ReadLineUtil.EnterValue(false, () =>
                {
                    PrintTitleAndErrorWhenExist(errorMessage);
                    if (userName != "")
                    {
                        Console.WriteLine($"Naam: {userName}");
                    }
                    Console.Write("Email: ");
                }
                );
            }

            string userPassword;

            if (user != null && user.Password != "")
            {
                Console.WriteLine($"Wachtwoord: {user.Password}");
                userPassword = user.Password;
            }
            else
            {
                userPassword = ReadLineUtil.EnterValue(false, () =>
                {
                    PrintTitleAndErrorWhenExist(errorMessage);
                    if (userName != "")
                    {
                        Console.WriteLine($"Naam: {userName}");
                    }
                    if (userEmail != "")
                    {
                        Console.WriteLine($"Email: {userEmail}");
                    }
                    Console.Write("Wachtwoord: ");
                }
                );
            }

            RegistrationResult registrationResult = userLogic.RegisterNewUser(userName, userEmail, userPassword);
            if (!registrationResult.IsValid) Start(registrationResult.ErrorMessage, registrationResult.User);
        }

        private static void PrintTitleAndErrorWhenExist(string? errorMessage)
        {
            ColorConsole.WriteColorLine("[Registratiepagina]\n", Globals.TitleColor);
            if (errorMessage != null)
            {
                Console.WriteLine(errorMessage);
            }
        }
    }
}