﻿using System.Drawing;
using System.Text.RegularExpressions;

namespace BioscoopReserveringsapplicatie
{
    public static class ColorConsole
    {
        public static void WriteColorLine(string message, ConsoleColor color, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            WriteColor(message, color, backgroundColor);
            Console.WriteLine();
        }

        public static void WriteColorLine(string message)
        {
            WriteColor(message);
            Console.WriteLine();
        }
        public static void WriteColor(string message, ConsoleColor fontColor, ConsoleColor backgroundColor = ConsoleColor.Black)
        {
            ConsoleColor originalForeColor = Console.ForegroundColor;
            ConsoleColor originalBackgroundColor = Console.BackgroundColor;
            List<string> piecesOfMessage = new List<string>(Regex.Split(message, @"(\[[^\]]*\])"));
            if (piecesOfMessage.Count == 1)
            {
                Console.BackgroundColor = backgroundColor;
                Console.ForegroundColor = fontColor;
                Console.Write(message);
                Console.ResetColor();
            }
            else
            {
                foreach (string messagePiece in piecesOfMessage)
                {
                    if (messagePiece.StartsWith("[") && messagePiece.EndsWith("]"))
                    {
                        Console.BackgroundColor = backgroundColor;
                        Console.ForegroundColor = fontColor;
                        Console.Write(messagePiece.Substring(1, messagePiece.Length - 2));
                    }
                    else
                    {
                        Console.Write(messagePiece);
                    }
                    Console.BackgroundColor = originalBackgroundColor;
                    Console.ForegroundColor = originalForeColor;
                }
            }
        }

        public static void WriteColor(string message)
        {
            ConsoleColor originalForeColor = Console.ForegroundColor;
            ConsoleColor originalBackColor = Console.BackgroundColor;

            int startIndex = 0;
            int openBracketIndex = message.IndexOf('[');

            while (openBracketIndex != -1)
            {
                int closeBracketIndex = message.IndexOf(']', openBracketIndex);
                if (closeBracketIndex == -1)
                {
                    Console.ForegroundColor = originalForeColor;
                    Console.WriteLine(message.Substring(startIndex));
                    return;
                }

                Console.ForegroundColor = originalForeColor;
                Console.Write(message.Substring(startIndex, openBracketIndex - startIndex));

                string colorString = message.Substring(openBracketIndex + 1, closeBracketIndex - openBracketIndex - 1);
                string[] colorParts = colorString.Split(':');
                if(colorParts.Length == 1)
                {
                    ConsoleColor color;
                    if (Enum.TryParse(colorString, true, out color))
                    {
                        int endTagIndex = message.IndexOf("[/]", closeBracketIndex);
                        if (endTagIndex == -1)
                        {
                            Console.ForegroundColor = originalForeColor;
                            Console.BackgroundColor = originalBackColor;
                            Console.WriteLine(message.Substring(startIndex));
                            return;
                        }

                        WriteColor(message.Substring(closeBracketIndex + 1, endTagIndex - closeBracketIndex - 1), color);

                        startIndex = endTagIndex + 3;

                        openBracketIndex = message.IndexOf('[', startIndex);
                    }
                    else
                    {
                        Console.Write($"[{colorString}]");
                        startIndex = closeBracketIndex + 1;
                        openBracketIndex = message.IndexOf('[', startIndex);
                    }
                }
                else if(colorParts.Length == 2)
                {
                    ConsoleColor foreColor;
                    ConsoleColor backColor = ConsoleColor.Black;
                    if (Enum.TryParse(colorParts[0], true, out foreColor) && Enum.TryParse(colorParts[1], true, out backColor))
                    {
                        Console.ForegroundColor = foreColor;
                        Console.BackgroundColor = backColor;
                    }
                    else
                    {
                        Console.Write($"[{colorParts[0]}:{colorParts[1]}]");
                    }
                    int endTagIndex = message.IndexOf("[/]", closeBracketIndex);
                    if (endTagIndex == -1)
                    {
                        Console.ForegroundColor = originalForeColor;
                        Console.BackgroundColor = originalBackColor;
                        Console.WriteLine(message.Substring(startIndex));
                        return;
                    }
                    WriteColor(message.Substring(closeBracketIndex + 1, endTagIndex - closeBracketIndex - 1), foreColor, backColor);
                    startIndex = endTagIndex + 3;
                    openBracketIndex = message.IndexOf('[', startIndex);
                }
            }

            Console.ForegroundColor = originalForeColor;
            Console.BackgroundColor = originalBackColor;
            Console.Write(message.Substring(startIndex));
        }

        public static void WriteLineColor(string message)
        {
            WriteColor(message + Environment.NewLine);
        }

        public static void WriteLineInfo(string message) => WriteColorLine($"[{message}]", ConsoleColor.DarkGray);
    }
}