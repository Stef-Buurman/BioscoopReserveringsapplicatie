namespace BioscoopReserveringsapplicatie
{
    public static class ReadLineUtil
    {
        public static string EditValue(string defaultValue, Action actionBeforeStartGotten, Action escapeAction, string textToShowEscapability = "*Klik op escape om dit onderdeel te verlaten*\n")
        {
            string input = defaultValue;
            int originalPosX = Console.CursorLeft;
            Console.Clear();
            Action actionBeforeStart = () =>
            {
                ColorConsole.WriteLineInfo(textToShowEscapability);
                actionBeforeStartGotten();
            };
            actionBeforeStart();
            ColorConsole.WriteColor($"[{defaultValue}]", Globals.ColorEditInput);

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape)
                {
                    if (EscapeKeyPressed(actionBeforeStart, escapeAction, input, "edit")) break;
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

        public static string EnterValue(Action actionBeforeStartGotten, Action escapeAction, bool isEscapable = true, bool mask = false, string textToShowEscapability = "*Klik op escape om dit onderdeel te verlaten*\n")
        {
            Console.Clear();

            Action actionBeforeStart;
            if (isEscapable)
            {
                actionBeforeStart = () =>
                {
                    ColorConsole.WriteLineInfo(textToShowEscapability);
                    actionBeforeStartGotten();
                };
            }
            else
            {
                actionBeforeStart = actionBeforeStartGotten;
            }
            actionBeforeStart();

            int originalPosX = Console.CursorLeft;
            string input = "";
            int cursorPosition = 0;

            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape && isEscapable)
                {
                    if (EscapeKeyPressed(actionBeforeStart, escapeAction, input, "enter")) break;
                }
                else if (key.Key == ConsoleKey.Enter)
                {
                    Console.WriteLine();
                    break;
                }
                else if (key.Key == ConsoleKey.Backspace)
                {
                    if (cursorPosition > 0)
                    {
                        input = input.Remove(cursorPosition - 1, 1);
                        cursorPosition--;
                        Console.SetCursorPosition(originalPosX, Console.CursorTop);
                        Console.Write(input + new string(' ', Console.WindowWidth - input.Length - originalPosX));
                        Console.SetCursorPosition(originalPosX + cursorPosition, Console.CursorTop);
                    }
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (cursorPosition > 0)
                    {
                        cursorPosition--;
                        Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    }
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    if (cursorPosition < input.Length)
                    {
                        cursorPosition++;
                        Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                    }
                }
                else if (!char.IsControl(key.KeyChar) && mask == false)
                {
                    input = input.Insert(cursorPosition, key.KeyChar.ToString());
                    cursorPosition++;
                    Console.Write(key.KeyChar + input.Substring(cursorPosition));
                    Console.SetCursorPosition(Console.CursorLeft - (input.Length - cursorPosition), Console.CursorTop);
                }
                else if (!char.IsControl(key.KeyChar) && mask == true)
                {
                    input = input.Insert(cursorPosition, key.KeyChar.ToString());
                    cursorPosition++;
                    Console.Write("*" + input.Substring(cursorPosition));
                    Console.SetCursorPosition(Console.CursorLeft - (input.Length - cursorPosition), Console.CursorTop);
                }
            }

            return input;
        }

        public static string EnterValue(bool isEscapable, Action actionBeforeStart, Action escapeAction = null, bool mask = false)
        {
            return EnterValue(actionBeforeStart, escapeAction, isEscapable, mask);
        }

        public static bool EscapeKeyPressed(Action actionBeforeStart, Action escapeAction, string input, string type)
        {
            bool WantToLeave = false;
            List<Option<string>> options = new List<Option<string>>
                    {
                        new Option<string>("Ja", () => {
                                Console.WriteLine("\nTeruggaan...");
                                Thread.Sleep(1000);
                                if(escapeAction != null) escapeAction();
                                WantToLeave = true;
                            }
                        ),
                        new Option<string>("Nee", () => {
                                Console.Clear();
                                actionBeforeStart();
                                if(type == "enter")
                                {
                                    Console.Write(input);
                                }
                                else if (type == "edit")
                                {
                                    ColorConsole.WriteColor($"[{input}]", Globals.ColorEditInput);
                                }
                                WantToLeave = false;
                            }
                        ),
                    };
            SelectionMenuUtil.Create(options, print);
            return WantToLeave;
        }

        public static bool EscapeKeyPressed(Action actionBeforeStart, Action escapeAction, Action whenNoPressed)
        {
            bool WantToLeave = false;
            List<Option<string>> options = new List<Option<string>>
                    {
                        new Option<string>("Ja", () => {
                                Console.WriteLine("\nTeruggaan...");
                                Thread.Sleep(1000);
                                if(escapeAction != null) escapeAction();
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
            SelectionMenuUtil.Create(options, print, false);
            return WantToLeave;
        }
        private static void print()
        {
            ColorConsole.WriteColorLine("Weet je zeker dat je terug wilt gaan?", ConsoleColor.Red);
        }
    }
}