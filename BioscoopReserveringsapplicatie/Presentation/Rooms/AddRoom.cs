namespace BioscoopReserveringsapplicatie
{
    public static class AddRoom
    {
        private static RoomLogic roomLogic = new RoomLogic();
        private static LocationLogic locationLogic = new LocationLogic();
        private static int roomNumber = 0;
        private static int capacity = 0;

        public static void Start(int locationId, string returnTo = "")
        {
            Console.Clear();
            

            if (returnTo == "" || returnTo == "RoomNumber")
            {
                roomNumber = AskForRoomNumber();
                returnTo = "";
            }
            if (returnTo == "" || returnTo == "Capacity")
            {
                roomNumber = AskForCapacity();
                returnTo = "";
            }
            Print(roomNumber.ToString(), capacity.ToString());
            RoomModel newRoom = new RoomModel(roomLogic.GetNextId(), locationId, roomNumber, capacity, Status.Active);
            List<Option<string>> options = new List<Option<string>>
            {
            new Option<string>("Opslaan en verlaten", () => 
            {
                if (roomLogic.Add(newRoom))
                {
                    LocationOverview.Start();
                }
                else
                {
                    Console.Clear();
                    Console.WriteLine("Er is een fout opgetreden tijdens het toevoegen van de zaal. Probeer het opnieuw.\n");
                    Start(locationId, "RoomNumber");
                }
            }),
            new Option<string>("Verder gaan met aanpassen", () => { Start(locationId, "RoomNumber"); }),
            new Option<string>("Verlaten zonder op te slaan", () => { LocationOverview.Start(); }),
            };

            new SelectionMenuUtil<string>(options).Create();
        }

        private static int AskForRoomNumber()
        {
            string input = ReadLineUtil.EnterValue("Vul het [Zaalnummer] van de zaal in: ", RoomOverview.Start);
            int output = Convert.ToInt32(input);
            return output;
        }

        private static int AskForCapacity()
        {
            string input = ReadLineUtil.EnterValue("Vul de [Capaciteit] van de zaal in: ", () => Start(0, "RoomNumber"), false, false);
            int output = Convert.ToInt32(input);
            return output;
        }

        private static void Print(string roomNumber, string capacity)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("[Zaal details]", Globals.RoomColor);
            ColorConsole.WriteColorLine($"[Zaalnummer: ]{roomNumber}", Globals.RoomColor);
            ColorConsole.WriteColorLine($"[Zaal capaciteit: ]{capacity}", Globals.RoomColor);
            HorizontalLine.Print();
            ColorConsole.WriteColorLine($"Wilt u deze [Zaal] toevoegen?", Globals.ColorInputcClarification);
        }
    }
}