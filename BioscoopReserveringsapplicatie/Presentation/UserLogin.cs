static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();


    public static void Start()
    {
        Console.WriteLine("Welcome to the login page");
        Console.WriteLine("Please enter your email address");
        string email = Console.ReadLine();
        Console.WriteLine("Please enter your password");
        string password = Console.ReadLine();
        accountsLogic.Login(email, password);
        if (AccountsLogic.CurrentAccount != null)
        {
            Console.WriteLine("Welcome back " + AccountsLogic.CurrentAccount.FullName);
            Console.WriteLine("Your email number is " + AccountsLogic.CurrentAccount.EmailAddress);

            //Write some code to go back to the menu
            //Menu.Start();
        }
        else
        {
            Console.WriteLine("No account found with that email and password");
            Start();
        }
    }
}