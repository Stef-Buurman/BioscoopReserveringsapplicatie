namespace BioscoopReserveringsapplicatie
{
    static class UserLogin
    {
        static private UserLogic _userLogic = new UserLogic();
        static private bool _isFirstTime = true;

        public static void Start()
        {
            Console.Clear();
            if (_isFirstTime)
            {
                ColorConsole.WriteColorLine("Welcome to the [login page]", ConsoleColor.Blue);
                _isFirstTime = false;
            }
            Console.Write("Please enter your email address: ");
            string email = Console.ReadLine() ?? "";
            Console.Write("Please enter your password: ");
            string password = Console.ReadLine() ?? "";
            if (_userLogic.CheckLogin(email, password) == null)
            {
                Console.WriteLine("No account found with that email and password.");
                Console.WriteLine("Press any key to try again.");
                Console.ReadKey();
                Start();
            }
        }
    }
}