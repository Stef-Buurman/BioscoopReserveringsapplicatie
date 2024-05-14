namespace BioscoopReserveringsapplicatie
{
    static class PaymentSimulation
    {
        public static void Start()
        {
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("iDEAL", Simulation),
                new Option<string>("Creditcard", Simulation),
                new Option<string>("PayPal", Simulation),
                new Option<string>("Klarna", Simulation),
                new Option<string>("Apple Pay", Simulation),
                new Option<string>("Paysafecard", Simulation),

            };
            ColorConsole.WriteColorLine($"\nKies een [betalingsmethode]", ConsoleColor.Green);
            string list = new SelectionMenuUtil2<string>(options).Create();

            return;
        }
        public static void Simulation()
        {
            int progress = 0;

            Console.WriteLine();

            while (progress <= 100)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Voortgang: {progress}% [{new string('=', progress / 10)}]");
                Thread.Sleep(500);
                progress += 10;
            }

            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine($"Voortgang: 100% [{new string('=', 10)}]");
            Console.WriteLine();
            ColorConsole.WriteColorLine("--------------------------------------------------------------------------------", ConsoleColor.Gray);
            ColorConsole.WriteColorLine("Betaling geslaagd!", ConsoleColor.Green);
            ColorConsole.WriteColorLine("--------------------------------------------------------------------------------", ConsoleColor.Gray);
            Thread.Sleep(2000);
        }
    }
}