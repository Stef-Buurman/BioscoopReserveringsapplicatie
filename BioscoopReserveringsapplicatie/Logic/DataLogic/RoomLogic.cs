namespace BioscoopReserveringsapplicatie
{
    public class RoomLogic : ILogic<RoomModel>
    {
        private List<RoomModel> _Rooms;
        public IDataAccess<RoomModel> _DataAccess { get; }
        public RoomLogic(IDataAccess<RoomModel> dataAccess = null)
        {
            if (dataAccess != null) _DataAccess = dataAccess;
            else _DataAccess = new DataAccess<RoomModel>();

            _Rooms = _DataAccess.LoadAll();
        }

        public int GetNextId() => IdGenerator.GetNextId(_Rooms);

        public List<RoomModel> GetAll() => _Rooms = _DataAccess.LoadAll();

        public RoomModel? GetById(int id) => _DataAccess.LoadAll().Find(i => i.Id == id);

        public List<RoomModel> GetByLocationId(int id) => _DataAccess.LoadAll().FindAll(i => i.LocationId == id);

        public bool Validate(RoomModel room)
        {
            if (room == null) return false;
            else if (!ValidateRoomType(room.RoomType)) return false;
            else if (!ValidateRoomNumber(room.RoomNumber)) return false;
            return true;
        }

        public bool ValidateRoomType(RoomType roomType) => (!Enum.IsDefined(typeof(RoomType), roomType)) ? false : true;
        public bool ValidateRoomNumber(int roomNumber) => roomNumber < 0 ? false : true;

        public bool IsDuplicateRoomNumber(int locationId, int roomNumber)
        {
            return _Rooms.Any(r => r.LocationId == locationId && r.RoomNumber == roomNumber);
        }

        public bool Add(RoomModel room)
        {
            GetAll();
            if (!Validate(room))
            {
                return false;
            }
            UpdateList(room);
            return true;
        }

        public bool Edit(RoomModel room)
        {
            if (!Validate(room))
            {
                return false;
            }

            UpdateList(room);
            return true;
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

        public void Archive(int id)
        {
            RoomModel? room = GetById(id);
            if (room != null)
            {
                room.Status = Status.Archived;
                _DataAccess.WriteAll(_Rooms);
                UpdateList(room);
            }
            else
            {
                return;
            }
        }

        public void Unarchive(int id)
        {
            RoomModel? room = GetById(id);
            if (room != null)
            {
                room.Status = Status.Active;
                _DataAccess.WriteAll(_Rooms);
                UpdateList(room);
            }
            else
            {
                return;
            }
        }
    }
}