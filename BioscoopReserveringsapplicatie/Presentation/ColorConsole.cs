using System.Text.RegularExpressions;

public static class ColorConsole 
{
    public static void WriteColorLine(string message, ConsoleColor color)
    {
        WriteColor(message, color);
        Console.WriteLine();
    }
    public static void WriteColor(string message, ConsoleColor color)
    {
        var piecesOfMessage = Regex.Split(message, @"(\[[^\]]*\])");

        for (int i = 0; i < piecesOfMessage.Length; i++)
        {
            string messagePeace = piecesOfMessage[i];

            if (messagePeace.StartsWith("[") && messagePeace.EndsWith("]"))
            {
                Console.ForegroundColor = color;
                messagePeace = messagePeace.Substring(1, messagePeace.Length - 2);
            }

            Console.Write(messagePeace);
            Console.ResetColor();
        }
    }
}