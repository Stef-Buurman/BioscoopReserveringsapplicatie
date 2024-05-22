namespace BioscoopReserveringsapplicatie
{
    public static class RoomDetails
    {
        private static RoomLogic roomLogic = new RoomLogic();
        private static RoomModel? room;
        private static LocationLogic locationLogic = new LocationLogic();
        private static LocationModel? location;

        public static void Start(int roomId)
        {
            Console.Clear();
            room = roomLogic.GetById(roomId);

            location = locationLogic.GetById(room.LocationId);

            if (room == null)
            {
                ColorConsole.WriteColorLine("Zaal niet gevonden.", Globals.ErrorColor);
                RoomOverview.Start();
                return;
            }

            List<Option<string>> options;
            if (room.Status == Status.Archived)
            {
                options = new List<Option<string>>
                {
                    //new Option<string>("Dearchiveer zaal", () => RoomArchive.Start(room.Id)),
                    new Option<string>("Terug", () => {Console.Clear(); RoomOverview.Start();}),
                };
            }
            else
            {
                options = new List<Option<string>>
                {
                    //new Option<string>("Archiveer zaal", () => RoomArchive.Start(room.Id)),
                    new Option<string>("Terug", () => {Console.Clear(); RoomOverview.Start();}),
                };
            }
            Print();
            new SelectionMenuUtil<string>(options).Create();
        }

        private static void Print()
        {
            if (room != null)
            {
                ColorConsole.WriteColorLine("[Zaal details]", Globals.RoomColor);
                ColorConsole.WriteColorLine($"[Locatie: ]{location.Name}", Globals.RoomColor);
                ColorConsole.WriteColorLine($"[Zaalnummer: ]{room.RoomNumber}", Globals.RoomColor);
                ColorConsole.WriteColorLine($"[Capaciteit: ]{room.Capacity}\n\n", Globals.RoomColor);
                Console.WriteLine("Wat wil je doen?");
            }
        }
    }
}