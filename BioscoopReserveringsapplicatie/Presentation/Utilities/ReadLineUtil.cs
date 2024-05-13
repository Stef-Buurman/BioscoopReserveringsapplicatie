namespace BioscoopReserveringsapplicatie
{
    public static class ReadLineUtil
    {
        public static string EditValue(string defaultValue, string whatToEnterText, Action escapeAction, string textToShowEscapability = "*Klik op escape om dit onderdeel te verlaten*\n", bool mask = false, bool showEscapability = true)
        {
            bool isEscapable = escapeAction != null;

            if (showEscapability) ColorConsole.WriteLineInfo(textToShowEscapability);

            int originalPosX = Console.CursorLeft;
            string input = defaultValue;
            int cursorPosition = 0 + defaultValue.Length;
            int Top = Console.GetCursorPosition().Top;

            int textLength = whatToEnterText.Length - 2;

            ColorConsole.WriteColor(whatToEnterText, Globals.ColorInputcClarification);
            ColorConsole.WriteColor(input, Globals.ColorEditInput);
            while (true)
            {
                ConsoleKeyInfo key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.Escape && isEscapable)
                {
                    if (EscapeKeyPressed(escapeAction, input, "edit")) break;
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
                        //Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
                    }
                }
                else if (key.Key == ConsoleKey.RightArrow)
                {
                    if (cursorPosition < input.Length)
                    {
                        cursorPosition++;
                        //Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
                    }
                }
                else if (!char.IsControl(key.KeyChar) && mask == false)
                {
                    input = input.Insert(cursorPosition, key.KeyChar.ToString());
                    cursorPosition++;
                    Console.Write(key.KeyChar + input.Substring(cursorPosition));
                    //Console.SetCursorPosition(Console.CursorLeft - (input.Length - cursorPosition) + whatToEnterText.Length, Console.CursorTop);
                }
                else if (!char.IsControl(key.KeyChar) && mask == true)
                {
                    input = input.Insert(cursorPosition, key.KeyChar.ToString());
                    cursorPosition++;
                    Console.Write("*" + input.Substring(cursorPosition));
                    //Console.SetCursorPosition(Console.CursorLeft - (input.Length - cursorPosition) + whatToEnterText.Length, Console.CursorTop);
                }
                Console.CursorVisible = false;
                Console.SetCursorPosition(originalPosX, Console.CursorTop);
                ColorConsole.WriteColor(whatToEnterText, Globals.ColorInputcClarification);
                //ColorConsole.WriteColor(input, Globals.ColorEditInput);
                if (mask)
                    Console.Write(new string('*', input.Length) + new string(' ', Console.WindowWidth - input.Length - originalPosX));
                else
                    ColorConsole.WriteColor(input + new string(' ', Console.WindowWidth - input.Length - originalPosX), Globals.ColorEditInput);
                Console.CursorVisible = true;
                Console.SetCursorPosition(originalPosX + cursorPosition + textLength, Top);
            }
            return input;
        }

        public static string EnterValue(string whatToEnterText, Action escapeAction, bool mask = false, bool showEscapability = true, string textToShowEscapability = "*Klik op escape om dit onderdeel te verlaten*\n")
        {
            return EditValue("", whatToEnterText, escapeAction, textToShowEscapability, mask, showEscapability);
            //Console.Clear();

            //Action actionBeforeStart;
            //if (isEscapable)
            //{
            //    actionBeforeStart = () =>
            //    {
            //        ColorConsole.WriteLineInfo(textToShowEscapability);
            //        actionBeforeStartGotten();
            //    };
            //}
            //else
            //{
            //    actionBeforeStart = actionBeforeStartGotten;
            //}
            //actionBeforeStart();

            //int originalPosX = Console.CursorLeft;
            //string input = "";
            //int cursorPosition = 0;

            //while (true)
            //{
            //    ConsoleKeyInfo key = Console.ReadKey(true);
            //    if (key.Key == ConsoleKey.Escape && isEscapable)
            //    {
            //        if (EscapeKeyPressed(actionBeforeStart, escapeAction, input, "enter")) break;
            //    }
            //    else if (key.Key == ConsoleKey.Enter)
            //    {
            //        Console.WriteLine();
            //        break;
            //    }
            //    else if (key.Key == ConsoleKey.Backspace)
            //    {
            //        if (cursorPosition > 0)
            //        {
            //            input = input.Remove(cursorPosition - 1, 1);
            //            cursorPosition--;
            //        }
            //    }
            //    else if (key.Key == ConsoleKey.LeftArrow)
            //    {
            //        if (cursorPosition > 0)
            //        {
            //            cursorPosition--;
            //            Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            //        }
            //    }
            //    else if (key.Key == ConsoleKey.RightArrow)
            //    {
            //        if (cursorPosition < input.Length)
            //        {
            //            cursorPosition++;
            //            Console.SetCursorPosition(Console.CursorLeft + 1, Console.CursorTop);
            //        }
            //    }
            //    else if (!char.IsControl(key.KeyChar) && mask == false)
            //    {
            //        input = input.Insert(cursorPosition, key.KeyChar.ToString());
            //        cursorPosition++;
            //        Console.Write(key.KeyChar + input.Substring(cursorPosition));
            //        Console.SetCursorPosition(Console.CursorLeft - (input.Length - cursorPosition), Console.CursorTop);
            //    }
            //    else if (!char.IsControl(key.KeyChar) && mask == true)
            //    {
            //        input = input.Insert(cursorPosition, key.KeyChar.ToString());
            //        cursorPosition++;
            //        Console.Write("*" + input.Substring(cursorPosition));
            //        Console.SetCursorPosition(Console.CursorLeft - (input.Length - cursorPosition), Console.CursorTop);
            //    }

            //    Console.SetCursorPosition(originalPosX, Console.CursorTop);
            //    if (mask)
            //        Console.Write(new string('*', input.Length) + new string(' ', Console.WindowWidth - input.Length - originalPosX));
            //    else
            //        Console.Write(input + new string(' ', Console.WindowWidth - input.Length - originalPosX));
            //    Console.SetCursorPosition(originalPosX + cursorPosition, Console.CursorTop);
            //}

            //return input;
        }

        //public static string EnterValue(string whatToEnterText, Action escapeAction = null, bool mask = false, bool showEscapability = true)
        //{
        //    return EnterValue(whatToEnterText, escapeAction, mask, showEscapability);
        //}

        public static bool EscapeKeyPressed(Action escapeAction, string input, string type)
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
            ColorConsole.WriteColorLine("\n----------------------------------------------------------------", Globals.ErrorColor);
            ColorConsole.WriteColorLine("Weet je zeker dat je terug wilt gaan?", Globals.ErrorColor);
            new SelectionMenuUtil2<string>(options).Create();
            return WantToLeave;
        }

        public static bool EscapeKeyPressed(Action escapeAction, Action whenNoPressed)
        {
            bool WantToLeave = false;
            List<Option<string>> options = new List<Option<string>>
                    {
                        new Option<string>("Ja", () => {
                                Console.WriteLine("\nTeruggaan...");
                                Thread.Sleep(1000);
                                if(escapeAction != null) escapeAction();
                                WantToLeave = true;
                                Console.Clear();
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
            ColorConsole.WriteColorLine("Weet je zeker dat je terug wilt gaan?", ConsoleColor.Red);
            new SelectionMenuUtil2<string>(options, false).Create();
            return WantToLeave;
        }
    }
}