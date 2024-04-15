namespace BioscoopReserveringsapplicatie
{
    public static class Promotions
    {
        public static void Start()
        {
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Promotie toevoegen", () => AddPromotion.Start()),
                new Option<string>("Promoties bekijken", () => PromotionOverview.Start()),
                new Option<string>("Terug", () => AdminMenu.Start()),
            };
            SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Kies wat je wilt doen: \n", Globals.TitleColor));
        }
    }
}