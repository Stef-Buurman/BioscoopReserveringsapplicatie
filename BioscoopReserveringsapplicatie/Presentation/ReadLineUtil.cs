namespace BioscoopReserveringsapplicatie
{
    public static class ReadLineUtil
    {
        public static string EditValue(string defaultValue, Action actionBeforeStartGotten, Action escapeAction, string textToShowEscapability = "Klik op escape om dit onderdeel te verlaten.")
        {
            string input = defaultValue;
            int originalPosX = Console.CursorLeft;
            Console.Clear();
            Action actionBeforeStart = () =>
            {
                ColorConsole.WriteColorLine($"[{textToShowEscapability}]", ConsoleColor.Gray);
                actionBeforeStartGotten();
            };
            actionBeforeStart();
            ColorConsole.WriteColor($"[{defaultValue}]", Globals.ColorEditInput);

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
                    ColorConsole.WriteColor($"[{key.KeyChar}]", Globals.ColorEditInput);
                }
            }
            return input;
        }

        public static string EnterValue(Action actionBeforeStartGotten, Action escapeAction, bool isEscapable = true, string textToShowEscapability = "Klik op escape om dit onderdeel te verlaten.")
        {
            string input = "";
            int originalPosX = Console.CursorLeft;
            Console.Clear();
            Action actionBeforeStart;
            if (isEscapable)
            {
                actionBeforeStart = () =>
                {
                    ColorConsole.WriteColorLine($"[{textToShowEscapability}]", ConsoleColor.Gray);
                    actionBeforeStartGotten();
                };
            }
            else
            {
                actionBeforeStart = actionBeforeStartGotten;
            }
            actionBeforeStart();

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape && isEscapable)
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

        public static string EnterValue(bool isEscapable, Action actionBeforeStart, Action escapeAction = null)
        {
            return EnterValue(actionBeforeStart, escapeAction, isEscapable);
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
            SelectionMenu.Create(options, print);
            return WantToLeave;
        }

        public static bool EscapeKeyPressed(Action actionBeforeStart, Action escapeAction, Action whenNoPressed)
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
                                whenNoPressed();
                                WantToLeave = false;
                            }
                        ),
                    };
            SelectionMenu.Create(options, print, false);
            return WantToLeave;
        }
        private static void print()
        {
            ColorConsole.WriteColorLine("[Weet je zeker dat je weg wilt gaan?]", ConsoleColor.Red);
        }
    }
}