namespace BioscoopReserveringsapplicatie
{
    public static class IdGenerator
    {
        public static int GetNextId<T>(List<T> items) where T : IID
        {
            if (items.Count > 0)
            {
                return items.Max(item => item.Id) + 1;
            }
            else
            {
                return 1;
            }
        }
    }
}