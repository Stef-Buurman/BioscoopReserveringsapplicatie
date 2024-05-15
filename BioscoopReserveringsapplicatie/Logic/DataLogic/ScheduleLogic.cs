using System.Globalization;

namespace BioscoopReserveringsapplicatie
{
    public class ScheduleLogic : ILogic<ScheduleModel>
    {
        private static ExperienceLogic experiencesLogic = new ExperienceLogic();
        private static LocationLogic locationLogic = new LocationLogic();
        private static RoomLogic roomLogic = new RoomLogic();
        private static ReservationLogic ReservationLogic = new ReservationLogic();

        private List<ScheduleModel> _Schedules;
        public IDataAccess<ScheduleModel> _DataAccess { get; }
        public ScheduleLogic(IDataAccess<ScheduleModel> dataAccess = null)
        {
            if (dataAccess != null) _DataAccess = dataAccess;
            else _DataAccess = new DataAccess<ScheduleModel>();

            _Schedules = _DataAccess.LoadAll();
        }

        public int GetNextId() => IdGenerator.GetNextId(_Schedules);

        public List<ScheduleModel> GetAll() => _Schedules = _DataAccess.LoadAll();

        public ScheduleModel? GetById(int id) => _DataAccess.LoadAll().Find(i => i.Id == id);

        public List<ScheduleModel> GetByLocationId(int id) => _DataAccess.LoadAll().FindAll(i => i.LocationId == id);

        public List<ScheduleModel> GetByRoomId(int id) => _DataAccess.LoadAll().FindAll(i => i.RoomId == id);

        public List<ScheduleModel> GetByExperienceId(int id) => _DataAccess.LoadAll().FindAll(i => i.ExperienceId == id);

        public ScheduleModel CreateSchedule(int experienceId, int roomId, int locationId, string scheduledDateTime)
        {
            if (UserLogic.IsAdmin())
            {
                if (DateTime.TryParseExact(scheduledDateTime, "dd-MM-yyyy HH:mm", CultureInfo.GetCultureInfo("nl-NL"), DateTimeStyles.None, out DateTime dateTimeStart))
                {
                    DateTime dateTimeEnd = dateTimeStart.AddMinutes(experiencesLogic.GetById(experienceId).TimeLength);
                    ScheduleModel schedule = new ScheduleModel(IdGenerator.GetNextId(_Schedules), experienceId, locationId, roomId, dateTimeStart, dateTimeEnd);
                    return schedule;
                }
            }
            return null;
        }

        public bool Validate(ScheduleModel schedule) => schedule != null;

        public bool Add(ScheduleModel schedule)
        {
            if (UserLogic.IsAdmin())
            {
                GetAll();

                if (!Validate(schedule))
                {
                    return false;
                }

                UpdateList(schedule);
                return true;
            }
            return false;
        }

        public bool Edit(ScheduleModel schedule)
        {
            if (UserLogic.IsAdmin())
            {
                if (!Validate(schedule))
                {
                    return false;
                }

                UpdateList(schedule);
                return true;
            }
            return false;
        }

        public bool TimeSlotOpenOnRoom(int experienceId, int locationId, int roomId, string scheduledDateTime, out string error)
        {

            if (DateTime.TryParseExact(scheduledDateTime, "dd-MM-yyyy HH:mm", CultureInfo.GetCultureInfo("nl-NL"), DateTimeStyles.None, out DateTime dateTimeStart))
            {
                DateTime dateTimeEnd = dateTimeStart.AddMinutes(experiencesLogic.GetById(experienceId).TimeLength);

                List<ScheduleModel> bookedSlots = GetByRoomId(roomId);
                foreach (ScheduleModel bookedSlot in bookedSlots)
                {
                    if (bookedSlot.ScheduledDateTimeStart >= dateTimeStart && bookedSlot.ScheduledDateTimeStart <= dateTimeEnd ||
                        bookedSlot.ScheduledDateTimeEnd >= dateTimeStart && bookedSlot.ScheduledDateTimeEnd <= dateTimeEnd)
                    {
                        error = $"Er is al een experience ingepland op {dateTimeStart.ToString("dd-MM-yyyy")} in {locationLogic.GetById(locationId).Name} Zaal: {roomLogic.GetById(roomId).RoomNumber} van {bookedSlot.ScheduledDateTimeStart.ToString("HH:mm")} T/M {bookedSlot.ScheduledDateTimeEnd.ToString("HH:mm")}";
                        return false;
                    }
                }
            }
            else
            {
                error = "Tijd format niet correct!";
                return false;
            }

            error = "";
            return true;
        }

