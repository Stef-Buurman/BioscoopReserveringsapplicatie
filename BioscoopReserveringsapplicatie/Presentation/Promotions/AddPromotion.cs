namespace BioscoopReserveringsapplicatie
{
    public static class AddPromotion
    {
        private static PromotionLogic promotionLogic = new PromotionLogic();
        private static string title = "";
        private static string description = "";

        public static void Start(string returnTo = "")
        {
            ClearFields();
            Console.Clear();

            if (returnTo == "" || returnTo == "Name")
            {
                title = AskForPromotionName();
                returnTo = "";
            }

            while (string.IsNullOrEmpty(title))
            {
                Console.Clear();
                ColorConsole.WriteColorLine("Voer alstublieft een geldige titel in!", Globals.ErrorColor);
                title = AskForPromotionName();
            }

            if (returnTo == "" || returnTo == "Description")
            {
                description = AskForPromotionDescription();
                returnTo = "";
            }

            while (string.IsNullOrEmpty(description))
            {
                Console.Clear();
                ColorConsole.WriteColorLine("Voer alstublieft een geldige beschrijving in!", Globals.ErrorColor);
                description = AskForPromotionDescription();
            }

            Print(title, description, Status.Inactive);
            PromotionModel newPromotion = new PromotionModel(promotionLogic.GetNextId(), title, description, Status.Inactive);
            List<Option<string>> options = new List<Option<string>>
            {
            new Option<string>("Opslaan en terug naar overzicht", () =>
            {
                if (promotionLogic.Add(newPromotion))
                {
                    ClearFields();
                    PromotionOverview.Start();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Er is een fout opgetreden tijdens het toevoegen van de promotie. Probeer het opnieuw.\n");
                    Start("Description");
                }
            }),
            new Option<string>("Verder gaan met aanpassen", () => { Start("Description"); }),
            new Option<string>("Verlaten zonder op te slaan", () => { ClearFields(); ExperienceOverview.Start(); }),
            };

            new SelectionMenuUtil<string>(options).Create();
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

        private static void Print(string title, string description, Status status)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("[Promotie details]", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie titel: ]{title}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie beschrijving: ]{description}", Globals.PromotionColor);
            ColorConsole.WriteColorLine($"[Promotie status: ]{status.GetDisplayName()}\n", Globals.PromotionColor);
            HorizontalLine.Print();
            ColorConsole.WriteColorLine($"Wilt u deze [Promotie] toevoegen?", Globals.ColorInputcClarification);
        }
        
        public static void ClearFields()
        {
            title = "";
            description = "";
        }
    }
}