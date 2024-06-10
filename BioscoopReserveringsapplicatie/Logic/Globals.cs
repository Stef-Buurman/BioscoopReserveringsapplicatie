namespace BioscoopReserveringsapplicatie
{
    public static class Globals
    {
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
        public static readonly ConsoleColor LocationColor = ConsoleColor.DarkMagenta;
        public static readonly ConsoleColor UserColor = ConsoleColor.Cyan;
        public static readonly ConsoleColor SaveColor = ConsoleColor.Green;
        public static readonly ConsoleColor GoBackColor = ConsoleColor.Red;

        public static readonly double pricePerSeat = 19.99;


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