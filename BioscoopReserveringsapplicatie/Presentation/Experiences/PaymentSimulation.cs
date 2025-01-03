namespace BioscoopReserveringsapplicatie
{
    static class PaymentSimulation
    {
        public static void Start(int amountOfSeats)
        {
            double price = Math.Round(Globals.pricePerSeat * amountOfSeats, 2);

            Console.WriteLine();

            HorizontalLine.Print();

            ColorConsole.WriteColorLine($"\nTotale prijs: [€ {price}] ", ConsoleColor.Green);

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
            string list = new SelectionMenuUtil<string>(options).Create();

            return;
        }
        public static void Simulation()
        {
            int progress = 0;

            Console.Clear();

            HorizontalLine.Print();
            ColorConsole.WriteColorLine("Betaling gestart", ConsoleColor.Green);
            HorizontalLine.Print();

            Console.WriteLine();

            while (progress <= 100)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"{progress}% [{new string('=', progress / 10)}]");
                WaitUtil.WaitTime(500);
                progress += 10;
            }

            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine($"100% [{new string('=', 10)}]");
            Console.WriteLine();
            HorizontalLine.Print();
            ColorConsole.WriteColorLine("Betaling geslaagd!", ConsoleColor.Green);
            HorizontalLine.Print();
            WaitUtil.WaitTime(2000);
        }
    }
}