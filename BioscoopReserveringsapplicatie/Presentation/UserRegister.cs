namespace BioscoopReserveringsapplicatie
{
    static class UserRegister
    {
        static private UserLogic userLogic = new UserLogic();

        public static void Start(string? errorMessage = null, string? userEmail = "", string? userName = "", string? userPassword = "")
        {
            Console.Clear();
            Console.WriteLine("Registratiepagina\n");
            if (errorMessage != null)
            {
                Console.WriteLine(errorMessage);
            }

            if (userName != "")
            {
                Console.WriteLine("Naam: " + userName);
            }
            else
            {
                Console.Write("Naam: ");
                userName = Console.ReadLine() ?? "";
            }

            if (userEmail != "")
            {
                Console.WriteLine("Email: " + userEmail);
            }
            else
            {
                Console.Write("Email: ");
                userEmail = Console.ReadLine() ?? "";
            }

            if (userPassword != "")
            {
                Console.WriteLine("Wachtwoord: " + userPassword);
            }
            else
            {
                Console.Write("Wachtwoord: ");
                userPassword = Console.ReadLine() ?? "";
            }

            userLogic.RegisterNewUser(userName, userEmail.ToLower(), userPassword);
        }
    }
}