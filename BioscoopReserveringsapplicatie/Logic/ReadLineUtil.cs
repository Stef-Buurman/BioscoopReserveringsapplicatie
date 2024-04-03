namespace BioscoopReserveringsapplicatie
{
    public static class ReadLineUtil
    {
        public static string EditValue(string defaultValue, Action escapeAction)
        {
            string input = defaultValue;
            int originalPosX = Console.CursorLeft;
            Console.Write(defaultValue);

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\nEscape-toets ingedrukt. Afsluiten...");
                    Thread.Sleep(2000);
                    escapeAction();
                    break;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0 && Console.CursorLeft > originalPosX)
                    {
                        input = input.Substring(0, input.Length - 1);
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            return input;
        }

        public static string EnterValue(Action escapeAction)
        {
            string input = "";
            int originalPosX = Console.CursorLeft;

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    Console.WriteLine("\nEscape-toets ingedrukt. Afsluiten...");
                    Thread.Sleep(1000);
                    escapeAction();
                    break;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (input.Length > 0 && Console.CursorLeft > originalPosX)
                    {
                        input = input.Substring(0, input.Length - 1);
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                        Console.Write(" ");
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    }
                }
                else if (!char.IsControl(key.KeyChar))
                {
                    input += key.KeyChar;
                    Console.Write(key.KeyChar);
                }
            }
            return input;
        }
    }
}