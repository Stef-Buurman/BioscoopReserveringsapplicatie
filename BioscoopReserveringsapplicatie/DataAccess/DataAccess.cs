using System.Globalization;
using System.Reflection;
using System.Text.Json;

namespace BioscoopReserveringsapplicatie
{
    public class DataAccess<T> : IDataAccess<T>
    {
        private string Filename
        {
            get
            {
                string modelName = typeof(T).Name;
                string fileName = "";
                if (modelName.EndsWith("Model"))
                {
                    fileName = modelName.Substring(0, modelName.Length - 5);
                }
                else
                {
                    fileName = modelName;
                }
                return fileName + ".json";
            }
        }
        private string Path
        {
            get => System.IO.Path.GetFullPath(System.IO.Path.Combine(getPath(), @"DataSources", Filename));
        }

        private static string CurrentDirectoryProduction = Environment.CurrentDirectory;
        private static string getPath(string currentPath = null)
        {
            var directory = new DirectoryInfo(
                currentPath ?? Directory.GetCurrentDirectory());
            while (directory != null && !directory.GetFiles("*.sln").Any())
            {
                directory = directory.Parent;
            }
            return System.IO.Path.Combine(directory?.ToString() ?? "", Assembly.GetCallingAssembly().GetName().Name ?? "");
        }
        public List<T> LoadAll()
        {
            try
            {
                string json = File.ReadAllText(Path);
                return JsonSerializer.Deserialize<List<T>>(json);
            }
            catch (FileNotFoundException)
            {
                File.Create(Path).Close();
                string startingJson = "[]";
                File.WriteAllText(Path, startingJson);

                string json = File.ReadAllText(Path);
                return JsonSerializer.Deserialize<List<T>>(json);
            }
        }

        public void WriteAll(List<T> accounts)
        {
            JsonSerializerOptions options = new JsonSerializerOptions { WriteIndented = true };
            string json = JsonSerializer.Serialize(accounts, options);
            File.WriteAllText(Path, json);
        }
    }
}
