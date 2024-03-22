using System.Text.Json;

static class ExperiencesAccess
{
    private static readonly string Filename = "Experiences.json";
    private static readonly DataAccess<ExperiencesModel> _dataAccess = new DataAccess<ExperiencesModel>(Filename);

    public static List<ExperiencesModel> LoadAll() => _dataAccess.LoadAll();

    public static void WriteAll(List<ExperiencesModel> accounts) => _dataAccess.WriteAll(accounts);
}