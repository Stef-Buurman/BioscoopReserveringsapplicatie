namespace BioscoopReserveringsapplicatie
{
    public static class AddPromotion
    {
        private static PromotionLogic promotionLogic = new PromotionLogic();
        private static string title = "";
        private static string description = "";

        public static void Start(string returnTo = "")
        {
            Console.Clear();

            if (returnTo == "" || returnTo == "Name")
            {
                title = AskForPromotionName();
                returnTo = "";
            }
            if (returnTo == "" || returnTo == "Description")
            {
                description = AskForPromotionDescription(title);
                returnTo = "";
            }

            if (promotionLogic.Add(new PromotionModel(promotionLogic.GetNextId(), title, description, Status.Inactive)))
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", () => PromotionOverview.Start())
                };

                Print(title, description, false);
                string selectionMenu = new SelectionMenuUtil2<string>(options).Create();
            }
            else
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", () => PromotionOverview.Start())
                };

                ColorConsole.WriteColorLine("\nEr is een fout opgetreden tijdens het toevoegen van de promotie. Probeer het opnieuw.\n", Globals.ErrorColor);
                string selectionMenu = new SelectionMenuUtil2<string>(options).Create();
            }
        }

        private static string AskForPromotionName()
        {
            ColorConsole.WriteColorLine("Promotie toevoegen\n", Globals.TitleColor);
            return ReadLineUtil.EnterValue("Vul de [titel] van de promotie in: ", Promotions.Start);
        }

        private static string AskForPromotionDescription(string title)
        {
            //ColorConsole.WriteColorLine("Promotie toevoegen\n", Globals.TitleColor);
            //ColorConsole.WriteColorLine($"Vul de [titel] van de promotie in: {title}", Globals.ColorInputcClarification);
            return ReadLineUtil.EnterValue("Vul de [beschrijving] van de promotie in: ", () => Start("Name"));
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