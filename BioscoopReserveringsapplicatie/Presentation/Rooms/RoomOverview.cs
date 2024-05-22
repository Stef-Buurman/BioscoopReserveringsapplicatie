namespace BioscoopReserveringsapplicatie
{
    public static class RoomOverview
    {
        private static RoomLogic roomLogic = new RoomLogic();
        private static LocationLogic locationLogic = new LocationLogic();
        private static Func<RoomModel, string[]> RoomDataExtractor = ExtractRoomData;

        public static void Start()
        {
            Console.Clear();
            List<RoomModel> rooms = roomLogic.GetAll();

            if (rooms.Count == 0) PrintWhenNoRoomsFound("Er zijn geen zalen gevonden."); 
            else ShowRooms(rooms);
        }

        private static void ShowRooms(List<RoomModel> rooms)
        {
            Console.Clear();
            List<Option<int>> options = new List<Option<int>>();

            List<string> columnHeaders = new List<string>
            {
                "Locatie",
                "Zaalnummer",
                "Capaciteit",
                //"Status",
            };

            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, rooms, RoomDataExtractor);

            foreach (RoomModel room in rooms)
            {
                LocationModel location = locationLogic.GetById(room.LocationId);
                string roomInfo = string.Format("{0,-" + (columnWidths[0] + 2) + "} {1,-" + (columnWidths[1] + 2) + "} {2,-" + (columnWidths[2] + 2) + "}", 
                location.Name, room.RoomNumber, room.Capacity);
                //room.Status.GetDisplayName()
                options.Add(new Option<int>(room.Id, roomInfo));
            }
            ColorConsole.WriteLineInfo("*Klik op escape om dit onderdeel te verlaten*\n");
            ColorConsole.WriteLineInfo("Klik op T om een zaal toe te voegen.\n");
            ColorConsole.WriteColorLine("Dit zijn alle zalen die momenteel bestaan:\n", Globals.TitleColor);
            Print();
            int roomId = new SelectionMenuUtil<int>(options,
            () => 
            {
                AdminMenu.Start();
            },
            () => 
            {
                Start();
            },
            new List<KeyAction>(){
                //new KeyAction(ConsoleKey.T, () => AddRoom.Start())
            },
            showEscapeabilityText: false).Create();

            ShowRoomDetails(roomId);
        }

        private static void ShowRoomDetails(int roomId)
        {
            if (roomId == 0)
            {
                RoomDetails.Start(roomId);
            }
        }

        private static void PrintWhenNoRoomsFound(string message)
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
            new SelectionMenuUtil<string>(options).Create();
        }

        private static void Print()
        {
            List<string> columnHeaders = new List<string>
            {
                "Locatie",
                "Zaalnummer",
                "Capaciteit",
                //"Status",
            };

            List<RoomModel> allRooms = roomLogic.GetAll();
            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, allRooms, RoomDataExtractor);

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
        
        private static string[] ExtractRoomData(RoomModel room)
        {
            LocationModel location = locationLogic.GetById(room.LocationId);

            string[] roomInfo = {
                location.Name,
                room.RoomNumber.ToString(),
                room.Capacity.ToString(),
                //room.Status.GetDisplayName()
            };

            return roomInfo;
        }
        
    }
}