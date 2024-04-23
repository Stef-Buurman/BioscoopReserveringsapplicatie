namespace BioscoopReserveringsapplicatie
{
    class RoomLogic
    {
        private List<RoomModel> _Rooms;
        private IDataAccess<RoomModel> _DataAccess = new DataAccess<RoomModel>();
        public RoomLogic(IDataAccess<RoomModel> dataAccess = null)
        {
            if (dataAccess != null) _DataAccess = dataAccess;
            else _DataAccess = new DataAccess<RoomModel>();

            _Rooms = _DataAccess.LoadAll();
        }
        
        public List<RoomModel> GetAll() => _Rooms = _DataAccess.LoadAll();

        public RoomModel? GetById(int id) => _DataAccess.LoadAll().Find(i => i.Id == id);

        public List<RoomModel> GetByLocationId(int id) => _DataAccess.LoadAll().FindAll(i => i.LocationId == id);

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
            _DataAccess.WriteAll(_Rooms);
        }
    }
}