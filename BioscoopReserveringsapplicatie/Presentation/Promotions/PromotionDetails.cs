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

            if (promotion == null)
            {
                ColorConsole.WriteColorLine("Promotie niet gevonden", Globals.ErrorColor);
                PromotionOverview.Start();
                return;
            }

            List<Option<string>> options = new List<Option<string>>();

            if (promotion.Status != Status.Archived)
            {
                options.Add(new Option<string>(promotion.Status == Status.Active ? "Deactiveer promotie" : "Activeer promotie", () =>
                {
                    ActiveOrDeActiveatePromotion(promotionId);
                }));
            }
            options.AddRange(new List<Option<string>>
            {
                new Option<string>("Bewerk promotie", () => PromotionEdit.Start(promotionId)),
                new Option<string>(promotion.Status != Status.Archived ? "Archiveer promotie" : "Dearchiveer promotie", () => PromotionArchive.Start(promotionId)),
                new Option<string>("Terug", () => PromotionOverview.Start()),
            });

            Print();
            string selectionMenu = new SelectionMenuUtil<string>(options).Create();
        }

        private static void ActiveOrDeActiveatePromotion(int promotionId)
        {
            if (promotion?.Status == Status.Active)
            {
                List<Option<string>> options2 = new List<Option<string>>
                        {
                            new Option<string>("Ja", () => {
                                promotionLogic.Deactivate(promotionId);
                                Start(promotionId);
                            }),
                            new Option<string>("Nee", () => {
                                Start(promotionId);
                            }),
                        };
                ColorConsole.WriteColorLine("\n----------------------------------------------------------------", Globals.ErrorColor);
                ColorConsole.WriteColorLine("Weet u zeker dat u deze promotie wilt deactiveren?", Globals.ErrorColor);
                string selectionMenu2 = new SelectionMenuUtil<string>(options2, new Option<string>("Nee")).Create();
            }
            else
            {
                List<Option<string>> options2 = new List<Option<string>>
                        {
                            new Option<string>("Ja", () => {
                                promotionLogic.Activate(promotionId);
                                Start(promotionId);
                            }),
                            new Option<string>("Nee", () => {
                                Start(promotionId);
                            }),
                        };
                ColorConsole.WriteColorLine("\n----------------------------------------------------------------", Globals.ErrorColor);
                ColorConsole.WriteColorLine("Weet u zeker dat u deze promotie wilt activeren?", Globals.ErrorColor);
                string selectionMenu2 = new SelectionMenuUtil<string>(options2, new Option<string>("Nee")).Create();
            }
        }

        private static void Print()
        {
            if (promotion == null) return;

            ColorConsole.WriteColorLine("[Promotie details]", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie titel: ]{promotion.Title}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie beschrijving: ]{promotion.Description}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie status: ]{promotion.Status.GetDisplayName()}\n\n", Globals.PromotionColor);
            Console.WriteLine("Wat wil je doen?");
        }
    }
}