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
                ColorConsole.WriteColorLine("Welkom bij de [login pagina]", ConsoleColor.Blue);
                _isFirstTime = false;
            }
            Console.Write("Please enter your email address: ");
            string email = Console.ReadLine() ?? "";
            Console.Write("Please enter your password: ");
            string password = Console.ReadLine() ?? "";
            if (_userLogic.CheckLogin(email, password) == null)
            {
                Console.WriteLine("Er is geen account gevonden met dat e-mailadres en wachtwoord.");
                Console.WriteLine("Druk op een willekeurige toets om opnieuw te proberen.");
                Console.ReadKey();
                Start();
            }
        }
    }
}