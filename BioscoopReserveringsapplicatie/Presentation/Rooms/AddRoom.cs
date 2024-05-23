namespace BioscoopReserveringsapplicatie
{
    public static class AddRoom
    {
        private static RoomLogic roomLogic = new RoomLogic();
        private static string roomNumber = "";
        private static string capacity = "";

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
                capacity = AskForCapacity();
                returnTo = "";
            }
            Print(roomNumber, capacity);
            RoomModel newRoom = new RoomModel(roomLogic.GetNextId(), locationId, Convert.ToInt32(roomNumber), Convert.ToInt32(capacity), Status.Active);
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
            new Option<string>("Verder gaan met aanpassen", () => { Start(locationId, "Capacity"); }),
            new Option<string>("Verlaten zonder op te slaan", () => { LocationOverview.Start(); }),
            };

            new SelectionMenuUtil<string>(options).Create();
        }

        private static string AskForRoomNumber()
        {
            ColorConsole.WriteColorLine("Zaal toevoegen\n", Globals.TitleColor);
            return ReadLineUtil.EnterValue("Vul het [Zaalnummer] van de zaal in: ", RoomOverview.Start);
        }

        private static string AskForCapacity()
        {
            return ReadLineUtil.EnterValue("Vul het [Capaciteit] van de zaal in: ", () => Start(0, "RoomNumber"), false, false);
        }

        private static void Print(string roomNumber, string capacity)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("[Zaal details]", Globals.RoomColor);
            ColorConsole.WriteColorLine($"[Zaalnummer: ]{roomNumber}", Globals.RoomColor);
            ColorConsole.WriteColorLine($"[Zaal capaciteit: ]{capacity}", Globals.RoomColor);
            HorizontalLine.Print();
            ColorConsole.WriteColorLine($"Wilt u deze [Zaal] toevoegen?\n", Globals.ColorInputcClarification);
        }
    }
}