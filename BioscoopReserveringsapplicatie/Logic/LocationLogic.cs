namespace BioscoopReserveringsapplicatie
{
    class LocationLogic
    {
        private List<LocationModel> _Locations;

        public LocationLogic() => _Locations = LocationAccess.LoadAll();
        
        public List<LocationModel> GetAll() => _Locations = LocationAccess.LoadAll();

        public LocationModel? GetById(int id) => LocationAccess.LoadAll().Find(i => i.Id == id);

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
            LocationAccess.WriteAll(_Locations);
        }
    }
}