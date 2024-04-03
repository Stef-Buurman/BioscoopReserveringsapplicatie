namespace BioscoopReserveringsapplicatie
{
    public static class UserAccess
    {
        private static readonly string Filename = "User.json";
        private static IDataAccess<UserModel> _dataAccess = new DataAccess<UserModel>(Filename);
        public static void NewDataAccess(IDataAccess<UserModel> dataAccess) => _dataAccess = dataAccess;
        public static List<UserModel> LoadAll() => _dataAccess.LoadAll();

        public static void WriteAll(List<UserModel> accounts) => _dataAccess.WriteAll(accounts);
    }
}