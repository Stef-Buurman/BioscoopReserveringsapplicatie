using System.Globalization;

namespace BioscoopReserveringsapplicatie
{
    class ScheduleLogic
    {
        private static ExperiencesLogic experiencesLogic = new ExperiencesLogic();
        private static LocationLogic locationLogic = new LocationLogic();
        private static RoomLogic roomLogic = new RoomLogic();

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
            ScheduleAccess.WriteAll(_Schedules);
        }
    }
}