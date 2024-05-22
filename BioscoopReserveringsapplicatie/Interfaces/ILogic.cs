namespace BioscoopReserveringsapplicatie
{
    public interface ILogic<T>
    {
        IDataAccess<T> _DataAccess { get; }
        int GetNextId();
        void UpdateList(T location);
        public List<T> GetAll();
        public T? GetById(int id);
        public bool Add(T item);
        public bool Edit(T item);
        public bool Validate(T item);
    }
}
