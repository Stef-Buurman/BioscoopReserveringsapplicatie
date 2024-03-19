using System;
using System.Threading;

static class Menu
{
    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    static public void Start()
    {
        Console.Clear();
        Console.WriteLine("Welkom bij de FX reserveringsapplicatie\n\n");
        Console.WriteLine("[1]Login");
        Console.WriteLine("[2]Registeren\n");
        string choice = Console.ReadLine() ?? "";

        if (choice == "1")
        {
            Console.WriteLine(choice);
        }
        else if (choice == "2")
        {
            UserRegister.Start();
        }
        else
        {
            Console.WriteLine("Invalid input");
            Thread.Sleep(1000);
            Start();
        }

    }
}