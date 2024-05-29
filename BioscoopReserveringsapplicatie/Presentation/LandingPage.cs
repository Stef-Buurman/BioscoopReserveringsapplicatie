using System;
using System.Threading;
namespace BioscoopReserveringsapplicatie
{
    static class LandingPage
    {
        public static void Start()
        {
            Console.Clear();

            ColorConsole.WriteLineInfoHighlight("*Gebruik [fullscreen] voor de beste ervaring*\n", Globals.ColorInputcClarification);

            ColorConsole.WriteColorLine(@"[Blue] _______  __[/]                [Red]_                     [/]");
            ColorConsole.WriteColorLine(@"[Blue]|  ___\ \/ /[/][Red]_ __   ___ _ __(_) ___ _ __   ___ ___ [/]");
            ColorConsole.WriteColorLine(@"[Blue]| |_   \  /[/][Red]| '_ \ / _ \ '__| |/ _ \ '_ \ / __/ _ \[/]");
            ColorConsole.WriteColorLine(@"[Blue]|  _|  /  \[/][Red]| |_) |  __/ |  | |  __/ | | | (_|  __/[/]");
            ColorConsole.WriteColorLine(@"[Blue]|_|   /_/\_\[/][Red] .__/ \___|_|  |_|\___|_| |_|\___\___|[/]");
            ColorConsole.WriteColorLine(@"[Red]           |_|[/]                                    ");

            ColorConsole.WriteColorLine("\nWelkom bij [Blue]FX[/][Red]perience[/]!\n");
            Console.WriteLine("Wat wil je doen?\n");

            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Inloggen", () => UserLogin.Start()),
                new Option<string>("Registreren", () => UserRegister.Start()),
                new Option<string>("Applicatie sluiten", () => {ColorConsole.WriteColorLine("\nTot ziens!", Globals.SuccessColor); Environment.Exit(0);}),
            };

            new SelectionMenuUtil<string>(options).Create();

        }
    }
}

