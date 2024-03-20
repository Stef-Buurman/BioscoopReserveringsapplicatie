using System.Text.Json;

static class ExperiencesAccess
{
    static readonly string Filename = "Experiences.json";
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Globals.currentDirectory, @"DataSources", Filename));

    public static List<ExperiencesModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<ExperiencesModel>>(json);
    }

    public static void WriteAll(List<ExperiencesModel> accounts)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(accounts, options);
        File.WriteAllText(path, json);
    }
}