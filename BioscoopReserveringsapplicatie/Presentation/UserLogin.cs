static class UserLogin
{
    static private UserLogic userLogic = new UserLogic();
    static private bool isFirstTime = true;

    public static void Start()
    {
        Console.Clear();
        if (isFirstTime) 
        {
            ColorConsole.WriteColorLine("Welkom bij de [login pagina]", ConsoleColor.Blue);
            isFirstTime = false; 
        }
        Console.Write("Voer alstublieft uw e-mailadres in: ");
        string ?email = Console.ReadLine();
        Console.Write("Please enter your password: ");
        string ?password = Console.ReadLine();
        UserModel ?acc = userLogic.CheckLogin(email, password);
        if (acc != null)
        {
            if (acc.IsAdmin)
            {
                AdminMenu.Start();
            }
            else
            {
                //UserMenu.Start();
            }
        }
        else
        {
            Console.WriteLine("Er is geen account gevonden met dat e-mailadres en wachtwoord.");
            Console.WriteLine("Druk op een willekeurige toets om opnieuw te proberen.");
            Console.ReadKey();
            Start(); 
        }
    }   
}