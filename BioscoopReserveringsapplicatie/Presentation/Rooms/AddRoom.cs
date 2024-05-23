namespace BioscoopReserveringsapplicatie
{
    public static class AddRoom
    {
        private static RoomLogic roomLogic = new RoomLogic();
        private static LocationLogic locationLogic = new LocationLogic();
        private static string roomNumber = "";
        private static RoomType roomType = RoomType.Undefined;

        public static void Start(int locationId, string returnTo = "")
        {
            Console.Clear();

            if (returnTo == "" || returnTo == "RoomNumber")
            {
                roomNumber = AskForRoomNumber(locationId);
                returnTo = "";
            }
            if (returnTo == "" || returnTo == "RoomType")
            {
                AskForRoomType();
                returnTo = "";
            }
            Print(roomNumber, roomType.GetDisplayName(), locationId);
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
                    ColorConsole.WriteColorLine("Er is een fout opgetreden tijdens het toevoegen van de zaal. Probeer het opnieuw.", Globals.ErrorColor);
                    ColorConsole.WriteColorLine("Druk op een [toets] om terug te gaan naar het gebruikersmenu.", Globals.ColorInputcClarification);
                    Console.ReadKey();
                    Start(locationId, "RoomNumber");
                }
            }),
            new Option<string>("Verder gaan met aanpassen", () => { Start(locationId, "RoomNumber"); }),
            new Option<string>("Verlaten zonder op te slaan", () => { LocationOverview.Start(); }),
            };

            new SelectionMenuUtil<string>(options).Create();
        }

        private static void WhatToDoWhenGoBack() => Start(0, "RoomNumber");

        private static string AskForRoomNumber(int locationId)
        {
            ColorConsole.WriteColorLine("Zaal toevoegen\n", Globals.TitleColor);
            while (true)
            {
                string enteredRoomNumber = ReadLineUtil.EnterValue("Vul het [Zaalnummer] van de zaal in: ", RoomOverview.Start);
                if (roomLogic.IsDuplicateRoomNumber(locationId, Convert.ToInt32(enteredRoomNumber)))
                {
                    Console.Clear();
                    ColorConsole.WriteColorLine("Dit zaalnummer bestaat al op deze locatie, kies een zaalnummer die nog niet bestaat op deze locatie.", Globals.ErrorColor);
                }
                else return enteredRoomNumber;
            }
        }

        private static void AskForRoomType()
        {
            List<RoomType> roomTypeenum = Globals.GetAllEnum<RoomType>();
            List<Option<RoomType>> roomTypeOption = new List<Option<RoomType>>();
            foreach (RoomType roomType in roomTypeenum)
            {
                roomTypeOption.Add(new Option<RoomType>(roomType, roomType.GetDisplayName()));
            }
            ColorConsole.WriteColorLine("Selecteer het [Zaaltype] van de zaal: ", Globals.ColorInputcClarification);
            roomType = new SelectionMenuUtil<RoomType>(roomTypeOption, 15, WhatToDoWhenGoBack, () => Start(0, "RoomType")).Create();
        }

        private static void Print(string roomNumber, string roomType, int locationId)
        {
            Console.Clear();

            LocationModel location = locationLogic.GetById(locationId);
            ColorConsole.WriteColorLine("[Zaal details]", Globals.RoomColor);
            ColorConsole.WriteColorLine($"[Locatie: ]{location.Name}", Globals.RoomColor);
            ColorConsole.WriteColorLine($"[Zaalnummer: ]{roomNumber}", Globals.RoomColor);
            ColorConsole.WriteColorLine($"[Zaaltype: ]{roomType}", Globals.RoomColor);
            HorizontalLine.Print();
            ColorConsole.WriteColorLine($"Wilt u deze [Zaal] toevoegen?\n", Globals.ColorInputcClarification);
        }
    }
}