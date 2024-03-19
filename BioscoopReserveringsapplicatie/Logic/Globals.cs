public static class Globals
{
    static string CurrentDirectoryDevelop = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
    static string CurrentDirectoryProduction = Environment.CurrentDirectory;
    public static string currentDirectory = CurrentDirectoryDevelop;
}

