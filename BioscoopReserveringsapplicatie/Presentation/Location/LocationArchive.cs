namespace BioscoopReserveringsapplicatie
{
    public static class LocationArchive
    {
        private static LocationLogic locationLogic = new LocationLogic();
        public static void Start(int LocationId)
        {
            LocationModel? location = locationLogic.GetById(LocationId);
            if (location == null) return;

            if (location.Status == Status.Archived)
            {
                List<Option<string>> options2 = new List<Option<string>>
                        {
                            new Option<string>("Ja", () => {
                                locationLogic.UnArchive(LocationId);
                                LocationDetails.Start(LocationId);
                            }),
                            new Option<string>("Nee", () => {
                                LocationDetails.Start(LocationId);
                            }),
                        };
                ColorConsole.WriteColorLine("\n----------------------------------------------------------------", Globals.ErrorColor);
                ColorConsole.WriteColorLine("Weet u zeker dat u deze locatie wilt dearchiveren?", Globals.ErrorColor);
                string selectionMenu2 = new SelectionMenuUtil<string>(options2, new Option<string>("Nee")).Create();
            }
            else
            {
                List<Option<string>> options2 = new List<Option<string>>
                        {
                            new Option<string>("Ja", () => {
                                locationLogic.Archive(LocationId);
                                LocationDetails.Start(LocationId);
                            }),
                            new Option<string>("Nee", () => {
                                LocationDetails.Start(LocationId);
                            }),
                        };
                ColorConsole.WriteColorLine("\n----------------------------------------------------------------", Globals.ErrorColor);
                ColorConsole.WriteColorLine("Weet u zeker dat u deze locatie wilt archiveren?", Globals.ErrorColor);
                string selectionMenu2 = new SelectionMenuUtil<string>(options2, new Option<string>("Nee")).Create();
            }
        }
    }
}