namespace BioscoopReserveringsapplicatie
{
    public static class Globals
    {
        static string CurrentDirectoryDevelop = System.IO.Path.GetFullPath(System.IO.Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.Parent.FullName, @"BioscoopReserveringsapplicatie"));
        static string CurrentDirectoryProduction = Environment.CurrentDirectory;
        public static string currentDirectory = CurrentDirectoryDevelop;

        public static readonly ConsoleColor TitleColor = ConsoleColor.Cyan;
        public static readonly ConsoleColor ColorInputcClarification = ConsoleColor.Blue;

        public static List<Genre> GetAllGenres()
        {
            List<Genre> availableGenres = new List<Genre>();
            foreach (Genre genre in Enum.GetValues(typeof(Genre)))
            {
                availableGenres.Add(genre);
            }
            return availableGenres;
        }

        public static T ParseEnum<T>(string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }
    }
}