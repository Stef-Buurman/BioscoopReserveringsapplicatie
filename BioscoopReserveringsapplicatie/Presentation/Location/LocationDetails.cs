namespace BioscoopReserveringsapplicatie
{
    static class LocationDetails
    {
        private static LocationLogic LocationLogic = new LocationLogic();
        private static LocationModel? location;

        public static void Start(int movieId)
        {
            Console.Clear();
            location = LocationLogic.GetById(movieId);
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
                    new Option<string>("Bewerk Locatie", () => LocationEdit.Start(location.Id)),
                    new Option<string>("Archiveer Locatie", () => LocationArchive.Start(location.Id)),
                    new Option<string>("Terug", () => {Console.Clear(); LocationOverview.Start();}),
                };
            }
            Print();
            new SelectionMenuUtil<string>(options).Create();
        }

        private static void Print()
        {
            ColorConsole.WriteColorLine("[Locatie details]", Globals.LocationColor);
            ColorConsole.WriteColorLine($"[naam Locatie: ]{location.Name}\n", Globals.LocationColor);

        
        }   
        
    }
}
