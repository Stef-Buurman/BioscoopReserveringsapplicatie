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
                AskForRoomType(locationId);
                returnTo = "";
            }
            Print(roomNumber, roomType.GetDisplayName(), locationId);
            RoomModel newRoom = new RoomModel(roomLogic.GetNextId(), locationId, Convert.ToInt32(roomNumber), roomType, Status.Active);
            List<Option<string>> options = new List<Option<string>>
            {
            new Option<string>("Opslaan en terug naar overzicht", () => 
            {
                if (roomLogic.Add(newRoom))
                {
                    ColorConsole.WriteColorLine("\nDe zaal is toegevoegd aan de locatie.", Globals.SuccessColor);
                    Thread.Sleep(1750);
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
                List<int> intList = Enumerable.Range(1, 100).ToList();
                intList = intList.FindAll(x => !roomLogic.IsDuplicateRoomNumber(locationId, x));
                SelectionMenuUtil<int> selection = new SelectionMenuUtil<int>(intList, 1, () => LocationDetails.Start(locationId), () => AskForRoomNumber(locationId), false, "Vul het [Zaalnummer] van de zaal in: ", new Option<int>(1));
                return selection.Create().ToString();
            }
        }

        private static void AskForRoomType(int locationId)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("Zaal toevoegen\n", Globals.TitleColor);
            LocationModel? location = locationLogic.GetById(locationId);
            ColorConsole.WriteColorLine($"[Locatie: ]{location?.Name}", Globals.RoomColor);
            if (roomNumber != "") ColorConsole.WriteColorLine($"[Zaalnummer: ]{roomNumber}", Globals.RoomColor);
            HorizontalLine.Print();
            List<RoomType> roomTypeEnum = Globals.GetAllEnum<RoomType>();
            List<Option<RoomType>> roomTypeOption = new List<Option<RoomType>>();
            foreach (RoomType roomType in roomTypeEnum)
            {
                roomTypeOption.Add(new Option<RoomType>(roomType, roomType.GetDisplayName()));
            }
            ColorConsole.WriteColorLine("\nSelecteer het [Zaaltype] van de zaal: ", Globals.ColorInputcClarification);
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