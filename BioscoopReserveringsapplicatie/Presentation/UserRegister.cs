using Spectre.Console;
static class UserRegister
{
    static private AccountsLogic accountsLogic = new AccountsLogic();

    public static void Start()
    {
        Console.Clear();
        AnsiConsole.Markup("[blue][bold]registratiepagina[/][/]\n\n");
        String userName = AnsiConsole.Prompt(
            new TextPrompt<string>("[blue]Naam:[/] ")
        );
        String userEmail = AnsiConsole.Prompt(
            new TextPrompt<string>("[blue]Email:[/] ")
        );
        String userPassword = AnsiConsole.Prompt(
            new TextPrompt<string>("[blue]Wachtwoord:[/] ").Secret()
        );

        accountsLogic.RegisterNewUser(userName, userEmail, userPassword);
    }
}