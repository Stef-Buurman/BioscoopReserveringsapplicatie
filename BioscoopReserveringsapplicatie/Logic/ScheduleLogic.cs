using System.Dynamic;
using System.Globalization;

namespace BioscoopReserveringsapplicatie
{
    class ScheduleLogic
    {
        private List<ScheduleModel> _Schedules;

        public ScheduleLogic() => _Schedules = ScheduleAccess.LoadAll();

        public List<ScheduleModel> GetAll() => _Schedules = ScheduleAccess.LoadAll();

        public ScheduleModel? GetById(int id) => ScheduleAccess.LoadAll().Find(i => i.Id == id);

        public List<ScheduleModel> GetByLocationId(int id) => ScheduleAccess.LoadAll().FindAll(i => i.LocationId == id);

        public List<ScheduleModel> GetByRoomId(int id) => ScheduleAccess.LoadAll().FindAll(i => i.RoomId == id);

        public List<ScheduleModel> GetByExperienceId(int id) => ScheduleAccess.LoadAll().FindAll(i => i.ExperienceId == id);

        public bool Add(int experienceId, int roomId, int locationId, string scheduledDateTime)
        {
            if (UserLogic.CurrentUser != null && UserLogic.CurrentUser.IsAdmin)
            {
                if (DateTime.TryParseExact(scheduledDateTime, "dd-MM-yyyy HH:mm", CultureInfo.GetCultureInfo("nl-NL"), DateTimeStyles.None, out DateTime dateTime))
                {
                    GetAll();

                    ScheduleModel schedule = new ScheduleModel(IdGenerator.GetNextId(_Schedules), experienceId, locationId, roomId, dateTime);

                    UpdateList(schedule);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else return false;
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
            ScheduleAccess.WriteAll(_Schedules);
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
            return schedules.Find(s => s.ExperienceId == experienceId && s.LocationId == location && s.ScheduledDateTime == dateTime && s.RoomId == room).Id;
        }
    }
}