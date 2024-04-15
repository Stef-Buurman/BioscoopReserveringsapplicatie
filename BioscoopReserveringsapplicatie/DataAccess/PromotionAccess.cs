namespace BioscoopReserveringsapplicatie
{
    public static class PromotionAccess
    {
        private static readonly string Filename = "Promotions.json";
        private static IDataAccess<PromotionModel> _dataAccess = new DataAccess<PromotionModel>(Filename);
        public static void NewDataAccess(IDataAccess<PromotionModel> dataAccess) => _dataAccess = dataAccess;
        public static List<PromotionModel> LoadAll() => _dataAccess.LoadAll();

        public static void WriteAll(List<PromotionModel> promotion) => _dataAccess.WriteAll(promotion);
    }
}