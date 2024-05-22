using System.Reflection;

namespace BioscoopReserveringsapplicatie
{
    public static class Globals
    {
        static string CurrentDirectoryProduction = Environment.CurrentDirectory;
        public static string currentDirectory = getPath();
        public static string getPath(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return System.IO.Path.Combine(directory?.ToString() ?? "", Assembly.GetCallingAssembly().GetName().Name ?? "");
        }

        public static readonly ConsoleColor TitleColor = ConsoleColor.Magenta;
        public static readonly ConsoleColor ColorInputcClarification = ConsoleColor.Blue;
        public static readonly ConsoleColor ColorEditInput = ConsoleColor.Yellow;
        public static readonly ConsoleColor ExperienceColor = ConsoleColor.Cyan;
        public static readonly ConsoleColor MovieColor = ConsoleColor.Green;
        public static readonly ConsoleColor PromotionColor = ConsoleColor.DarkYellow;
        public static readonly ConsoleColor ReservationColor = ConsoleColor.DarkCyan;
        public static readonly ConsoleColor RoomColor = ConsoleColor.DarkMagenta;
        public static readonly ConsoleColor ErrorColor = ConsoleColor.DarkRed;
        public static readonly ConsoleColor SuccessColor = ConsoleColor.DarkGreen;

        public static List<T> GetAllEnum<T>()
        {
            List<T> availableT = new List<T>();
            foreach (T itemT in Enum.GetValues(typeof(T)))
            {
                if (!itemT.Equals(default(T))) availableT.Add(itemT);
            }
            return availableT;
        }

        public static List<T> GetAllEnumIncludeUndefined<T>() => new List<T> { default(T) }.Concat(GetAllEnum<T>()).ToList();
    }
}