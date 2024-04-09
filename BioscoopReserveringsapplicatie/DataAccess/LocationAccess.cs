namespace BioscoopReserveringsapplicatie
{
    static class LocationAccess
    {
        private static readonly string Filename = "Location.json";
        private static readonly DataAccess<LocationModel> _dataAccess = new DataAccess<LocationModel>(Filename);

        public static List<LocationModel> LoadAll() => _dataAccess.LoadAll(); 

        public static void WriteAll(List<LocationModel> locations) => _dataAccess.WriteAll(locations);
    }
}