static class UserRegister
{
    static private UserLogic accountsLogic = new UserLogic();

    public static void Start(string? errorMessage = null)
    {
        Console.Clear();
        Console.WriteLine("registratiepagina\n\n");
        if (errorMessage != null)
        {
            Console.WriteLine(errorMessage);
        }
        Console.WriteLine("Naam: ");
        string userName = Console.ReadLine() ?? "";
        Console.WriteLine("Email: ");
        string userEmail = Console.ReadLine() ?? "";
        Console.WriteLine("Wachtwoord: ");
        string userPassword = Console.ReadLine() ?? "";

        accountsLogic.RegisterNewUser(userName, userEmail, userPassword);
    }
}