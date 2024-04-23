namespace BioscoopReserveringsapplicatie
{
    public class ReservationLogic
    {
        private List<ReservationModel> _reservations = new();
        private IDataAccess<ReservationModel> _DataAccess = new DataAccess<ReservationModel>();
        public ReservationLogic(IDataAccess<ReservationModel> dataAccess = null)
        {
            if (dataAccess != null) _DataAccess = dataAccess;
            else _DataAccess = new DataAccess<ReservationModel>();

            _reservations = _DataAccess.LoadAll();
        }

        public List<ReservationModel> GetAll()
        {
            _reservations = _DataAccess.LoadAll();
            return _reservations;
        }

        public ReservationModel? GetById(int id)
        {
            _reservations = _DataAccess.LoadAll();
            return _reservations.Find(s => s.Id == id);
        }

        public bool Complete(int scheduleId, int userId)
        {
            GetAll();

            if (scheduleId != 0 && userId != 0)
            {
                ReservationModel reservation = new ReservationModel(IdGenerator.GetNextId(_reservations), scheduleId, userId);

                if (this.Validate(reservation))
                {
                    UpdateList(reservation);
                    return true;
                }
            }

            return false;
        }

        public bool Validate(ReservationModel reservation)
        {
            if (reservation == null) return false;

            return true;
        }

        public void UpdateList(ReservationModel reservation)
        {
            //Find if there is already an model with the same id
            int index = _reservations.FindIndex(s => s.Id == reservation.Id);

            if (index != -1)
            {
                //update existing model
                _reservations[index] = reservation;
            }
            else
            {
                //add new model
                _reservations.Add(reservation);
            }
            _DataAccess.WriteAll(_reservations);
        }

        public bool HasUserAlreadyReservedScheduledExperience(int scheduleId, int userId)
        {
            List<ReservationModel> reservations = _DataAccess.LoadAll();
            return reservations.Exists(r => r.ScheduleId == scheduleId && r.UserId == userId);
        }
    }
}