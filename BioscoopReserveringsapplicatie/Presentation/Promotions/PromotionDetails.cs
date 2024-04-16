namespace BioscoopReserveringsapplicatie
{
    public static class PromotionDetails
    {
        private static PromotionLogic promotionLogic = new PromotionLogic();

        private static PromotionModel? promotion;

        public static void Start(int promotionId)
        {
            Console.Clear();

            promotion = promotionLogic.GetById(promotionId);

            List<Option<string>> options;

            if (promotion.Status)
            {
                options = new List<Option<string>>
                {
                    new Option<string>("Deactiveer promotie", () => {promotionLogic.Deactivate(promotionId); Start(promotionId);}),
                    new Option<string>("Bewerk promotie", () => PromotionEdit.Start(promotionId)),
                    new Option<string>("Verwijder promotie", () => PromotionDelete.Start(promotionId)),
                    new Option<string>("Terug", () => PromotionOverview.Start()),
                };
            }
            else
            {
                options = new List<Option<string>>
                {
                    new Option<string>("Activeer promotie", () => {promotionLogic.Activate(promotionId); Start(promotionId);}),
                    new Option<string>("Bewerk promotie", () => PromotionEdit.Start(promotionId)),
                    new Option<string>("Verwijder promotie", () => PromotionDelete.Start(promotionId)),
                    new Option<string>("Terug", () => PromotionOverview.Start()),
                };
            }
            Print();
            string selectionMenu = new SelectionMenuUtil2<string>(options).Create();
        }

        private static void Print()
        {
            if (promotion == null) return;

            ColorConsole.WriteColorLine("[Promotie details]", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie titel: ]{promotion.Title}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie beschrijving: ]{promotion.Description}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie status: ]{(promotion.Status ? "Actief" : "Inactief")}\n", Globals.PromotionColor);
        }
    }
}