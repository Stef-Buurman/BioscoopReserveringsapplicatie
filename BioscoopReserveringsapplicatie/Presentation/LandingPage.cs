using System;
using System.Threading;
namespace BioscoopReserveringsapplicatie
{
    static class LandingPage
    {
        //This shows the menu. You can call back to this method to show the menu again
        //after another presentation method is completed.
        //You could edit this to show different menus depending on the user's role
        public static void Start()
        {
            Console.Clear();
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Inloggen", () => UserLogin.Start()),
                new Option<string>("Registreren", () => UserRegister.Start()),
                new Option<string>("Applicatie sluiten", () => Environment.Exit(0)),
            };
            SelectionMenuUtil.Create(options, () => Print());

        }

        private static void Print()
        {
            ColorConsole.WriteColorLine(@" [_______  __]                _                     ",ConsoleColor.Green);
            ColorConsole.WriteColorLine(@"[|  ___\ \/ /]_ __   ___ _ __(_) ___ _ __   ___ ___ ",ConsoleColor.Green);
            ColorConsole.WriteColorLine(@"[| |_   \  /]| '_ \ / _ \ '__| |/ _ \ '_ \ / __/ _ \",ConsoleColor.Green);
            ColorConsole.WriteColorLine(@"[|  _|  /  \]| |_) |  __/ |  | |  __/ | | | (_|  __/",ConsoleColor.Green);
            ColorConsole.WriteColorLine(@"[|_|   /_/\_\] .__/ \___|_|  |_|\___|_| |_|\___\___|",ConsoleColor.Green);
            ColorConsole.WriteColorLine(@"           |_|                                    ");

            Console.WriteLine("\nWelkom bij FXperience!\n");
            Console.WriteLine("Wat wil je doen?\n");
        }
    }
}

