static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();
    static private bool isFirstTime = true;

    public static void Start()
    {
        Console.Clear();
        if (isFirstTime) 
        {
            ColorConsole.WriteColorLine("Welcome to the [login page]", ConsoleColor.Blue);
            isFirstTime = false; 
        }
        Console.Write("Please enter your email address: ");
        string email = Console.ReadLine();
        Console.Write("Please enter your password: ");
        string password = Console.ReadLine();
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        if (acc != null)
        {
            ColorConsole.WriteColorLine($"[Welcome back {acc.FullName}]", ConsoleColor.Green);
            Console.WriteLine($"Your email is {acc.EmailAddress}");
        }
        else
        {
            Console.WriteLine("No account found with that email and password.");
            Console.WriteLine("Press any key to try again.");
            Console.ReadKey();
            Start(); 
        }
    }   
}