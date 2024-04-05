using System.Reflection;

namespace BioscoopReserveringsapplicatie
{
    public static class Globals
    {
        //static string CurrentDirectoryDevelop = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"BioscoopReserveringsapplicatie"));
        //static string CurrentDirectoryProduction = Environment.CurrentDirectory;
        //public static string currentDirectory = CurrentDirectoryDevelop;

        public static string currentDirectory = getPath();
        public static string TryGetSolutionDirectoryInfo(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return directory?.ToString() ?? "";
        }

        private static string getPath()
        {
            string projectName = Assembly.GetCallingAssembly().GetName().Name;
            return System.IO.Path.Combine(TryGetSolutionDirectoryInfo(), projectName);
        }

        public static readonly ConsoleColor TitleColor = ConsoleColor.Cyan;
        public static readonly ConsoleColor ColorInputcClarification = ConsoleColor.Blue;
        public static readonly ConsoleColor ColorEditInput = ConsoleColor.Yellow;

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