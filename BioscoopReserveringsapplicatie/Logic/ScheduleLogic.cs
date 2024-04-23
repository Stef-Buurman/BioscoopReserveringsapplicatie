using System.Globalization;

namespace BioscoopReserveringsapplicatie
{
    class ScheduleLogic
    {
        private static ExperiencesLogic experiencesLogic = new ExperiencesLogic();
        private static LocationLogic locationLogic = new LocationLogic();
        private static RoomLogic roomLogic = new RoomLogic();

        private List<ScheduleModel> _Schedules;
        private IDataAccess<ScheduleModel> _DataAccess = new DataAccess<ScheduleModel>();
        public ScheduleLogic(IDataAccess<ScheduleModel> dataAccess = null)
        {
            if (dataAccess != null) _DataAccess = dataAccess;
            else _DataAccess = new DataAccess<ScheduleModel>();

            _Schedules = _DataAccess.LoadAll();
        }

        public List<ScheduleModel> GetAll() => _Schedules = _DataAccess.LoadAll();

        public ScheduleModel? GetById(int id) => _DataAccess.LoadAll().Find(i => i.Id == id);

        public List<ScheduleModel> GetByLocationId(int id) => _DataAccess.LoadAll().FindAll(i => i.LocationId == id);

        public List<ScheduleModel> GetByRoomId(int id) => _DataAccess.LoadAll().FindAll(i => i.RoomId == id);

        public List<ScheduleModel> GetByExperienceId(int id) => _DataAccess.LoadAll().FindAll(i => i.ExperienceId == id);

        public bool Add(int experienceId, int roomId, int locationId, string scheduledDateTime)
        {
            if (UserLogic.CurrentUser != null && UserLogic.CurrentUser.IsAdmin)
            {
                if (DateTime.TryParseExact(scheduledDateTime, "dd-MM-yyyy HH:mm", CultureInfo.GetCultureInfo("nl-NL"), DateTimeStyles.None, out DateTime dateTimeStart))
                {
                    DateTime dateTimeEnd = dateTimeStart.AddMinutes(experiencesLogic.GetById(experienceId).TimeLength);

                    GetAll();

                    ScheduleModel schedule = new ScheduleModel(IdGenerator.GetNextId(_Schedules), experienceId, locationId, roomId, dateTimeStart, dateTimeEnd);

                    UpdateList(schedule);
                    return true;
                }
                else return false;
            }
            else return false;
        }

        public bool TimeSlotOpenOnRoom(int experienceId, int locationId, int roomId,  string scheduledDateTime, out string error)
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
            return schedules.FindAll(s => s.ExperienceId == id && s.LocationId == locationId);
        }

        public List<ScheduleModel> GetScheduledExperienceTimeSlotsForLocationById(int id, int locationId, DateTime? dateTime)
        {
            List<ScheduleModel> schedules = _DataAccess.LoadAll();
            return schedules.FindAll(s => s.ExperienceId == id && s.LocationId == locationId && s.ScheduledDateTime.Date == dateTime.Value.Date);
        }

        public ScheduleModel GetRoomForScheduledExperience(int id, int locationId, DateTime? dateTime)
        {
            List<ScheduleModel> schedules = _DataAccess.LoadAll();
            return schedules.Find(s => s.ExperienceId == id && s.LocationId == locationId && s.ScheduledDateTime == dateTime);
        }

        public bool HasScheduledExperience(int id)
        {
            List<ScheduleModel> schedules = _DataAccess.LoadAll();
            return schedules.Exists(s => s.ExperienceId == id && s.ScheduledDateTime > DateTime.Now && s.ScheduledDateTime.Date < DateTime.Today.AddDays(8));
        }

        public int GetRelatedScheduledExperience(int experienceId, int location, DateTime? dateTime, int room)
        {
            List<ScheduleModel> schedules = _DataAccess.LoadAll();
            return schedules.Find(s => s.ExperienceId == experienceId && s.LocationId == location && s.ScheduledDateTime == dateTime && s.RoomId == room).Id;
        }

        public List<ScheduleModel> GetScheduledExperienceDatesForLocationById(int id, int? locationId)
        {
            List<ScheduleModel> schedules = ScheduleAccess.LoadAll();
            return schedules.FindAll(s => s.ExperienceId == id && s.LocationId == locationId);
        }

        public List<ScheduleModel> GetScheduledExperienceTimeSlotsForLocationById(int id, int? locationId, DateTime? date)
        {
            List<ScheduleModel> schedules = ScheduleAccess.LoadAll();
            return schedules.FindAll(s => s.ExperienceId == id && s.LocationId == locationId && s.ScheduledDateTime.Date == date);
        }

        public ScheduleModel GetRoomForScheduledExperience(int id, int? locationId, DateTime? date, TimeSpan? time)
        {
            List<ScheduleModel> schedules = ScheduleAccess.LoadAll();
            return schedules.Find(s => s.ExperienceId == id && s.LocationId == locationId && s.ScheduledDateTime.Date == date && s.ScheduledDateTime.TimeOfDay == time);
        }

        public bool HasScheduledExperience(int id)
        {
            List<ScheduleModel> schedules = ScheduleAccess.LoadAll();
            return schedules.Exists(s => s.ExperienceId == id && s.ScheduledDateTime > DateTime.Now && s.ScheduledDateTime.Date < DateTime.Today.AddDays(8));
        }

        public int GetRelatedScheduledExperience(int experienceId, int? location, DateTime dateTime, int? room)
        {
            List<ScheduleModel> schedules = _DataAccess.LoadAll();
            return schedules.Find(s => s.ExperienceId == experienceId && s.LocationId == location && s.ScheduledDateTime == dateTime && s.RoomId == room).Id;
        }

        public List<ScheduleModel> GetScheduledExperienceDatesForLocationById(int id, int? locationId)
        {
            List<ScheduleModel> schedules = ScheduleAccess.LoadAll();
            return schedules.FindAll(s => s.ExperienceId == id && s.LocationId == locationId);
        }

        public List<ScheduleModel> GetScheduledExperienceTimeSlotsForLocationById(int id, int? locationId, DateTime? date)
        {
            List<ScheduleModel> schedules = ScheduleAccess.LoadAll();
            return schedules.FindAll(s => s.ExperienceId == id && s.LocationId == locationId && s.ScheduledDateTime.Date == date);
        }

        public ScheduleModel GetRoomForScheduledExperience(int id, int? locationId, DateTime? date, TimeSpan? time)
        {
            List<ScheduleModel> schedules = ScheduleAccess.LoadAll();
            return schedules.Find(s => s.ExperienceId == id && s.LocationId == locationId && s.ScheduledDateTime.Date == date && s.ScheduledDateTime.TimeOfDay == time);
        }

        public bool HasScheduledExperience(int id)
        {
            List<ScheduleModel> schedules = ScheduleAccess.LoadAll();
            return schedules.Exists(s => s.ExperienceId == id && s.ScheduledDateTime > DateTime.Now && s.ScheduledDateTime.Date < DateTime.Today.AddDays(8));
        }

        public int GetRelatedScheduledExperience(int experienceId, int? location, DateTime dateTime, int? room)
        {
            List<ScheduleModel> schedules = ScheduleAccess.LoadAll();
            return schedules.Find(s => s.ExperienceId == experienceId && s.LocationId == location && s.ScheduledDateTimeStart == dateTime && s.RoomId == room).Id;
        }
    }
}