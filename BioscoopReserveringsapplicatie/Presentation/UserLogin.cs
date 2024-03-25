static class UserLogin
{
    static private AccountsLogic accountsLogic = new AccountsLogic();
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
        string email = Console.ReadLine();
        Console.Write("Voer alstublieft uw wachtwoord in: ");
        string password = Console.ReadLine();
        AccountModel acc = accountsLogic.CheckLogin(email, password);
        if (acc != null)
        {
            ColorConsole.WriteColorLine($"[Welkom terug, {acc.FullName}]", ConsoleColor.Green);
            Console.WriteLine($"Uw e-mailadres is: {acc.EmailAddress}");
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