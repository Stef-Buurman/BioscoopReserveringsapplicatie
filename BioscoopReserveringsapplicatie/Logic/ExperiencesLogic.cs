namespace BioscoopReserveringsapplicatie
{
    public class ExperiencesLogic
    {
        private List<ExperienceModel> _experiences;
        private IDataAccess<ExperienceModel> _DataAccess = new DataAccess<ExperienceModel>();
        private static MoviesLogic MoviesLogic;
        private static ScheduleLogic ScheduleLogic;

        public ExperiencesLogic(IDataAccess<ExperienceModel> experienceAccess = null,
            IDataAccess<MovieModel> movieAccess = null,
            IDataAccess<ScheduleModel> scheduleAccess = null)
        {
            if (experienceAccess != null) _DataAccess = experienceAccess;
            else _DataAccess = new DataAccess<ExperienceModel>();

            if (movieAccess != null) MoviesLogic = new MoviesLogic(movieAccess);
            else MoviesLogic = new MoviesLogic();

            if (scheduleAccess != null) ScheduleLogic = new ScheduleLogic(scheduleAccess);
            else ScheduleLogic = new ScheduleLogic();

            _experiences = _DataAccess.LoadAll();
        }

        public ExperienceModel GetById(int id)
        {
            _experiences = _DataAccess.LoadAll();
            return _experiences.Find(i => i.Id == id);
        }

        public bool ValidateExperience(ExperienceModel experience)
        {
            if (experience == null) return false;
            else if (!ValidateExperienceName(experience.Name)) return false;
            else if (!ValidateExperienceDescription(experience.Description)) return false;
            else if (!ValidateExperienceIntensity(experience.Intensity)) return false;
            else if (!ValidateExperienceTimeLength(experience.TimeLength)) return false;
            return true;
        }
        public bool ValidateExperienceName(string name) => (name == null || name == "") ? false : true;
        public bool ValidateExperienceDescription(string description) => !string.IsNullOrEmpty(description);
        public bool ValidateExperienceTimeLength(int timeLength) => timeLength < 0 ? false : true;
        public bool ValidateExperienceIntensity(Intensity intensity) => (!Enum.IsDefined(typeof(Intensity), intensity)) ? false : true;
        public bool ValidateExperienceTimeLength(string timeLength) => (int.TryParse(timeLength, out int _)) ? true : false;
        public bool ValidateMovieId(int filmId) => MoviesLogic.GetMovieById(filmId) == null ? false : true;
        public bool ValidateExperienceArchive(bool archived) => true;

        public bool AddExperience(ExperienceModel experience)
        {
            if (experience == null || !this.ValidateExperience(experience))
            {
                return false;
            }
            if (this.GetById(experience.Id) == null) experience.Id = IdGenerator.GetNextId(_experiences);
            _experiences.Add(experience);
            _DataAccess.WriteAll(_experiences);
            return true;
        }

        public List<ExperienceModel> GetExperiences()
        {
            _experiences = _DataAccess.LoadAll();
            return _experiences;
        }

        public List<ExperienceModel> GetExperiencesByUserPreferences(UserModel currentUser)
        {
            GetExperiences();

            List<ExperienceModel> experiences = new List<ExperienceModel>();

            foreach (ExperienceModel experience in _experiences)
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

        public bool EditExperience(int id, string name, string description, Intensity intensity, int timeLength, int filmId)
        {
            if (ValidateExperienceName(name) && ValidateExperienceDescription(description) && ValidateExperienceIntensity(intensity) && ValidateExperienceTimeLength(timeLength) && ValidateMovieId(filmId))
            {
                ExperienceModel experience = GetById(id);
                experience.Name = name;
                experience.Description = description;
                experience.Intensity = intensity;
                experience.FilmId = filmId;
                experience.TimeLength = timeLength;

                UpdateList(experience);
                return true;
            }

            return false;
        }

        public void UpdateList(ExperienceModel experience)
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
            _DataAccess.WriteAll(_experiences);
        }

        public void ArchiveExperience(int id)
        {
            ExperienceModel experience = GetById(id);
            if (experience != null)
            {
                experience.Archived = true;
                _DataAccess.WriteAll(_experiences);
            }
            else
            {
                return;
            }
        }

        public void UnarchiveExperience(int id)
        {
            ExperienceModel experience = GetById(id);
            if (experience != null)
            {
                experience.Archived = false;
                _DataAccess.WriteAll(_experiences);
            }
            else
            {
                return;
            }
        }

        public List<ExperienceModel> GetAllArchivedExperiences()
        {
            _experiences = _DataAccess.LoadAll();
            return _experiences.FindAll(e => e.Archived);
        }

        public List<ExperienceModel> GetAllActiveExperiences()
        {
            _experiences = _DataAccess.LoadAll();
            return _experiences.FindAll(e => !e.Archived);
        }
    }
}