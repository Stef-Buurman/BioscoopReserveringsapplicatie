namespace BioscoopReserveringsapplicatie
{
    public static class ReadLineUtil
    {
        public static int Top = 0;
        public static string EditValue(string defaultValue, string whatToEnterText, Action escapeAction, string textToShowEscapability = "*Klik op [Escape] om terug te gaan*\n", bool mask = false, bool showEscapability = true)
        {
            bool isEscapable = escapeAction != null;

            if (showEscapability) ColorConsole.WriteLineInfoHighlight(textToShowEscapability, Globals.ColorInputcClarification);

            int originalPosX = Console.CursorLeft;
            string input = defaultValue;
            int cursorPosition = 0 + defaultValue.Length;
            Top = Console.GetCursorPosition().Top;

            int textLength = whatToEnterText.Length - 2;

            ColorConsole.WriteColor(whatToEnterText, Globals.ColorInputcClarification);
            ColorConsole.WriteColor(input, Globals.ColorEditInput);
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape && isEscapable)
                {
                    if (EscapeKeyPressed(escapeAction)) break;
                    Console.SetCursorPosition(Console.CursorLeft, Top);
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
                    }
                }
                else if (key.Key == ConsoleKey.LeftArrow)
                {
                    if (cursorPosition > 0)
                    {
                        cursorPosition--;
                    }
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    if (cursorPosition < input.Length)
                    {
                        cursorPosition++;
                    }
                }
                else if (!char.IsControl(key.KeyChar) && mask == false)
                {
                    input = input.Insert(cursorPosition, key.KeyChar.ToString());
                    cursorPosition++;
                }
                else if (!char.IsControl(key.KeyChar) && mask == true)
                {
                    input = input.Insert(cursorPosition, key.KeyChar.ToString());
                    cursorPosition++;
                }

                Console.CursorVisible = false;
                Console.SetCursorPosition(originalPosX, Top);
                Console.Write(new string(' ', Console.WindowWidth * (Console.WindowHeight - Top)));
                Console.SetCursorPosition(originalPosX, Top);
                ColorConsole.WriteColor(whatToEnterText, Globals.ColorInputcClarification);

                if (mask)
                    ColorConsole.WriteColor(new string('*', input.Length), Globals.ColorEditInput);
                else
                    ColorConsole.WriteColor(input, Globals.ColorEditInput);

                Console.CursorVisible = true;
                int currentLine = (originalPosX + cursorPosition + textLength) / Console.WindowWidth;
                int currentColumn = (originalPosX + cursorPosition + textLength) % Console.WindowWidth;
                Console.SetCursorPosition(currentColumn, Top + currentLine);
            }
            return input;
        }

        public static string EnterValue(string whatToEnterText, Action escapeAction, bool mask = false, bool showEscapability = true, string textToShowEscapability = "*Klik op [Escape] om terug te gaan*\n")
        {
            return EditValue("", whatToEnterText, escapeAction, textToShowEscapability, mask, showEscapability);
        }

        public static bool EscapeKeyPressed(Action escapeAction)
        {
            Console.SetCursorPosition(Console.CursorLeft, Top);
            bool WantToLeave = false;
            string Line = "\n\n----------------------------------------------------------------";
            string Message = "Weet je zeker dat je terug wilt gaan?";
            List<Option<string>> options = new List<Option<string>>
                    {
                        new Option<string>("Ja", () => {
                                Console.WriteLine("\nTeruggaan...");
                                WaitUtil.WaitTime(1000);
                                if(escapeAction != null) escapeAction();
                                WantToLeave = true;
                            }
                        ),
                        new Option<string>("Nee", () => {
                                int cursorPosition = Console.GetCursorPosition().Top;
                                Console.SetCursorPosition(Console.CursorLeft, Top);
                                for (int i = 0; i < (cursorPosition - Top); i++)
                                {
                                    string x = "";
                                    for(int j = 0; j < Line.Length; j++) x += " ";
                                    Console.WriteLine(x);
                                }
                                WantToLeave = false;
                            }
                        ),
                    };
            ColorConsole.WriteColorLine(Line, Globals.ErrorColor);
            ColorConsole.WriteColorLine(Message, Globals.ErrorColor);
            new SelectionMenuUtil<string>(options, new Option<string>("Nee")).Create();
            return WantToLeave;
        }

        public static bool EscapeKeyPressed(Action escapeAction, Action whenNoPressed, string message = "Weet je zeker dat je terug wilt gaan?")
        {
            bool WantToLeave = false;
            List<Option<string>> options = new List<Option<string>>
                    {
                        new Option<string>("Ja", () => {
                                Console.WriteLine("\nTeruggaan...");
                                WaitUtil.WaitTime(1000);
                                if(escapeAction != null) escapeAction();
                                WantToLeave = true;
                            }
                        ),
                        new Option<string>("Nee", () => {
                                Console.Clear();
                                whenNoPressed();
                                WantToLeave = false;
                            }
                        ),
                    };
            ColorConsole.WriteColorLine("\n----------------------------------------------------------------", ConsoleColor.Red);
            ColorConsole.WriteColorLine(message, ConsoleColor.Red);
            new SelectionMenuUtil<string>(options, false, new Option<string>("Nee")).Create();
            return WantToLeave;
        }
    }
}