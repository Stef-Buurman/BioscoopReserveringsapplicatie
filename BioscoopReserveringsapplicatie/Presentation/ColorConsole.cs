﻿using System.Text.RegularExpressions;

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
            string messagePiece = piecesOfMessage[i];

            if (messagePiece.StartsWith("[") && messagePiece.EndsWith("]"))
            {
                Console.ForegroundColor = color;
                messagePiece = messagePiece.Substring(1, messagePiece.Length - 2);
            }

            Console.Write(messagePiece);
            Console.ResetColor();
        }
    }
}