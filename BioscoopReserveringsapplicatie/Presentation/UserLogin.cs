namespace BioscoopReserveringsapplicatie
{
    static class UserLogin
    {
        static private UserLogic _userLogic = new UserLogic();

        public static void Start()
        {
            Console.Clear();
            Console.WriteLine("Loginpagina\n");

            Console.Write("Vul uw e-mailadres in: ");
            string email = Console.ReadLine() ?? "";
            Console.Write("Vul uw wachtwoord in: ");
            string password = Console.ReadLine() ?? "";

            if (_userLogic.CheckLogin(email, password) != null)
            {
                Console.WriteLine("U bent ingelogd.");
                Thread.Sleep(2000);

                if (UserLogic.CurrentUser.IsAdmin)
                {
                    AdminMenu.Start();
                }
                else
                {
                    UserMenu.Start();
                }
            }
            else
            {
                Console.WriteLine("Er is geen account gevonden met dat e-mailadres en wachtwoord.");
                Console.WriteLine("Druk op een willekeurige toets om het opnieuw te proberen.");
                Console.ReadKey();
                Start();
            }
        }
    }
}