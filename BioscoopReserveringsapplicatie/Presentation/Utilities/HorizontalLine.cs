namespace BioscoopReserveringsapplicatie
{
    public static class HorizontalLine
    {
        public static void Print(int length = 70, ConsoleColor color = ConsoleColor.White)
        {
            ColorConsole.WriteColorLine(new string('-', length), color);
        }
        
    }
}
