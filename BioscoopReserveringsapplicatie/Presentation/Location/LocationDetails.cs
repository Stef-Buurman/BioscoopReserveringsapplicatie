namespace BioscoopReserveringsapplicatie
{
    static class LocationDetails
    {
        private static LocationLogic LocationLogic = new LocationLogic();
        private static RoomLogic RoomLogic = new RoomLogic();
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
                    new Option<string>("Dearchiveer locatie", () => LocationArchive.Start(location.Id)),
                    new Option<string>("Terug", () => {Console.Clear(); LocationOverview.Start();}),
                };
            }
            else
            {
                options = new List<Option<string>>
                {
                    new Option<string>("Archiveer locatie", () => LocationArchive.Start(location.Id)),
                    new Option<string>("Zaal toevoegen", () => AddRoom.Start(location.Id)),
                    new Option<string>("Terug", () => {Console.Clear(); LocationOverview.Start();}),
                };
            }
            Print();
            Console.WriteLine();
            new SelectionMenuUtil<string>(options).Create();
        }

        private static void Print()
        {
            ColorConsole.WriteColorLine("[Locatie details]", Globals.LocationColor);
            ColorConsole.WriteColorLine($"[Naam locatie: ]{location.Name}", Globals.LocationColor);
            ColorConsole.WriteColorLine($"[Status: ]{location.Status}\n", Globals.LocationColor);
            ColorConsole.WriteColorLine($"[Zalen: ]", Globals.LocationColor);
            List<RoomModel> rooms = RoomLogic.GetByLocationId(location.Id);
            foreach (RoomModel room in rooms)
            {
                ColorConsole.WriteColorLine($"[Zaalnummer: ]{room.RoomNumber} | [Type: ]{room.RoomType.GetDisplayName()}", Globals.RoomColor);
            }
            HorizontalLine.Print();
        }   
        
    }
}