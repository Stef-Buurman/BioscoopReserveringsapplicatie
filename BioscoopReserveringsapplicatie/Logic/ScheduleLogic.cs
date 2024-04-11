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
            if(UserLogic.CurrentUser != null && UserLogic.CurrentUser.IsAdmin)
            {
                DateTime dateTime = DateTime.ParseExact(scheduledDateTime, "dd-M-yyyy hh:mm", CultureInfo.GetCultureInfo("nl-NL"));

                GetAll();

                ScheduleModel schedule = new ScheduleModel(IdGenerator.GetNextId(_Schedules), experienceId, locationId, roomId, dateTime);

                UpdateList(schedule);
                return true;
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
    }
}