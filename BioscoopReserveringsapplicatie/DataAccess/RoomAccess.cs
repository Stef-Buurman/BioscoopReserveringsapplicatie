namespace BioscoopReserveringsapplicatie
{
    static class RoomAccess
    {
        private static readonly string Filename = "Location.json";
        private static readonly DataAccess<RoomModel> _dataAccess = new DataAccess<RoomModel>(Filename);

        public static List<RoomModel> LoadAll() => _dataAccess.LoadAll(); 

        public static void WriteAll(List<RoomModel> locations) => _dataAccess.WriteAll(locations);
    }
}