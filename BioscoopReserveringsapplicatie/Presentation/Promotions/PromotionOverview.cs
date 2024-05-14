namespace BioscoopReserveringsapplicatie
{
    public static class PromotionOverview
    {
        private static PromotionLogic promotionLogic = new PromotionLogic();
        private static Func<PromotionModel, string[]> promotionDataExtractor = ExtractPromotionData;

        public static void Start()
        {
            Console.Clear();

            List<PromotionModel> promotions = promotionLogic.GetAll();

            if (promotions.Count == 0) PrintWhenNoPromotionsFound("Er zijn geen promoties gevonden.");
            else ShowPromotions(promotions);
        }

        private static void ShowPromotions(List<PromotionModel> promotions)
        {
            Console.Clear();

            List<Option<int>> options = new List<Option<int>>();

            List<string> columnHeaders = new List<string>
            {
                "Naam promotie",
                "Beschrijving",
                "Status",
            };

            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, promotions, promotionDataExtractor);

            foreach (PromotionModel promotion in promotions)
            {
                string promotionTitle = promotion.Title;
                if (promotionTitle.Length > 25)
                {
                    promotionTitle = promotionTitle.Substring(0, 25) + "...";
                }

                string promotionDescription = promotion.Description;
                if (promotionDescription.Length > 25)
                {
                    promotionDescription = promotionDescription.Substring(0, 25) + "...";
                }

                string promotionInfo = string.Format("{0,-" + (columnWidths[0] + 2) + "} {1,-" + (columnWidths[1] + 2) + "} {2,-" + (columnWidths[2] + 2) + "}", promotionTitle, promotionDescription, promotion.Status.GetDisplayName());
                options.Add(new Option<int>(promotion.Id, promotionInfo));
            }
            ColorConsole.WriteLineInfo("*Klik op escape om dit onderdeel te verlaten*\n");
            ColorConsole.WriteLineInfo("Klik op T om een promotie toe te voegen.\n");
            ColorConsole.WriteColorLine("Dit zijn alle promoties die momenteel bestaan:\n", Globals.TitleColor);
            Print();
            int promotionId = new SelectionMenuUtil2<int>(options,
            () => 
            {
                AdminMenu.Start();
            },
            () => 
            {
                Start();
            },
            new List<KeyAction>(){ 
                new KeyAction(ConsoleKey.T, () => AddPromotion.Start()) 
            },
            showEscapeabilityText: false).Create();

            ShowPromotionDetails(promotionId);
        }

        private static void ShowPromotionDetails(int promotionId)
        {
            if (promotionId != 0)
            {
                PromotionDetails.Start(promotionId);
            }
        }

        private static void PrintWhenNoPromotionsFound(string message)
        {
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                   AddPromotion.Start();
                }),
                new Option<string>("Nee", () => {
                    AdminMenu.Start();
                }),
            };
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine("Wil je een promotie aanmaken?");
            new SelectionMenuUtil2<string>(options).Create();
        }

        private static void Print()
        {
            List<string> columnHeaders = new List<string>
            {
                "Naam promotie",
                "Beschrijving",
                "Status",
            };

            List<PromotionModel> allPromotions = promotionLogic.GetAll();
            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, allPromotions, promotionDataExtractor);

            Console.Write("".PadRight(3));
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write(columnHeaders[i].PadRight(columnWidths[i] + 3));
            }
            Console.WriteLine();

            Console.Write("".PadRight(3));
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write("".PadRight(columnWidths[i], '-').PadRight(columnWidths[i] + 3));
            }
            Console.WriteLine();
        }

        private static string[] ExtractPromotionData(PromotionModel promotion)
        {
            string[] promotionInfo = {
                promotion.Title,
                promotion.Description,
                promotion.Status.GetDisplayName(),
            };

            return promotionInfo;
        }
    }
}