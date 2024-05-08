namespace BioscoopReserveringsapplicatie
{
    public class KeyAction
    {
        public ConsoleKey Key { get; }
        public Action Action { get; }

        public KeyAction(ConsoleKey key, Action action)
        {
            Key = key;
            Action = action;
        }
    }
}