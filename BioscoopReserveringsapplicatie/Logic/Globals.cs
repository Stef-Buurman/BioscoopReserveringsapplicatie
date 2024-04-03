namespace BioscoopReserveringsapplicatie
{
    public static class Globals
    {
        static string CurrentDirectoryDevelop = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"BioscoopReserveringsapplicatie"));
        static string CurrentDirectoryProduction = Environment.CurrentDirectory;
        public static string currentDirectory = CurrentDirectoryDevelop;
        public static readonly ConsoleColor TitleColor = ConsoleColor.Cyan;
        public static readonly ConsoleColor ColorInputcClarification = ConsoleColor.Blue;

        public static List<T> GetAllEnum<T>()
        {
            List<T> availableT = new List<T>();
            foreach (T itemT in Enum.GetValues(typeof(T)))
            {
                if (!itemT.Equals(default(T))) availableT.Add(itemT);
            }
            return availableT;
        }
    }
}