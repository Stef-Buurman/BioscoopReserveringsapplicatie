namespace BioscoopReserveringsapplicatie
{
    public static class ReadLineUtil
    {
        public static string EditValue(string defaultValue, Action actionBeforeStart, Action escapeAction)
        {
            string input = defaultValue;
            int originalPosX = Console.CursorLeft;
            Console.Clear();
            actionBeforeStart();
            Console.Write(defaultValue);

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    if (EscapeKeyPressed(actionBeforeStart, escapeAction, input)) break;
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

        public static string EnterValue(Action actionBeforeStart, Action escapeAction)
        {
            string input = "";
            int originalPosX = Console.CursorLeft;
            Console.Clear();
            actionBeforeStart();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    if (EscapeKeyPressed(actionBeforeStart, escapeAction, input)) break;
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

        public static bool EscapeKeyPressed(Action actionBeforeStart, Action escapeAction, string input)
        {
            bool WantToLeave = false;
            List<Option<string>> options = new List<Option<string>>
                    {
                        new Option<string>("Ja", () => {
                                Console.WriteLine("\nEscape-toets ingedrukt. Afsluiten...");
                                Thread.Sleep(2000);
                                escapeAction();
                                WantToLeave = true;
                            }
                        ),
                        new Option<string>("Nee", () => {
                                Console.Clear();
                                actionBeforeStart();
                                Console.Write(input);
                                WantToLeave = false;
                            }
                        ),
                    };
            SelectionMenu.Create(options, () => Console.WriteLine("Weet je zeker dat je weg wilt gaan?"));
            return WantToLeave;
        }
    }
}