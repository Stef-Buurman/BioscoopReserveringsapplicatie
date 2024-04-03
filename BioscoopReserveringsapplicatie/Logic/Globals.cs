namespace BioscoopReserveringsapplicatie
{
    public static class Globals
    {
        static string CurrentDirectoryDevelop = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"BioscoopReserveringsapplicatie"));
        static string CurrentDirectoryProduction = Environment.CurrentDirectory;
        public static string currentDirectory = CurrentDirectoryProduction;
        public static readonly ConsoleColor TitleColor = ConsoleColor.Cyan;
        public static readonly ConsoleColor ColorInputcClarification = ConsoleColor.Blue;
    }
}