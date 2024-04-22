namespace BioscoopReserveringsapplicatie
{
    public class ExperiencesLogic
    {
        private List<ExperiencesModel> _experiences;

        private static MoviesLogic MoviesLogic = new MoviesLogic();
        private static ScheduleLogic ScheduleLogic = new ScheduleLogic();

        public ExperiencesLogic()
        {
            _experiences = ExperiencesAccess.LoadAll();
        }

        public ExperiencesModel GetById(int id)
        {
            _experiences = ExperiencesAccess.LoadAll();
            return _experiences.Find(i => i.Id == id);
        }

        public bool ValidateExperience(ExperiencesModel experience)
        {
            if (experience == null) return false;
            else if (!ValidateExperienceName(experience.Name)) return false;
            else if (!ValidateExperienceIntensity(experience.Intensity)) return false;
            else if (!ValidateExperienceTimeLength(experience.TimeLength)) return false;
            return true;
        }
        public bool ValidateExperienceName(string name) => (name == null || name == "") ? false : true;
        public bool ValidateExperienceTimeLength(int timeLength) => timeLength < 0 ? false : true;
        public bool ValidateExperienceIntensity(Intensity intensity) => (!Enum.IsDefined(typeof(Intensity), intensity)) ? false : true;
        public bool ValidateExperienceTimeLength(string timeLength) => (int.TryParse(timeLength, out int _)) ? true : false;
        public bool ValidateMovieId(int filmId) => MoviesLogic.GetMovieById(filmId) == null ? false : true;
        public bool ValidateExperienceArchive(bool archived) => true;

        public bool AddExperience(ExperiencesModel experience)
        {
            if (experience == null || !this.ValidateExperience(experience))
            {
                return false;
            }
            if (this.GetById(experience.Id) == null) experience.Id = IdGenerator.GetNextId(_experiences);
            _experiences.Add(experience);
            ExperiencesAccess.WriteAll(_experiences);
            return true;
        }

        public List<ExperiencesModel> GetExperiences()
        {
            _experiences = ExperiencesAccess.LoadAll();
            return _experiences;
        }

        public List<ExperiencesModel> GetExperiencesByUserPreferences(UserModel currentUser)
        {
            GetExperiences();

            List<ExperiencesModel> experiences = new List<ExperiencesModel>();

            foreach (ExperiencesModel experience in _experiences)
            {
                if (experience.Archived) continue;

                MovieModel movie = MoviesLogic.GetMovieById(experience.FilmId);

                if (movie == null) continue;

                bool genreMatch = currentUser.Genres.Count == 0 || movie.Genres.Intersect(currentUser.Genres).Any();
                bool ageMatch = currentUser.AgeCategory == AgeCategory.Undefined || Convert.ToInt32(movie.AgeCategory) <= Convert.ToInt32(currentUser.AgeCategory);
                bool intensityMatch = currentUser.Intensity == Intensity.Undefined || experience.Intensity == currentUser.Intensity;
                bool hasScheduldedExperience = ScheduleLogic.HasScheduledExperience(experience.Id);

                if (genreMatch && ageMatch && intensityMatch && hasScheduldedExperience)
                {
                    experiences.Add(experience);
                }
            }
            return experiences;
        }
        public bool EditExperience(int id, string name, Intensity intensity, int timeLength, int filmId)
        {


            if (ValidateExperienceName(name) && ValidateExperienceIntensity(intensity) && ValidateExperienceTimeLength(timeLength) && ValidateMovieId(filmId))
            {
                ExperiencesModel experience = GetById(id);
                experience.Name = name;
                experience.Intensity = intensity;
                experience.FilmId = filmId;
                experience.TimeLength = timeLength;

                UpdateList(experience);
                return true;
            }

            return false;
        }
        public void UpdateList(ExperiencesModel experience)
        {
            //Find if there is already an model with the same id
            int index = _experiences.FindIndex(s => s.Id == experience.Id);

            if (index != -1)
            {
                //update existing model
                _experiences[index] = experience;
            }
            else
            {
                //add new model
                _experiences.Add(experience);
            }
            ExperiencesAccess.WriteAll(_experiences);
        }

        public void ArchiveExperience(int id)
        {
            ExperiencesModel experience = GetById(id);
            if (experience != null)
            {
                experience.Archived = true;
                ExperiencesAccess.WriteAll(_experiences);
            }
            else
            {
                return;
            }
        }

        public List<ExperiencesModel> GetAllArchivedExperiences()
        {
            _experiences = ExperiencesAccess.LoadAll();
            return _experiences.FindAll(e => e.Archived);
        }

        public List<ExperiencesModel> GetAllActiveExperiences()
        {
            _experiences = ExperiencesAccess.LoadAll();
            return _experiences.FindAll(e => !e.Archived);
        }

        public bool HasScheduledExperience(int id)
        {
            List<ScheduleModel> schedules = ScheduleAccess.LoadAll();
            return schedules.Exists(s => s.ExperienceId == id && s.ScheduledDateTimeStart > DateTime.Now && s.ScheduledDateTimeStart.Date < DateTime.Today.AddDays(8));
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

        public int GetRelatedScheduledExperience(int experienceId, int? location, DateTime dateTime, int? room)
        {
            List<ScheduleModel> schedules = ScheduleAccess.LoadAll();
            return schedules.Find(s => s.ExperienceId == experienceId && s.LocationId == location && s.ScheduledDateTime == dateTime && s.RoomId == room).Id;
        }

        public bool HasCurrentUserAlreadyReservatedScheduledExperience(int scheduleId, int userId)
        {
            List<ReservationModel> reservations = ReservationAccess.LoadAll();
            return reservations.Exists(r => r.ScheduleId == scheduleId && r.UserId == userId);
        }
    }
}