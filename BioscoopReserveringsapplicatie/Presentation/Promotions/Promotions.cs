namespace BioscoopReserveringsapplicatie
{
    public static class Promotions
    {
        public static void Start()
        {
            Console.Clear();

            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Promotie toevoegen", () => AddPromotion.Start()),
                new Option<string>("Promoties bekijken", () => PromotionOverview.Start()),
                new Option<string>("Terug", () => AdminMenu.Start()),
            };
            ColorConsole.WriteColorLine("Kies wat je wilt doen: \n", Globals.TitleColor);
            string selectionMenu = new SelectionMenuUtil2<string>(options).Create();
        }
    }
}