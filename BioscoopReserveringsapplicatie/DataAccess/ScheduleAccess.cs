namespace BioscoopReserveringsapplicatie
{
    static class ScheduleAccess
    {
        private static readonly string Filename = "Schedules.json";
        private static readonly DataAccess<ScheduleModel> _dataAccess = new DataAccess<ScheduleModel>(Filename);

        public static List<ScheduleModel> LoadAll() => _dataAccess.LoadAll(); 

        public static void WriteAll(List<ScheduleModel> rooms) => _dataAccess.WriteAll(rooms);
    }
}