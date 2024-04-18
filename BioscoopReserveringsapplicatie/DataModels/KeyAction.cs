namespace BioscoopReserveringsapplicatie
{
    public class KeyAction : IEquatable<KeyAction>
    {
        public ConsoleKey Key { get; }
        public Action Action { get; }

        public KeyAction(ConsoleKey key, Action action)
        {
            Key = key;
            Action = action;
        }

        public bool Equals(KeyAction other) => Key == other.Key;
    }
}