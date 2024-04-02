using System.Text.Json;

namespace BioscoopReserveringsapplicatie
{
    public class DataAccess<T> : IDataAccess<T>
    {
        private readonly string Filename;
        private string Path
        {
            get => System.IO.Path.GetFullPath(System.IO.Path.Combine(Globals.currentDirectory, @"DataSources", Filename));
        }
        public DataAccess(string filename) => Filename = filename;
        public List<T> LoadAll()
        {
            string json = File.ReadAllText(Path);
            return JsonSerializer.Deserialize<List<T>>(json);
        }

        public void WriteAll(List<T> accounts)
        {
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(accounts, options);
            File.WriteAllText(Path, json);
        }
    }
}
