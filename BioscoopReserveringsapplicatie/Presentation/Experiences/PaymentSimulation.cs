namespace BioscoopReserveringsapplicatie
{
    static class PaymentSimulation
    {
        public static void Start()
        {
        List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Creditcard", () => {Console.Clear(); Simulation();}),
                new Option<string>("PayPal", () => {Console.Clear(); Simulation();}),
                new Option<string>("Klarna", () => {Console.Clear(); Simulation();}),
                new Option<string>("Apple Pay", () => {Console.Clear(); Simulation();}),
                new Option<string>("iDEAL", () => {Console.Clear(); Simulation();}),
            };
            ColorConsole.WriteColorLine($"Kies een [betalingsmethode]", ConsoleColor.Green);
            string list = new SelectionMenuUtil2<string>(options).Create();
            Environment.Exit(0);
        }
        public static void Simulation()
        {
            int x = 5;
            while (x > 0)
            {
                Console.Clear();
                ColorConsole.WriteColorLine("--------------------------------------------------------------------------------", ConsoleColor.Gray);
                ColorConsole.WriteColorLine($"Betaling wordt verwerkt nog {x} seconden over", ConsoleColor.Green);
                ColorConsole.WriteColorLine("--------------------------------------------------------------------------------", ConsoleColor.Gray);
                Thread.Sleep(1000);
                x--;
            }
            Console.Clear();
            ColorConsole.WriteColorLine("--------------------------------------------------------------------------------", ConsoleColor.Gray);
            ColorConsole.WriteColorLine("Betaling geslaagd!", ConsoleColor.Green);
            ColorConsole.WriteColorLine("--------------------------------------------------------------------------------", ConsoleColor.Gray);
            Thread.Sleep(2000);
            Console.Clear();
        }
    }
}