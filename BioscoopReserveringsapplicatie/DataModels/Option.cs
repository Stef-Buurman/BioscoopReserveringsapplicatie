namespace BioscoopReserveringsapplicatie
{
    public class Option<T> : IEquatable<Option<T>>
    {
        public T Value { get; }
        public string Name { get; }
        public Action Selected { get; }
        public bool IsSelected { get; private set; }

        public Option(T value, string name, Action selected, bool isSelected = false) : this(value, name, isSelected)
        {
            Selected = selected;
        }
        public Option(T value, string name, bool isSelected = false)
        {
            Name = name;
            Value = value;
            IsSelected = isSelected;
        }
        public Option(T name, Action selected, bool isSelected = false) : this(name, isSelected)
        {
            Selected = selected;
        }
        public Option(T name, bool isSelected = false)
        {
            Name = name.ToString();
            Value = name;
            IsSelected = isSelected;
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