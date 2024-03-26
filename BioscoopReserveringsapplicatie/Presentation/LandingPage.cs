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
            SelectionMenu.Create(options, () => Print());

        }

        private static void Print()
        {
            Console.WriteLine(@" _______  __                _                     ");
            Console.WriteLine(@"|  ___\ \/ /_ __   ___ _ __(_) ___ _ __   ___ ___ ");
            Console.WriteLine(@"| |_   \  /| '_ \ / _ \ '__| |/ _ \ '_ \ / __/ _ \");
            Console.WriteLine(@"|  _|  /  \| |_) |  __/ |  | |  __/ | | | (_|  __/");
            Console.WriteLine(@"|_|   /_/\_\ .__/ \___|_|  |_|\___|_| |_|\___\___|");
            Console.WriteLine(@"           |_|                                    ");

            Console.WriteLine("\nWelkom bij FXperience!\n");
            Console.WriteLine("Wat wil je doen?\n");
        }
    }
}

