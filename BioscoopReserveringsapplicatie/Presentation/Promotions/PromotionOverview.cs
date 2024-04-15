namespace BioscoopReserveringsapplicatie
{
    public static class PromotionOverview
    {
        private static PromotionLogic promotionLogic = new PromotionLogic();

        public static void Start()
        {
            List<PromotionModel> promotions = promotionLogic.GetAll();

            if (promotions.Count == 0) PrintWhenNoPromotionsFound("Er zijn geen promoties gevonden.");
            else ShowPromotions(promotions);
        }

        private static int ShowPromotions(List<PromotionModel> promotions)
        {
            List<Option<int>> options = new List<Option<int>>();

            foreach (PromotionModel promotion in promotions)
            {
                options.Add(new Option<int>(promotion.Id, promotion.Title));
            }

            int promotionId = SelectionMenuUtil.Create(options, 21, Print, () => { Console.Clear(); Promotions.Start(); });
            Console.Clear();
            ShowPromotionDetails(promotionId);
            return promotionId;
        }

        private static void ShowPromotionDetails(int promotionId)
        {
            if (promotionId != 0)
            {
                PromotionDetails.Start(promotionId);
            }
        }

        private static void Print()
        {
            ColorConsole.WriteColorLine("Dit zijn alle promoties die momenteel bestaan:", Globals.TitleColor);
        }

        private static void PrintWhenNoPromotionsFound(string message)
        {
            Console.Clear();
            Console.WriteLine(message);
            Thread.Sleep(500);
            Console.WriteLine("Terug naar promotie overzicht...");
            Thread.Sleep(1500);
            Start();
        }
    }
}