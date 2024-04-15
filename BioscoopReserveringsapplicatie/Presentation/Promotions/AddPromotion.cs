namespace BioscoopReserveringsapplicatie
{
    public static class AddPromotion
    {
        private static PromotionLogic promotionLogic = new PromotionLogic();

        public static void Start()
        {
            string title = ReadLineUtil.EnterValue(true, () =>
            {
                ColorConsole.WriteColorLine("Promotie toevoegen\n", Globals.TitleColor);
                ColorConsole.WriteColor("Vul de [titel] van de promotie in: ", Globals.ColorInputcClarification);
            }, Promotions.Start);

            string description = ReadLineUtil.EnterValue(true, () =>
            {
                ColorConsole.WriteColorLine("Promotie toevoegen\n", Globals.TitleColor);
                ColorConsole.WriteColorLine($"Vul de [titel] van de promotie in: {title}", Globals.ColorInputcClarification);
                ColorConsole.WriteColor("Vul de [beschrijving] van de promotie in: ", Globals.ColorInputcClarification);
            }, Promotions.Start);

            if (promotionLogic.Add(title, description))
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", () => {Console.Clear(); PromotionOverview.Start();}),
                };
                SelectionMenuUtil.Create(options, () => Print(title, description, false));
            }
            else
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Terug", () => {Console.Clear(); PromotionOverview.Start();}),
                };
                SelectionMenuUtil.Create(options, () => Console.WriteLine("Er is een fout opgetreden tijdens het toevoegen van de promotie. Probeer het opnieuw.\n"));
            }
        }

        private static void Print(string title, string description, bool status)
        {
            ColorConsole.WriteColorLine("De promotie is toegevoegd!\n", Globals.PromotionColor);
            ColorConsole.WriteColorLine("[Promotie details]", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie titel: ]{title}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie beschrijving: ]{description}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie status: ]{(status ? "Actief" : "Inactief")}", Globals.PromotionColor);
        }
    }
}