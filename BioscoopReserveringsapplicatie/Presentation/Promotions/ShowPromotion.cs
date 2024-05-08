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
            ColorConsole.WriteColorLine("***************************************************", Globals.PromotionColor);
            ColorConsole.WriteColorLine("*                      ACTIE                      *", Globals.PromotionColor);
            ColorConsole.WriteColorLine("***************************************************", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Titel:] {promotion.Title}", Globals.PromotionColor);
            Console.WriteLine();
            ColorConsole.WriteColorLine($"[Beschrijving:] {promotion.Description}", Globals.PromotionColor);
            Console.WriteLine();

            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Sluiten", () => UserMenu.Start()),
            };

            new SelectionMenuUtil2<string>(options).Create();
        }
    }
}