namespace BioscoopReserveringsapplicatie
{
    public class Option<T> : IEquatable<Option<T>>
    {
        public T Value { get; }
        public string Name { get; }
        public Action Selected { get; }

        public bool IsSelected { get; }

        public Option(T value, string name, Action selected) : this(value, name)
        {
            Selected = selected;
        }
        public Option(T value, string name)
        {
            Name = name;
            Value = value;
        }
        public Option(T name, Action selected) : this(name)
        {
            Selected = selected;
        }
        public Option(T name)
        {
            Name = name.ToString();
            Value = name;
        }

        public void Select()
        {
            Selected?.Invoke();
        }

        public bool Equals(Option<T> other)
        {
            return Value.Equals(other.Value);
        }
    }
}