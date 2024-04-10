namespace BioscoopReserveringsapplicatie
{
    class RoomLogic
    {
        private List<RoomModel> _Rooms;

        public RoomLogic() => _Rooms = RoomAccess.LoadAll();
        
        public List<RoomModel> GetAll() => _Rooms = RoomAccess.LoadAll();

        public RoomModel? GetById(int id) => RoomAccess.LoadAll().Find(i => i.Id == id);

        public List<RoomModel> GetByLocationId(int id) => RoomAccess.LoadAll().FindAll(i => i.LocationId == id);

        public void Add(int locationId, int roomNumber, int capacity)
        {
                GetAll();

                RoomModel room = new RoomModel(IdGenerator.GetNextId(_Rooms), locationId, roomNumber, capacity);

                UpdateList(room);
        }

        public void UpdateList(RoomModel room)
        {
            //Find if there is already an model with the same id
            int index = _Rooms.FindIndex(s => s.Id == room.Id);

            if (index != -1)
            {
                //update existing model
                _Rooms[index] = room;
            }
            else
            {
                //add new model
                _Rooms.Add(room);
            }
            RoomAccess.WriteAll(_Rooms);
        }
    }
}