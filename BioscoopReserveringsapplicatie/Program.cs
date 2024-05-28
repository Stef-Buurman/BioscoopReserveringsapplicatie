using System.Text;

namespace BioscoopReserveringsapplicatie
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            //LandingPage.Start();


            Option<string>[,] options = OptionGrid.GenerateOptionGrid(40, 40, true);

            List<(int, int)> selectedOptions = new List<(int, int)>
            {
                (0, 0),
                (1, 1),
                (0, 6),
                (0, 1),
                (0, 2),
                (0, 3),
                (0, 4),
                (0, 5),
                (6,5)
            };

            List<(int, int)> selectionMenu = new SelectionMenuUtil<string>(options, selectedOptions, true).CreateGridSelect(out List<(int, int)> SelectedOptions);
            Console.WriteLine("\nSelected options:");
            foreach (var index in selectionMenu)
            {
                Console.WriteLine($"Selected option at index: ({index.Item1}, {index.Item2})");
            }
        }
    }
}