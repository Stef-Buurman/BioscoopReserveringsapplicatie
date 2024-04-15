namespace BioscoopReserveringsapplicatie
{
    public static class PromotionDetails
    {
        private static PromotionLogic promotionLogic = new PromotionLogic();

        private static PromotionModel? promotion;

        public static void Start(int promotionId)
        {
            promotion = promotionLogic.GetById(promotionId);

            List<Option<string>> options;

            if (promotion.Status)
            {
                options = new List<Option<string>>
                {
                    new Option<string>("Deactiveer promotie", () => {promotionLogic.Deactivate(promotionId); Start(promotionId);}),
                    new Option<string>("Bewerk promotie"),
                    new Option<string>("Verwijder promotie"),
                    new Option<string>("Terug", () => PromotionOverview.Start()),
                };
            }
            else
            {
                options = new List<Option<string>>
                {
                    new Option<string>("Activeer promotie", () => {promotionLogic.Activate(promotionId); Start(promotionId);}),
                    new Option<string>("Bewerk promotie"),
                    new Option<string>("Verwijder promotie"),
                    new Option<string>("Terug", () => PromotionOverview.Start()),
                };
            }

            SelectionMenuUtil.Create(options, Print);
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