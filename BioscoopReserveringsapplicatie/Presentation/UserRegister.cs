static class UserRegister
{
    static private AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start()
    {
        Console.Clear();
        Console.WriteLine("registratiepagina\n\n");
        Console.WriteLine("Naam: ");
        string userName = Console.ReadLine() ?? "";
        Console.WriteLine("Email: ");
        string userEmail = Console.ReadLine() ?? "";
        Console.WriteLine("Wachtwoord: ");
        string userPassword = Console.ReadLine() ?? "";

        accountsLogic.RegisterNewUser(userName, userEmail, userPassword);
    }
}