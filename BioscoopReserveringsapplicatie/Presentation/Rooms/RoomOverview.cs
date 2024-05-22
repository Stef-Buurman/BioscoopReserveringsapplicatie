namespace BioscoopReserveringsapplicatie
{
    public static class RoomOverview
    {
        private static RoomLogic roomLogic = new RoomLogic();

        public static void Start()
        {
            Console.Clear();
            List<RoomModel> rooms = roomLogic.GetAll();

            if (rooms.Count == 0)
            {
                ColorConsole.WriteColorLine("Er zijn geen kamers gevonden.", Globals.TitleColor); 
            }
            else
            {
                ShowRooms(rooms);
            }
        }

        private static void ShowRooms(List<RoomModel> rooms)
        {
            Console.Clear();
        }
        
    }
}