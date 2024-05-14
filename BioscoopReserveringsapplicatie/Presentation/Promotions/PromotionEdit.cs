namespace BioscoopReserveringsapplicatie
{
    public static class PromotionEdit
    {
        private static PromotionLogic PromotionLogic = new PromotionLogic();
        private static Action actionWhenEscapePressed = PromotionOverview.Start;
        private static PromotionModel? promotion = null;

        private static string newTitle = "";
        private static string newDescription = "";

        private static string _returnToTitle = "Title";
        private static string _returnToDescription = "Description";

        public static void Start(int promotionId, string returnTo = "")
        {
            promotion = PromotionLogic.GetById(promotionId);
            actionWhenEscapePressed = () => PromotionDetails.Start(promotionId);
            if (newTitle == "") newTitle = promotion.Title;
            if (newDescription == "") newDescription = promotion.Description;

            Console.Clear();

            if (returnTo == "" || returnTo == _returnToTitle)
            {
                PromotionTitle();
                returnTo = "";
            }

            if (returnTo == "" || returnTo == _returnToDescription)
            {
                PromotionDescription(promotionId);
                returnTo = "";
            }

            Console.Clear();
            Print(newTitle, newDescription);

            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                    if (PromotionLogic.Edit(new PromotionModel(promotionId, newTitle, newDescription, promotion.Status)))
                        {
                            PromotionDetails.Start(promotionId);
                        }
                        else
                        {
                            List<Option<string>> options = new List<Option<string>>
                            {
                                new Option<string>("Terug", () => {Console.Clear(); PromotionDetails.Start(promotionId);}),
                            };
                            Console.WriteLine("Er is een fout opgetreden tijdens het bewerken van de promotie. Probeer het opnieuw.\n");
                            new SelectionMenuUtil2<string>(options).Create();
                        }
                }),
                new Option<string>("Nee, pas de promotie verder aan", () => {Start(promotion.Id, _returnToDescription);}),
                new Option<string>("Nee, stop met aanpassen", () => {Console.Clear(); PromotionDetails.Start(promotion.Id);})
            };
            new SelectionMenuUtil2<string>(options).Create();
        }
        private static void PromotionTitle()
        {
            PrintEditingPromotion();
            string question = "Voer de promotie [titel] in: ";
            newTitle = ReadLineUtil.EditValue(newTitle, question,
            actionWhenEscapePressed,
            "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n");

            while (string.IsNullOrEmpty(newTitle))
            {
                PrintEditingPromotion();
                ColorConsole.WriteColorLine("De titel mag niet leeg zijn.", Globals.ErrorColor);
                newTitle = ReadLineUtil.EditValue(newTitle, question, 
                actionWhenEscapePressed,
                "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n");
            }
        }
        
        private static void PromotionDescription(int promotionId)
        {
            PrintEditingPromotion();
            string question = "Voer de promotie [beschrijving] in: ";
            newDescription = ReadLineUtil.EditValue(newDescription, question,
            () => Start(promotionId, _returnToTitle),
            "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n");


            while (string.IsNullOrEmpty(newDescription))
            {
                ColorConsole.WriteColorLine("De beschrijving mag niet leeg zijn.", Globals.ErrorColor);
                newDescription = ReadLineUtil.EditValue(newDescription, question, 
                () => Start(promotionId, _returnToTitle),
                "(druk op Enter om de huidige waarde te behouden en op Esc om terug te gaan)\n");
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
            ColorConsole.WriteColorLine("Voer nieuwe promotie details in:\n", Globals.TitleColor);
        }

        private static void Print(string newTitle, string description)
        {
            ColorConsole.WriteColorLine("[De nieuwe promotie details:]", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie titel: ]{newTitle}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie beschrijving: ]{description}", Globals.PromotionColor);
            HorizontalLine.Print();
            ColorConsole.WriteColorLine($"Weet u zeker dat u de promotie details van [{newTitle}] wilt [bewerken]?", Globals.ColorInputcClarification);
        }
    }
}