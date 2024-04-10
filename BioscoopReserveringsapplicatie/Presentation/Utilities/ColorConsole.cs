using System.Text.RegularExpressions;

namespace BioscoopReserveringsapplicatie
{
    public static class ColorConsole
    {
        public static void WriteColorLine(string message, ConsoleColor color)
        {
            WriteColor(message, color);
            Console.WriteLine();
        }

        public static void WriteColorLine(string message)
        {
            WriteColor(message);
            Console.WriteLine();
        }
        public static void WriteColor(string message, ConsoleColor color)
        {
            string[] piecesOfMessage = Regex.Split(message, @"(\[[^\]]*\])");

        static void WriteColor(string message)
        {
            ConsoleColor originalForeColor = Console.ForegroundColor;

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
                ConsoleColor color;
                if (Enum.TryParse(colorString, true, out color))
                {
                    int endTagIndex = message.IndexOf("[/]", closeBracketIndex);
                    if (endTagIndex == -1)
                {
                        Console.ForegroundColor = originalForeColor;
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

            Console.ForegroundColor = originalForeColor;
            Console.Write(message.Substring(startIndex));
        }

        public static void WriteLineInfo(string message) => WriteColorLine($"[{message}]", ConsoleColor.DarkGray);
    }
}