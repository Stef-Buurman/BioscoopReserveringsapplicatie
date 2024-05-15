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
                description = AskForPromotionDescription();
                returnTo = "";
            }
            Print(title, description, true);
            PromotionModel newPromotion = new PromotionModel(promotionLogic.GetNextId(), title, description, Status.Active);
            List<Option<string>> options = new List<Option<string>>
            {
            new Option<string>("Opslaan en verlaten", () => 
            {
                if (promotionLogic.Add(newPromotion))
                {
                    PromotionOverview.Start();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Er is een fout opgetreden tijdens het toevoegen van de promotie. Probeer het opnieuw.\n");
                    Start("Name");
                }
            }),
            new Option<string>("Verder gaan met aanpassen", () => { Start("Name"); }),
            new Option<string>("Verlaten zonder op te slaan", () => { ExperienceOverview.Start(); }),
            };

            new SelectionMenuUtil2<string>(options).Create();
        }

        private static string AskForPromotionName()
        {
            ColorConsole.WriteColorLine("Promotie toevoegen\n", Globals.TitleColor);
            return ReadLineUtil.EnterValue("Vul de [titel] van de promotie in: ", PromotionOverview.Start);
        }

        private static string AskForPromotionDescription()
        {
            return ReadLineUtil.EnterValue("Vul de [beschrijving] van de promotie in: ", () => Start("Name"), false, false);
        }

        private static void Print(string title, string description, bool status)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("De promotie is toegevoegd!\n", Globals.PromotionColor);
            ColorConsole.WriteColorLine("[Promotie details]", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie titel: ]{title}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie beschrijving: ]{description}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie status: ]{(status ? "Actief" : "Inactief")}\n", Globals.PromotionColor);
            ColorConsole.WriteColorLine("\nWat wilt u doen?\n", Globals.ColorInputcClarification);
        }
    }
}