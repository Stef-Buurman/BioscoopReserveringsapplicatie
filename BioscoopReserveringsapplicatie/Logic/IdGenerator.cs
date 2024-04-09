namespace BioscoopReserveringsapplicatie
{
    public static class IdGenerator
    {
        public static int GetNextId<T>(List<T> experiences) where T : IID
        {
            if (experiences.Count > 0)
            {
                return experiences.Max(exp => exp.Id) + 1;
            }
            else
            {
                return 1;
            }
        }
    }
}