namespace BioscoopReserveringsapplicatie
{
    public static class RoomArchive
    {
        private static RoomLogic roomLogic = new RoomLogic();
        private static LocationLogic locationLogic = new LocationLogic();
        public static void Start(int roomId)
        {
            Console.Clear();
            RoomModel room = roomLogic.GetById(roomId);
            LocationModel location = locationLogic.GetById(room.LocationId);
            if (room == null) 
            {
                ColorConsole.WriteColorLine("Zaal niet gevonden.", Globals.ErrorColor);
                WaitUtil.WaitTime(2000);
                RoomOverview.Start();   
                return;
            }

            if (room.Status == Status.Active)
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Ja", () => {
                        roomLogic.Archive(roomId);
                        Console.Clear();
                        ColorConsole.WriteColorLine($"Zaal {room.RoomNumber} is gearchiveerd!", Globals.SuccessColor);
                        WaitUtil.WaitTime(2000);
                        RoomDetails.Start(roomId);
                    }),
                    new Option<string>("Nee", () => {
                        RoomDetails.Start(roomId);
                    }),
                };
                ColorConsole.WriteColorLine("De zaal details zijn:", Globals.RoomColor);
                ColorConsole.WriteColorLine($"[Locatie:] {location.Name}", Globals.RoomColor);
                ColorConsole.WriteColorLine($"[Zaanummer:] {room.RoomNumber}", Globals.RoomColor);
                ColorConsole.WriteColorLine($"[Zaaltype:] {room.RoomType.GetDisplayName()}\n", Globals.RoomColor);
                ColorConsole.WriteColorLine($"Weet u zeker dat u de zaal {room.RoomNumber} wilt [archiveren]?", Globals.ColorInputcClarification);
                new SelectionMenuUtil<string>(options, new Option<string>("Nee")).Create();
            }
            else
            {
                List<Option<string>> options = new List<Option<string>>
                {
                    new Option<string>("Ja", () => {
                        roomLogic.Unarchive(roomId);
                        Console.Clear();
                        ColorConsole.WriteColorLine($"Zaal {room.RoomNumber} is gedearchiveerd!", Globals.SuccessColor);
                        WaitUtil.WaitTime(4000);
                        RoomDetails.Start(roomId);
                    }),
                    new Option<string>("Nee", () => {
                        RoomDetails.Start(roomId);
                    }),
                };
                ColorConsole.WriteColorLine("De zaal details zijn:", Globals.RoomColor);
                ColorConsole.WriteColorLine($"[Locatie:] {location.Name}", Globals.RoomColor);
                ColorConsole.WriteColorLine($"[Zaanummer:] {room.RoomNumber}", Globals.RoomColor);
                ColorConsole.WriteColorLine($"[Zaaltype:] {room.RoomType.GetDisplayName()}\n", Globals.RoomColor);
                ColorConsole.WriteColorLine($"Weet u zeker dat u de zaal {room.RoomNumber} wilt [dearchiveren]?", Globals.ColorInputcClarification);
                new SelectionMenuUtil<string>(options, new Option<string>("Nee")).Create();
            }
        }
    }
}