        public void UpdateList(ScheduleModel schedule)
        {
            //Find if there is already an model with the same id
            int index = _Schedules.FindIndex(s => s.Id == schedule.Id);

            if (index != -1)
            {
                //update existing model
                _Schedules[index] = schedule;
            }
            else
            {
                //add new model
                _Schedules.Add(schedule);
            }
            _DataAccess.WriteAll(_Schedules);
        }

        public List<ScheduleModel> GetScheduledExperienceDatesForLocationById(int id, int locationId)
        {
            List<ScheduleModel> schedules = _DataAccess.LoadAll();
            return schedules.FindAll(s => s.ExperienceId == id && s.LocationId == locationId && s.ScheduledDateTimeStart.Date > DateTime.Today.AddDays(-1));
        }

        public List<ScheduleModel> GetScheduledExperienceTimeSlotsForLocationById(int id, int locationId, DateTime? dateTime)
        {
            List<ScheduleModel> schedules = _DataAccess.LoadAll();
            return schedules.FindAll(s => s.ExperienceId == id && s.LocationId == locationId && s.ScheduledDateTimeStart.Date == dateTime.Value.Date && s.ScheduledDateTimeStart > DateTime.Now && ReservationLogic.HasUserAlreadyReservedScheduledExperienceOnDateTime(UserLogic.CurrentUser.Id, s.ScheduledDateTimeStart) == false);
        }

        public ScheduleModel GetRoomForScheduledExperience(int id, int locationId, DateTime? dateTime)
        {
            List<ScheduleModel> schedules = _DataAccess.LoadAll();
            return schedules.Find(s => s.ExperienceId == id && s.LocationId == locationId && s.ScheduledDateTimeStart == dateTime);
        }

        public bool HasScheduledExperience(int id)
        {
            List<ScheduleModel> schedules = _DataAccess.LoadAll();
            return schedules.Exists(s => s.ExperienceId == id && s.ScheduledDateTimeStart > DateTime.Now && s.ScheduledDateTimeStart.Date < DateTime.Today.AddDays(8));
        }

        public int GetRelatedScheduledExperience(int experienceId, int? location, DateTime? dateTime, int? room)
        {
            List<ScheduleModel> schedules = _DataAccess.LoadAll();
            return schedules.Find(s => s.ExperienceId == experienceId && s.LocationId == location && s.ScheduledDateTimeStart == dateTime && s.RoomId == room).Id;
        }

        public List<ScheduleModel> GetScheduledExperienceDatesForLocationById(int id, int? locationId)
        {
            List<ScheduleModel> schedules = _DataAccess.LoadAll();
            return schedules.FindAll(s => s.ExperienceId == id && s.LocationId == locationId);
        }

        public List<ScheduleModel> GetScheduledExperienceTimeSlotsForLocationById(int id, int? locationId, DateTime? date)
        {
            List<ScheduleModel> schedules = _DataAccess.LoadAll();
            return schedules.FindAll(s => s.ExperienceId == id && s.LocationId == locationId && s.ScheduledDateTimeStart.Date == date);
        }

        public ScheduleModel GetRoomForScheduledExperience(int id, int? locationId, DateTime? date, TimeSpan? time)
        {
            List<ScheduleModel> schedules = _DataAccess.LoadAll();
            return schedules.Find(s => s.ExperienceId == id && s.LocationId == locationId && s.ScheduledDateTimeStart.Date == date && s.ScheduledDateTimeStart.TimeOfDay == time);
        }
    }
}