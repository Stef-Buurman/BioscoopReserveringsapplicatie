using System.Drawing;

namespace BioscoopReserveringsapplicatie
{
    public class ShowPromotion
    {
        public static void Start()
        {
            Console.Clear();
            PromotionLogic promotionLogic = new PromotionLogic();
            PromotionModel? activePromotion = promotionLogic.GetActivePromotion();

            if (activePromotion != null)
            {
                ColorConsole.WriteColorLine("***************************************************", Globals.PromotionColor);
                ColorConsole.WriteColorLine("*                      ACTIE                      *", Globals.PromotionColor);
                ColorConsole.WriteColorLine("***************************************************", Globals.PromotionColor);
                Console.ResetColor();

                ColorConsole.WriteColorLine($"[Titel:] {activePromotion.Title}", Globals.PromotionColor);
                ColorConsole.WriteColorLine($"[Beschrijving:] {activePromotion.Description}", Globals.PromotionColor);

                // ColorConsole.WriteColorLine("*************************", Globals.PromotionColor);
                Console.WriteLine();

                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Volgende", () => UserMenu.Start()),
                };

                new SelectionMenuUtil2<string>(options).Create();
            }
            else
            {
                UserMenu.Start();
            }
        }
    }
}