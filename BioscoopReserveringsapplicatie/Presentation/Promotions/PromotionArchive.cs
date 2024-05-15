namespace BioscoopReserveringsapplicatie
{
    public static class PromotionArchive
    {
        private static PromotionLogic promotionLogic = new PromotionLogic();
        public static void Start(int promotionId)
        {
            PromotionModel? promotion = promotionLogic.GetById(promotionId);
            if (promotion == null) return;

            if (promotion.Status == Status.Archived)
            {
                List<Option<string>> options2 = new List<Option<string>>
                        {
                            new Option<string>("Ja", () => {
                                promotionLogic.Unarchive(promotionId);
                                PromotionDetails.Start(promotionId);
                            }),
                            new Option<string>("Nee", () => {
                                PromotionDetails.Start(promotionId);
                            }),
                        };
                ColorConsole.WriteColorLine("\n----------------------------------------------------------------", Globals.ErrorColor);
                ColorConsole.WriteColorLine("Weet u zeker dat u deze promotie wilt dearchiveren?", Globals.ErrorColor);
                string selectionMenu2 = new SelectionMenuUtil<string>(options2, new Option<string>("Nee")).Create();
            }
            else
            {
                List<Option<string>> options2 = new List<Option<string>>
                        {
                            new Option<string>("Ja", () => {
                                promotionLogic.Archive(promotionId);
                                PromotionDetails.Start(promotionId);
                            }),
                            new Option<string>("Nee", () => {
                                PromotionDetails.Start(promotionId);
                            }),
                        };
                ColorConsole.WriteColorLine("\n----------------------------------------------------------------", Globals.ErrorColor);
                ColorConsole.WriteColorLine("Weet u zeker dat u deze promotie wilt archiveren?", Globals.ErrorColor);
                string selectionMenu2 = new SelectionMenuUtil<string>(options2, new Option<string>("Nee")).Create();
            }
        }
    }
}