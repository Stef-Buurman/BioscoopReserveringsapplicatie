namespace BioscoopReserveringsapplicatie
{
    public static class AddRoom
    {
        private static RoomLogic roomLogic = new RoomLogic();
        private static int _newRoomId = 0;
        private static int _selectedLocationId = 0;

        private static void AskForRoomId()
        {
            ColorConsole.WriteColorLine("Voer de ID van de nieuwe kamer in:", Globals.TitleColor);
            return ReadLineUtil.EnterValue("Vul de [ID] van de kamer in: ", RoomOverview.Start);
        }
    }
}