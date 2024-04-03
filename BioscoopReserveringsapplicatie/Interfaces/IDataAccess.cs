namespace BioscoopReserveringsapplicatie
{
    public interface IDataAccess<T>
    {
        List<T> LoadAll();
        void WriteAll(List<T> items);
    }
}
