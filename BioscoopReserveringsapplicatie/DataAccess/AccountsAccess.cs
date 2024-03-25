using System.Text.Json;

static class AccountsAccess
{
    private static readonly string Filename = "Accounts.json";
    private static readonly DataAccess<AccountModel> _dataAccess = new DataAccess<AccountModel>(Filename);

    public static List<AccountModel> LoadAll() => _dataAccess.LoadAll();

    public static void WriteAll(List<AccountModel> accounts) => _dataAccess.WriteAll(accounts);
}