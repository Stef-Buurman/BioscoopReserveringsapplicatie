using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace BioscoopReserveringsapplicatie
{
    class RoomLogic
    {
        private List<RoomModel> _Rooms;

        public RoomLogic() => _Rooms = RoomAccess.LoadAll();
        
        public List<RoomModel> GetAll() => _Rooms = RoomAccess.LoadAll();

        private int GenerateId() => _Rooms.Count == 0 ? 0 : _Rooms.Max(location => location.Id) + 1;

        public RoomModel? GetById(int id) => RoomAccess.LoadAll().Find(i => i.Id == id);

        public void Add(int locationId, int roomNumber, int capacity)
        {
                GetAll();

                RoomModel room = new RoomModel(GenerateId(), locationId, roomNumber, capacity);

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