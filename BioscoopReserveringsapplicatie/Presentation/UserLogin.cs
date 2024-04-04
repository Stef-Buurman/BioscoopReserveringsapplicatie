namespace BioscoopReserveringsapplicatie
{
    static class UserLogin
    {
        static private UserLogic _userLogic = new UserLogic();
        static private bool _isFirstTime = true;

        public static void Start()
        {
            Console.Clear();
            string email = ReadLineUtil.EnterValue(false, () =>
            {
                if (_isFirstTime) ColorConsole.WriteColorLine("Welkom bij de [login pagina]", ConsoleColor.Blue);
                Console.Write("Vul uw e-mailadres in: ");
            });
            Console.Write("Vul uw wachtwoord in: ");
            string password = ReadLineUtil.EnterValue(false, () =>
            {
                if (_isFirstTime)
                {
                    ColorConsole.WriteColorLine("Welkom bij de [login pagina]", ConsoleColor.Blue);
                    _isFirstTime = false;
                }
                Console.WriteLine($"Vul uw e-mailadres in: {email}");
                Console.Write("Vul uw wachtwoord in: ");
            });
            if (_userLogic.CheckLogin(email, password) == null)
            {
                Console.WriteLine("Er is geen account gevonden met dat e-mailadres en wachtwoord.");
                Console.WriteLine("Druk op een willekeurige toets om het opnieuw te proberen.");
                Console.ReadKey();
                Start();
            }
        }
    }
}