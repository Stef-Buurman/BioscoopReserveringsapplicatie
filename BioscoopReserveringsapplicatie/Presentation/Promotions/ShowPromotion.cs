namespace BioscoopReserveringsapplicatie
{
    public class ShowPromotion
    {
        private static UserLogic userLogic = new UserLogic();
        private static PromotionLogic promotionLogic = new PromotionLogic();
        private static PromotionModel? activePromotion = promotionLogic.GetActivePromotion();
        public static void Start()
        {
            Console.Clear();
            if (activePromotion != null)
            {
                bool shownRecently = promotionLogic.IsPromotionShownRecently(activePromotion.Id);

                if (!shownRecently)
                {
                    promotionLogic.UpdatePromotionShown(activePromotion.Id);
                    userLogic.UpdateList(UserLogic.CurrentUser);
                    DisplayPromotion(activePromotion);
                }
            }
            UserMenu.Start();
        }

        private static void DisplayPromotion(PromotionModel promotion)
        {
            int titleLength = promotion.Title.Length;
            int totalPadding = (80 - titleLength) / 2;
            string titleLine = new string('*', 80);
            string centeredTitle =  $"{new string(' ', totalPadding)}{promotion.Title}{new string(' ', totalPadding)}";
            ColorConsole.WriteColorLine(titleLine, Globals.PromotionColor);
            ColorConsole.WriteColorLine(centeredTitle, Globals.PromotionColor);
            ColorConsole.WriteColorLine(titleLine, Globals.PromotionColor);
            Console.WriteLine();
            ColorConsole.WriteColorLine($"{promotion.Description}", Globals.PromotionColor);
            Console.WriteLine();

            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Sluiten", () => UserMenu.Start()),
            };

            new SelectionMenuUtil2<string>(options).Create();
        }
    }
}