namespace BioscoopReserveringsapplicatie
{
    public static class AddPromotion
    {
        private static PromotionLogic promotionLogic = new PromotionLogic();

        public static void Start(string returnTo = "")
        {
            Console.Clear();

            string title = AskForPromotionName();
            string description = AskForPromotionDescription(title);

            if (promotionLogic.Add(title, description))
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", () => {Console.Clear(); PromotionOverview.Start();}),
                };
                Print(title, description, false);
                string selectionMenu = new SelectionMenuUtil2<string>(options).Create();
            }
            else
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", () => {Console.Clear(); Promotions.Start();}),
                };
                ColorConsole.WriteColorLine("\nEr is een fout opgetreden tijdens het toevoegen van de promotie. Probeer het opnieuw.\n", Globals.ErrorColor);
                string selectionMenu = new SelectionMenuUtil2<string>(options).Create();
            }
        }

        private static string AskForPromotionName()
        {
            return ReadLineUtil.EnterValue(true, () =>
            {
                ColorConsole.WriteColorLine("Promotie toevoegen\n", Globals.TitleColor);
                ColorConsole.WriteColor("Vul de [titel] van de promotie in: ", Globals.ColorInputcClarification);
            }, Promotions.Start);
        }

        private static string AskForPromotionDescription(string title)
        {
            return ReadLineUtil.EnterValue(true, () =>
            {
                ColorConsole.WriteColorLine("Promotie toevoegen\n", Globals.TitleColor);
                ColorConsole.WriteColorLine($"Vul de [titel] van de promotie in: {title}", Globals.ColorInputcClarification);
                ColorConsole.WriteColor("Vul de [beschrijving] van de promotie in: ", Globals.ColorInputcClarification);
            }, () => AskForPromotionName());
        }

        private static void Print(string title, string description, bool status)
        {
            ColorConsole.WriteColorLine("\nDe promotie is toegevoegd!\n", Globals.PromotionColor);
            ColorConsole.WriteColorLine("[Promotie details]", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie titel: ]{title}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie beschrijving: ]{description}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie status: ]{(status ? "Actief" : "Inactief")}\n", Globals.PromotionColor);
        }
    }
}