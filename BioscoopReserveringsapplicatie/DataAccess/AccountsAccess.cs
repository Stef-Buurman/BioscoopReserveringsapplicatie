using System.Text.Json;

static class AccountsAccess
{
    static string CurrentDirectoryDevelop = Directory.GetParent(Directory.GetCurrentDirectory()).Parent.Parent.FullName;
    static string CurrentDirectoryProduction =  Environment.CurrentDirectory;
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(CurrentDirectoryProduction, @"DataSources/accounts.json"));
    static readonly string Filename = "Accounts.json";
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Globals.currentDirectory, @"DataSources", Filename));


    public static List<AccountModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<AccountModel>>(json);
    }

    public static void WriteAll(List<AccountModel> accounts)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(accounts, options);
        File.WriteAllText(path, json);
    }
}