using System.Text.Json;

static class MoviesAccess
{
    static readonly string Filename = "Movies.json";
    static string path = System.IO.Path.GetFullPath(System.IO.Path.Combine(Globals.currentDirectory, @"DataSources", Filename));

    public static List<MovieModel> LoadAll()
    {
        string json = File.ReadAllText(path);
        return JsonSerializer.Deserialize<List<MovieModel>>(json);
    }

    public static void WriteAll(List<MovieModel> accounts)
    {
        var options = new JsonSerializerOptions { WriteIndented = true };
        string json = JsonSerializer.Serialize(accounts, options);
        File.WriteAllText(path, json);
    }
}