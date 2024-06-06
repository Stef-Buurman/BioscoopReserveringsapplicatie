namespace BioscoopReserveringsapplicatie
{
    public class ReservationLogic : ILogic<ReservationModel>
    {
        private static ScheduleLogic scheduleLogic;
        private static bool isInitializedScheduleLogic = false;
        private List<ReservationModel> _reservations = new();
        public IDataAccess<ReservationModel> _DataAccess { get; }
        public ReservationLogic(IDataAccess<ReservationModel> dataAccess = null, IDataAccess<ScheduleModel> schedulelogic = null, ScheduleLogic schedulelogicComplete = null)
        {
            if (dataAccess != null) _DataAccess = dataAccess;
            else _DataAccess = new DataAccess<ReservationModel>();

            if(schedulelogicComplete != null)
            {
                scheduleLogic = schedulelogicComplete;
            }
            else if (!isInitializedScheduleLogic)
            {
                isInitializedScheduleLogic = true;
                if (schedulelogic != null && dataAccess != null) scheduleLogic = new ScheduleLogic(schedulelogic, reservationAccess: dataAccess);
                if (schedulelogic != null) scheduleLogic = new ScheduleLogic(schedulelogic);
                else scheduleLogic = new ScheduleLogic();
            }

            _reservations = _DataAccess.LoadAll();
        }

        public int GetNextId() => IdGenerator.GetNextId(_reservations);

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

        public List<ReservationModel> GetByUserId(int userId)
        {
            _reservations = _DataAccess.LoadAll();
            return _reservations.FindAll(s => s.UserId == userId);
        }

        public bool Complete(int scheduleId, int userId, List<(int, int)> seat)
        {
            GetAll();

            if (scheduleId != 0 && userId != 0)
            {
                ReservationModel reservation = new ReservationModel(IdGenerator.GetNextId(_reservations), scheduleId, userId, seat);
                return Add(reservation);
            }

            return false;
        }

        public bool Add(ReservationModel reservation)
        {
            if (!Validate(reservation))
            {
                return false;
            }

            UpdateList(reservation);
            return true;
        }

        public bool Edit(ReservationModel reservation)
        {
            // This will be done in the near future
            return true;
        }

        public bool Cancel(ReservationModel reservation)
        {
            if (reservation == null) return false;

            reservation.IsCanceled = true;
            UpdateList(reservation);
            foreach(ReservationModel res in _reservations)
            {
                if (res.Id == reservation.Id)
                {
                    res.IsCanceled = true;
                }
            }
            return true;
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
            return reservations.Exists(r => r.ScheduleId == scheduleId && r.UserId == userId && r.IsCanceled == false);
        }

        public bool HasUserAlreadyReservedScheduledExperienceOnDateTimeForLocation(int userId, DateTime date, int locationId)
        {
            List<ReservationModel> reservations = _DataAccess.LoadAll().FindAll(r => r.UserId == userId);

            foreach (ReservationModel reservation in reservations)
            {
                ScheduleModel schedule = scheduleLogic.GetById(reservation.ScheduleId);

                if (schedule.ScheduledDateTimeStart.Date == date.Date && schedule.ScheduledDateTimeStart.TimeOfDay <= date.TimeOfDay && schedule.ScheduledDateTimeEnd.TimeOfDay >= date.TimeOfDay && reservation.IsCanceled == false && schedule.LocationId == locationId)
                {
                    return true;
                }
            }

            return false;
        }

        public bool HasUserReservedAvailableOptionsForLocation(int experienceId, int locationId, int userId)
        {
            List<ReservationModel> reservations = _DataAccess.LoadAll().FindAll(r => r.UserId == userId);
            List<ScheduleModel> schedules = scheduleLogic.GetScheduledExperiencesByLocationId(experienceId, locationId);

            foreach (ScheduleModel schedule in schedules)
            {
                if (!reservations.Exists(r => r.ScheduleId == schedule.Id && r.IsCanceled == false))
                {
                    return false;
                }
            }
            return true;
        }

        public List<(int, int)> GetAllReservedSeatsOfSchedule(int scheduleId) => GetAllReservationsByScheduleId(scheduleId).SelectMany(reservation => reservation.IsCanceled ? null : reservation.Seat).ToList();
        
        public List<ReservationModel> GetAllReservationsByScheduleId(int scheduleId) => _reservations.FindAll(r => r.ScheduleId == scheduleId && !r.IsCanceled);
    }
}