using System.Text.Json;

static class MoviesAccess
{
    private static readonly string Filename = "Movies.json";
    private static readonly DataAccess<MovieModel> _dataAccess = new DataAccess<MovieModel>(Filename);

    public static List<MovieModel> LoadAll() => _dataAccess.LoadAll();
    
    public static void WriteAll(List<MovieModel> accounts) => _dataAccess.WriteAll(accounts);
}