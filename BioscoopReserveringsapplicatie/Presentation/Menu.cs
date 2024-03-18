using Spectre.Console;

static class Menu
{

    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.Clear();
        string choice = AnsiConsole.Prompt(
            new SelectionPrompt<string>()
            .Title("")
            .AddChoices(new[] {
                "Login", "Registeren", 
            })
        );

        if (choice == "Login")
        {
            Console.WriteLine(choice);
        }
        else if (choice == "Registeren")
        {
            UserRegister.Start();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Start();
        }

    }
}