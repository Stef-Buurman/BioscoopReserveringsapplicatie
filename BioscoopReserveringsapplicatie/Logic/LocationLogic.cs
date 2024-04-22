namespace BioscoopReserveringsapplicatie
{
    class LocationLogic
    {
        private List<LocationModel> _Locations;
        private IDataAccess<LocationModel> _DataAccess = new DataAccess<LocationModel>();
        private ScheduleLogic SheduleLogic;
        public LocationLogic(IDataAccess<LocationModel> dataAccess = null, IDataAccess<ScheduleModel> sheduleAccess = null)
        {
            if (dataAccess != null) _DataAccess = dataAccess;
            else _DataAccess = new DataAccess<LocationModel>();

            if (sheduleAccess != null) SheduleLogic = new ScheduleLogic(sheduleAccess);
            else SheduleLogic = new ScheduleLogic();

            _Locations = _DataAccess.LoadAll();
        }

        public List<LocationModel> GetAll() => _Locations = _DataAccess.LoadAll();

        public LocationModel? GetById(int id) => _DataAccess.LoadAll().Find(i => i.Id == id);

        public void Add(string name)
        {
            GetAll();

            LocationModel location = new LocationModel(IdGenerator.GetNextId(_Locations), name);

            UpdateList(location);
        }

        public void UpdateList(LocationModel location)
        {
            //Find if there is already an model with the same id
            int index = _Locations.FindIndex(s => s.Id == location.Id);

            if (index != -1)
            {
                //update existing model
                _Locations[index] = location;
            }
            else
            {
                //add new model
                _Locations.Add(location);
            }
            _DataAccess.WriteAll(_Locations);
        }

        public List<LocationModel> GetLocationsForScheduledExperienceById(int id)
        {
            List<ScheduleModel> schedules = SheduleLogic.GetAll();
            List<LocationModel> locations = new List<LocationModel>();

            foreach (ScheduleModel schedule in schedules)
            {
                if (schedule.ExperienceId == id)
                {
                    LocationModel location = new LocationLogic().GetById(schedule.LocationId);
                    if (location != null && !locations.Any(loc => loc.Id == location.Id))
                    {
                        locations.Add(location);
                    }
                }
            }

            return locations;
        }

        public List<LocationModel> GetLocationsForScheduledExperienceById(int id)
        {
            List<ScheduleModel> schedules = ScheduleAccess.LoadAll();
            List<LocationModel> locations = new List<LocationModel>();

            foreach (ScheduleModel schedule in schedules)
            {
                if (schedule.ExperienceId == id)
                {
                    LocationModel location = new LocationLogic().GetById(schedule.LocationId);
                    if (location != null && !locations.Any(loc => loc.Id == location.Id))
                    {
                        locations.Add(location);
                    }
                }
            }

            return locations;
        }
    }
}