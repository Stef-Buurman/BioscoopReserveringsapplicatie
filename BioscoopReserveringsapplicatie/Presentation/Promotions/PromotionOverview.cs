namespace BioscoopReserveringsapplicatie
{
    public static class PromotionOverview
    {
        private static PromotionLogic promotionLogic = new PromotionLogic();

        public static void Start()
        {
            Console.Clear();

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

            ColorConsole.WriteLineInfo("*Klik op escape om dit onderdeel te verlaten*\n");
            ColorConsole.WriteLineInfo("Klik op T om een promotie toetevoegen.\n");
            Print();
            int promotionId = new SelectionMenuUtil2<int>(options,
            () => AdminMenu.Start(), () => Start(),
            new List<KeyAction>(){ new KeyAction(ConsoleKey.T, () => AddPromotion.Start()) },
            showEscapeabilityText: false).Create();

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
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                   AddPromotion.Start();
                }),
                new Option<string>("Nee", () => {
                    AdminMenu.Start();
                }),
            };
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine("Wil je een promotie aanmaken?");
            new SelectionMenuUtil2<string>(options).Create();
        }
    }
}