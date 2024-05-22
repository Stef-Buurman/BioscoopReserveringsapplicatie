namespace BioscoopReserveringsapplicatie
{
    public static class AddLocation
    {
        private static LocationLogic locationLogic = new LocationLogic();
        private static string title = "";

        public static void Start(string returnTo = "")
        {
            Console.Clear();

            if (returnTo == "" || returnTo == "Name")
            {
                title = AskForLocationName();
                returnTo = "";
            }
            Print(title);
            LocationModel newLocation = new LocationModel(locationLogic.GetNextId(), title);
            List<Option<string>> options = new List<Option<string>>
            {
            new Option<string>("Opslaan en verlaten", () => 
            {
                if (locationLogic.Add(newLocation))
                {
                    LocationOverview.Start();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Er is een fout opgetreden tijdens het toevoegen van de Locatie. Probeer het opnieuw.\n");
                    Thread.Sleep(3000);
                    Start("Name");
                }
            }),
            new Option<string>("Verder gaan met aanpassen", () => { Start("Name"); }),
            new Option<string>("Verlaten zonder op te slaan", () => { LocationOverview.Start(); }),
            };

            new SelectionMenuUtil<string>(options).Create();
        }

        private static string AskForLocationName()
        {
            ColorConsole.WriteColorLine("Locatie toevoegen\n", Globals.TitleColor);
            return ReadLineUtil.EnterValue("Vul de [Naam] van de Locatie in: ", LocationOverview.Start);
        }

        private static void Print(string title)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("[Locatie details]", Globals.LocationColor);
            ColorConsole.WriteColorLine($"[Naam Locatie: ]{title}", Globals.LocationColor);
            HorizontalLine.Print();
            ColorConsole.WriteColorLine($"Wilt u deze [Locatie] toevoegen?", Globals.ColorInputcClarification);
        }
    }
}