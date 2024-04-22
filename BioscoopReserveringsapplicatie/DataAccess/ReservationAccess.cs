namespace BioscoopReserveringsapplicatie
{
    public static class ReservationAccess
    {
        private static readonly string Filename = "Reservations.json";
        private static IDataAccess<ReservationModel> _dataAccess = new DataAccess<ReservationModel>(Filename);
        public static void NewDataAccess(IDataAccess<ReservationModel> dataAccess) => _dataAccess = dataAccess;
        public static List<ReservationModel> LoadAll() => _dataAccess.LoadAll();
        public static void WriteAll(List<ReservationModel> reservation) => _dataAccess.WriteAll(reservation);
    }
}