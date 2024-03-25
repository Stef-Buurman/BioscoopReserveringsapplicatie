using System.Text.Json;

static class UserAccess
{
    private static readonly string Filename = "User.json";
    private static readonly DataAccess<UserModel> _dataAccess = new DataAccess<UserModel>(Filename);

    public static List<UserModel> LoadAll() => _dataAccess.LoadAll();

    public static void WriteAll(List<UserModel> accounts) => _dataAccess.WriteAll(accounts);
}