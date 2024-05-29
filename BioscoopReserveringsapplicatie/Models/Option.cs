namespace BioscoopReserveringsapplicatie
{
    public class Option<T> : IEquatable<Option<T>>
    {
        public T Value { get; }
        public string Name { get; }
        public Action Selected { get; }
        public bool IsSelected { get; private set; }
        private ConsoleColor? _Color { get; }
        public ConsoleColor Color { get => _Color ?? Console.ForegroundColor; }

        public Option(T value, string name, Action selected, bool isSelected = false, ConsoleColor? color = null) : this(value, name, isSelected, color)
        {
            Selected = selected;
        }
        public Option(T value, string name, bool isSelected = false, ConsoleColor? color = null)
        {
            Name = name;
            Value = value;
            IsSelected = isSelected;
            _Color = color;
        }
        public Option(T name, Action selected, bool isSelected = false) : this(name, isSelected)
        {
            Selected = selected;
        }

        public Option(T name, Action selected, ConsoleColor Color, bool isSelected = false) : this(name, isSelected, Color)
        {
            Selected = selected;
        }
        public Option(T name, bool isSelected = false, ConsoleColor? color = null)
        {
            Name = name.ToString();
            Value = name;
            IsSelected = isSelected;
            _Color = color;
        }

        public void InvertSelecttion() => IsSelected = !IsSelected;

        public void SelectFunction()
        {
            Selected?.Invoke();
        }

        public bool Equals(Option<T> other)
        {
            return Value.Equals(other.Value);
        }
    }
}