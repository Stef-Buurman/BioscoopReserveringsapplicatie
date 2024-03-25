static class UserLogin
{
    static private UserLogic userLogic = new UserLogic();
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
            Console.WriteLine("No account found with that email and password.");
            Console.WriteLine("Press any key to try again.");
            Console.ReadKey();
            Start(); 
        }
    }   
}