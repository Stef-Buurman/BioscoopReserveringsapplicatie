namespace BioscoopReserveringsapplicatie
{
    public static class MoviesAccess
    {
        private static readonly string Filename = "Movies.json";
        private static IDataAccess<MovieModel> _dataAccess = new DataAccess<MovieModel>(Filename);
        public static void NewDataAccess(IDataAccess<MovieModel> dataAccess) => _dataAccess = dataAccess;
        public static List<MovieModel> LoadAll() => _dataAccess.LoadAll();

        public static void WriteAll(List<MovieModel> movies) => _dataAccess.WriteAll(movies);
    }
}