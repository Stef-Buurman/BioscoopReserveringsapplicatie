namespace BioscoopReserveringsapplicatie
{
    static class UserRegister
    {
        static private UserLogic userLogic = new UserLogic();

        public static void Start(string? errorMessage = null, UserModel? user = null)
        {
            Console.Clear();
            Console.WriteLine("Registratiepagina\n");

            if (errorMessage != null)
            {
                Console.WriteLine(errorMessage);
            }

            string userName;

            if (user != null && user.FullName != "")
            {
                Console.WriteLine($"Naam: {user.FullName}");
                userName = user.FullName;
            }
            else
            {
                Console.Write("Naam: ");
                userName = Console.ReadLine() ?? "";
            }

            string userEmail;

            if (user != null && user.EmailAddress != "")
            {
                Console.WriteLine($"Email: {user.EmailAddress}");
                userEmail = user.EmailAddress;
            }
            else
            {
                Console.Write("Email: ");
                userEmail = Console.ReadLine() ?? "";
            }

            string userPassword;

            if (user != null && user.Password != "")
            {
                Console.WriteLine($"Wachtwoord: {user.Password}");
                userPassword = user.Password;
            }
            else
            {
                Console.Write("Wachtwoord: ");
                userPassword = Console.ReadLine() ?? "";
            }

            RegistrationResult registrationResult = userLogic.RegisterNewUser(userName, userEmail, userPassword);

            if (!registrationResult.IsValid)
            {
                Start(registrationResult.ErrorMessage, registrationResult.User);
            }
            else
            {
                Preferences.Start(registrationResult.User);
                Console.WriteLine("U bent geregistreerd.");
                Thread.Sleep(2000);
                UserLogin.Start();
            }
        }
    }
}