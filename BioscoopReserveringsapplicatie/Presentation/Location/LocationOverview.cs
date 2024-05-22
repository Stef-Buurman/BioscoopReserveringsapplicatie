namespace BioscoopReserveringsapplicatie
{
    public static class LocationOverview
    {
        private static LocationLogic locationLogic = new LocationLogic();
        private static Func<LocationModel, string[]> locationDataExtractor = ExtractLocationData;

        public static void Start()
        {
            Console.Clear();

            List<LocationModel> locations = locationLogic.GetAll();

            if (locations.Count == 0) PrintWhenNoLocationsFound("Er zijn geen Locaties gevonden.");
            else ShowLocations(locations);
        }

        private static void ShowLocations(List<LocationModel> location)
        {
            Console.Clear();

            List<Option<int>> options = new List<Option<int>>();

            List<string> columnHeaders = new List<string>
            {
                "Naam locatie",
                "Status",
            };

            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, location, locationDataExtractor);

            foreach (LocationModel locations in location)
            {
                string locationName = locations.Name;
                if (locationName.Length > 25)
                {
                    locationName = locationName.Substring(0, 25) + "...";
                }

                string locationInfo = string.Format("{0,-" + (columnWidths[0] + 2) + "} {1,-" + (columnWidths[1] + 2) + "}", locationName, locations.Status.GetDisplayName());
                options.Add(new Option<int>(locations.Id, locationInfo));
            }
            ColorConsole.WriteLineInfo("*Klik op escape om dit onderdeel te verlaten*\n");
            ColorConsole.WriteLineInfo("Klik op T om een locatie toe te voegen.\n");
            ColorConsole.WriteColorLine("Dit zijn alle locaties die momenteel bestaan:\n", Globals.TitleColor);
            Print();
            int locationId = new SelectionMenuUtil<int>(options,
            () => 
            {
                AdminMenu.Start();
            },
            () => 
            {
                Start();
            },
            new List<KeyAction>(){ 
                new KeyAction(ConsoleKey.T, () => AddLocation.Start()) 
            },
            showEscapeabilityText: false).Create();

            ShowLocationDetails(locationId);
        }

        private static void ShowLocationDetails(int locationId)
        {
            if (locationId != 0)
            {
                LocationDetails.Start(locationId);
            }
        }

        private static void PrintWhenNoLocationsFound(string message)
        {
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ja", () => {
                   AddLocation.Start();
                }),
                new Option<string>("Nee", () => {
                    AdminMenu.Start();
                }),
            };
            Console.WriteLine(message);
            Console.WriteLine();
            Console.WriteLine("Wil je een locatie aanmaken?");
            new SelectionMenuUtil<string>(options).Create();
        }

        private static void Print()
        {
            List<string> columnHeaders = new List<string>
            {
                "Naam Locatie",
                "Status",
            };

            List<LocationModel> allLocations = locationLogic.GetAll();
            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, allLocations, locationDataExtractor);

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

        private static string[] ExtractLocationData(LocationModel location)
        {
            string[] locationInfo = {
                location.Name,
                location.Status.GetDisplayName(),
            };

            return locationInfo;
        }
    }
}