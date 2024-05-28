using System.Security.Cryptography.X509Certificates;
using System.Text.RegularExpressions;

namespace BioscoopReserveringsapplicatie
{
    public static class ColorConsole
    {
        public static void WriteColorLine(string message, ConsoleColor color, ConsoleColor? backgroundColor = null)
        {
            WriteColor(message, color, backgroundColor);
            Console.WriteLine();
        }

        public static void WriteColorLine(string message)
        {
            WriteColor(message);
            Console.WriteLine();
        }

        public static void WriteColor(string message, ConsoleColor fontColor, ConsoleColor? backgroundColor = null, ConsoleColor? originalForeColor = null, ConsoleColor? originalBackgroundColor = null)
        {
            ConsoleColor newOriginalForeColor = originalForeColor ?? Console.ForegroundColor;
            ConsoleColor newOriginalBackgroundColor = originalBackgroundColor ?? Console.BackgroundColor;
            ConsoleColor newBackgroundColor = backgroundColor ?? Console.BackgroundColor;

            List<string> piecesOfMessage = new List<string>(Regex.Split(message, @"(\[[^\]]*\])"));
            if (piecesOfMessage.Count == 1)
            {
                Console.BackgroundColor = newBackgroundColor;
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
                        Console.BackgroundColor = newBackgroundColor;
                        Console.ForegroundColor = fontColor;
                        Console.Write(messagePiece.Substring(1, messagePiece.Length - 2));
                    }
                    else
                    {
                        Console.Write(messagePiece);
                    }
                    Console.BackgroundColor = newOriginalBackgroundColor;
                    Console.ForegroundColor = newOriginalForeColor;
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

                        WriteColor(message.Substring(closeBracketIndex + 1, endTagIndex - closeBracketIndex - 1), color, originalForeColor: originalBackColor, originalBackgroundColor: originalBackColor);

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
                    ConsoleColor backColor = Console.BackgroundColor;
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
                    WriteColor(message.Substring(closeBracketIndex + 1, endTagIndex - closeBracketIndex - 1), foreColor, backColor, originalForeColor, originalBackColor);
                    startIndex = endTagIndex + 3;
                    openBracketIndex = message.IndexOf('[', startIndex);
                }
            }

            Console.ForegroundColor = originalForeColor;
            Console.BackgroundColor = originalBackColor;
            Console.Write(message.Substring(startIndex));
        }

        public static void WriteLineInfo(string message) => WriteColorLine($"[{message}]", ConsoleColor.DarkGray);

        public static void WriteLineInfoHighlight(string message, ConsoleColor fontColor, ConsoleColor? backgroundColor = null)
        {
            if (backgroundColor == null)
            {
                backgroundColor = Console.BackgroundColor;
            }
            ConsoleColor originalForeColor = Console.ForegroundColor;
            ConsoleColor originalBackColor = Console.BackgroundColor;
            Console.ForegroundColor = ConsoleColor.DarkGray;

            WriteColor($"{message}", fontColor, backgroundColor ?? Console.BackgroundColor, ConsoleColor.DarkGray, originalBackColor);

            Console.ForegroundColor = originalForeColor;
            Console.BackgroundColor = originalBackColor;
            Console.WriteLine();
        }
    }
}