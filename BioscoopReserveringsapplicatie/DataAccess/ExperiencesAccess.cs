namespace BioscoopReserveringsapplicatie
{
    public static class ExperiencesAccess
    {
        private static readonly string Filename = "Experiences.json";
        private static IDataAccess<ExperiencesModel> _dataAccess = new DataAccess<ExperiencesModel>(Filename);
        public static void NewDataAccess(IDataAccess<ExperiencesModel> dataAccess) => _dataAccess = dataAccess;
        public static List<ExperiencesModel> LoadAll() => _dataAccess.LoadAll();

        public static void WriteAll(List<ExperiencesModel> accounts) => _dataAccess.WriteAll(accounts);
    }
}