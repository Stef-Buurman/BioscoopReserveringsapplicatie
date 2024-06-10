namespace BioscoopReserveringsapplicatie
{
    public static class PromotionEdit
    {
        private static PromotionLogic PromotionLogic = new PromotionLogic();
        private static PromotionModel? promotion = null;

        private static string newTitle = "";
        private static string newDescription = "";

        public static void Start(int promotionId)
        {
            promotion = PromotionLogic.GetById(promotionId);
            if (promotion == null) return;
            if (newTitle == "") newTitle = promotion.Title;
            if (newDescription == "") newDescription = promotion.Description;

            PrintEditingPromotion();
            ColorConsole.WriteColorLine("\nWat wilt u aanpassen van deze promotie?", Globals.TitleColor);

            List<Option<string>> editOptions = new List<Option<string>>()
            {
                new Option<string>("Titel", () => { PromotionTitle(); }),
                new Option<string>("Beschrijving", () => { PromotionDescription(promotionId); }),
                new Option<string>("Opslaan", () => { SavePromotion(); }, Globals.SaveColor),
                new Option<string>("Terug", () => ReadLineUtil.EscapeKeyPressed(GoBackToDetails, () => Start(promotionId)), Globals.GoBackColor)
            };

            new SelectionMenuUtil<string>(editOptions, new Option<string>("Naam")).Create();
        }

        private static void SavePromotion()
        {
            Console.Clear();
            Print(newTitle, newDescription);

            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    if (PromotionLogic.Edit(new PromotionModel(promotion.Id, newTitle, newDescription, promotion.Status)))
                        {
                            GoBackToDetails();
                        }
                        else
                        {
                            List<Option<string>> options = new List<Option<string>>
                            {
                                new Option<string>("Terug", () => GoBackToDetails()),
                            };
                            Console.WriteLine("Er is een fout opgetreden tijdens het bewerken van de promotie. Probeer het opnieuw.\n");
                            new SelectionMenuUtil<string>(options).Create();
                        }
                }),
                new Option<string>("Nee, pas de promotie verder aan", () => {Start(promotion.Id);}),
                new Option<string>("Nee, stop met aanpassen", () => GoBackToDetails())
            };
            new SelectionMenuUtil<string>(options, new Option<string>("Nee, pas de promotie verder aan")).Create();
        }

        private static void PromotionTitle()
        {
            PrintEditingPromotion();
            string question = "Voer de promotie [titel] in: ";
            newTitle = ReadLineUtil.EditValue(newTitle, question,
            () => Start(promotion.Id),
            "(druk op [Enter] om de huidige waarde te behouden en op [Esc] om terug te gaan)\n");

            while (string.IsNullOrEmpty(newTitle))
            {
                PrintEditingPromotion();
                ColorConsole.WriteColorLine("De titel mag niet leeg zijn.", Globals.ErrorColor);
                newTitle = ReadLineUtil.EditValue(newTitle, question,
                () => Start(promotion.Id),
                "(druk op [Enter] om de huidige waarde te behouden en op [Esc] om terug te gaan)\n");
            }
        }
        
        private static void PromotionDescription(int promotionId)
        {
            PrintEditingPromotion();
            string question = "Voer de promotie [beschrijving] in: ";
            newDescription = ReadLineUtil.EditValue(newDescription, question,
            () => Start(promotionId),
            "(druk op [Enter] om de huidige waarde te behouden en op [Esc] om terug te gaan)\n");


            while (string.IsNullOrEmpty(newDescription))
            {
                ColorConsole.WriteColorLine("De beschrijving mag niet leeg zijn.", Globals.ErrorColor);
                newDescription = ReadLineUtil.EditValue(newDescription, question, 
                () => Start(promotionId),
                "(druk op [Enter] om de huidige waarde te behouden en op [Esc] om terug te gaan)\n");
            }
        }

        private static void PrintEditingPromotion()
        {
            Console.Clear();
            if (newTitle != "")
            {
                ColorConsole.WriteColorLine("[Aangepaste Promotie Details]", Globals.PromotionColor);
                ColorConsole.WriteColorLine($"[Titel Promotie:] {newTitle}", Globals.PromotionColor);
            }
            if(newDescription != "")
            {
                ColorConsole.WriteColorLine($"[Beschrijving Promotie:] {newDescription}", Globals.PromotionColor);
            }
            if (newTitle != "" || newDescription != "")
            {
                HorizontalLine.Print();
            }
        }

        private static void Print(string newTitle, string description)
        {
            ColorConsole.WriteColorLine("[De nieuwe promotie details:]", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie titel: ]{newTitle}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie beschrijving: ]{description}", Globals.PromotionColor);
            HorizontalLine.Print();
            ColorConsole.WriteColorLine($"Weet u zeker dat u de promotie details van {newTitle} wilt bewerken?", Globals.ColorInputcClarification);
        }

        private static void GoBackToDetails()
        {
            newTitle = "";
            newDescription = "";
            Console.Clear();
            PromotionDetails.Start(promotion.Id);
        }
    }
}