namespace BioscoopReserveringsapplicatie
{
    public static class AddRoom
    {
        private static RoomLogic roomLogic = new RoomLogic();
        private static string roomNumber = "";
        private static RoomType roomType = RoomType.Undefined;

        public static void Start(int locationId, string returnTo = "")
        {
            Console.Clear();

            if (returnTo == "" || returnTo == "RoomNumber")
            {
                roomNumber = AskForRoomNumber();
                returnTo = "";
            }
            if (returnTo == "" || returnTo == "RoomType")
            {
                AskForRoomType();
                returnTo = "";
            }
            Print(roomNumber, roomType.GetDisplayName());
            RoomModel newRoom = new RoomModel(roomLogic.GetNextId(), locationId, Convert.ToInt32(roomNumber), roomType, Status.Active);
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
            new Option<string>("Verder gaan met aanpassen", () => { Start(locationId, "RoomType"); }),
            new Option<string>("Verlaten zonder op te slaan", () => { LocationOverview.Start(); }),
            };

            new SelectionMenuUtil<string>(options).Create();
        }

        private static void WhatToDoWhenGoBack() => Start(0, "RoomNumber");

        private static string AskForRoomNumber()
        {
            ColorConsole.WriteColorLine("Zaal toevoegen\n", Globals.TitleColor);
            return ReadLineUtil.EnterValue("Vul het [Zaalnummer] van de zaal in: ", RoomOverview.Start);
        }

        private static void AskForRoomType()
        {
            List<RoomType> roomTypeenum = Globals.GetAllEnum<RoomType>();
            List<Option<RoomType>> roomTypeOption = new List<Option<RoomType>>();
            foreach (RoomType roomType in roomTypeenum)
            {
                roomTypeOption.Add(new Option<RoomType>(roomType, roomType.GetDisplayName()));
            }
            ColorConsole.WriteColorLine("Selecteer het [Zaaltype] van de zaal in: ", Globals.ColorInputcClarification);
            roomType = new SelectionMenuUtil<RoomType>(roomTypeOption, 15, WhatToDoWhenGoBack, () => Start(0, "RoomType")).Create();
        }

        private static void Print(string roomNumber, string roomType)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("[Zaal details]", Globals.RoomColor);
            ColorConsole.WriteColorLine($"[Zaalnummer: ]{roomNumber}", Globals.RoomColor);
            ColorConsole.WriteColorLine($"[Zaaltype: ]{roomType}", Globals.RoomColor);
            HorizontalLine.Print();
            ColorConsole.WriteColorLine($"Wilt u deze [Zaal] toevoegen?\n", Globals.ColorInputcClarification);
        }
    }
}