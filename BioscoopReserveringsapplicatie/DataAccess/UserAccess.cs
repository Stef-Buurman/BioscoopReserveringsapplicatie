namespace BioscoopReserveringsapplicatie
{
    public static class UserAccess
    {
        private static readonly string Filename = "User.json";
        private static IDataAccess<UserModel> DataAccess = new DataAccess<UserModel>(Filename);
        public static void NewDataAccess(IDataAccess<UserModel> dataAccess) => DataAccess = dataAccess;
        public static List<UserModel> LoadAll() => DataAccess.LoadAll();

        public static void WriteAll(List<UserModel> accounts) => DataAccess.WriteAll(accounts);
    }
}