public static class Globals
{
    static string CurrentDirectoryDevelop = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
    static string CurrentDirectoryProduction = Environment.CurrentDirectory;
    public static string currentDirectory = CurrentDirectoryProduction;

    public static readonly ConsoleColor TitleColor = ConsoleColor.Cyan;
    public static readonly ConsoleColor ColorInputcClarification = ConsoleColor.Blue;
}

