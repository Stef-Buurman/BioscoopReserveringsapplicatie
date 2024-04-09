namespace BioscoopReserveringsapplicatie
{
    public static class IdGenerator
    {
        public static int GetNextId<T>(List<T> items) where T : IID => items.Count > 0 ? items.Max(item => item.Id) + 1 : 1;
    }
}