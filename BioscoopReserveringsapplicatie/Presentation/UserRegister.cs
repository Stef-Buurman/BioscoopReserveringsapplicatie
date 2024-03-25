namespace BioscoopReserveringsapplicatie
{
    static class UserRegister
    {
        static private UserLogic userLogic = new UserLogic();

    public static void Start(string? errorMessage = null)
    {
        Console.Clear();
        Console.WriteLine("Registratiepagina\n");
        if (errorMessage != null)
        {
            Console.WriteLine(errorMessage);
        }
        Console.Write("Naam: ");
        string userName = Console.ReadLine() ?? "";
        Console.Write("Email: ");
        string userEmail = Console.ReadLine() ?? "";
        Console.Write("Wachtwoord: ");
        string userPassword = Console.ReadLine() ?? "";

            userLogic.RegisterNewUser(userName, userEmail.ToLower(), userPassword);
        }
    }
}