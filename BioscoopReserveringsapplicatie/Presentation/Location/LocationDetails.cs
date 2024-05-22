namespace BioscoopReserveringsapplicatie
{
    static class LocationDetails
    {
        private static LocationLogic LocationLogic = new LocationLogic();
        private static LocationModel? location;

        public static void Start(int locationId)
        {
            Console.Clear();
            location = LocationLogic.GetById(locationId);
            List<Option<string>> options;

            if (location.Status == Status.Archived)
            {
                options = new List<Option<string>>
                {
                    new Option<string>("Bewerk locatie", () => LocationEdit.Start(location.Id)),
                    new Option<string>("Dearchiveer locatie", () => LocationArchive.Start(location.Id)),
                    new Option<string>("Terug", () => {Console.Clear(); LocationOverview.Start();}),
                };
            }
            else
            {
                options = new List<Option<string>>
                {
                    new Option<string>("Bewerk locatie", () => LocationEdit.Start(location.Id)),
                    new Option<string>("Archiveer locatie", () => LocationArchive.Start(location.Id)),
                    new Option<string>("Terug", () => {Console.Clear(); LocationOverview.Start();}),
                };
            }
            Print();
            new SelectionMenuUtil<string>(options).Create();
        }

        private static void Print()
        {
            ColorConsole.WriteColorLine("[Locatie details]", Globals.LocationColor);
            ColorConsole.WriteColorLine($"[Naam locatie: ]{location.Name}\n", Globals.LocationColor);

        
        }   
        
    }
}
