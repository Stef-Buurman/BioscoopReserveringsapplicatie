namespace BioscoopReserveringsapplicatie
{
    public class ExperienceLogic : ILogic<ExperienceModel>
    {
        private List<ExperienceModel> _experiences;
        public IDataAccess<ExperienceModel> _DataAccess { get; }
        private static MovieLogic MoviesLogic;
        private static ScheduleLogic ScheduleLogic;

        public ExperienceLogic(IDataAccess<ExperienceModel> experienceAccess = null,
            IDataAccess<MovieModel> movieAccess = null,
            IDataAccess<ScheduleModel> scheduleAccess = null)
        {
            if (experienceAccess != null) _DataAccess = experienceAccess;
            else _DataAccess = new DataAccess<ExperienceModel>();

            if (movieAccess != null) MoviesLogic = new MovieLogic(movieAccess);
            else MoviesLogic = new MovieLogic();

            if (scheduleAccess != null) ScheduleLogic = new ScheduleLogic(scheduleAccess);
            else ScheduleLogic = new ScheduleLogic();

            _experiences = _DataAccess.LoadAll();
        }

        public int GetNextId() => IdGenerator.GetNextId(_experiences);

        public ExperienceModel GetById(int id)
        {
            _experiences = _DataAccess.LoadAll();
            return _experiences.Find(i => i.Id == id);
        }

        public bool Validate(ExperienceModel experience)
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
        public bool ValidateMovieId(int filmId) => MoviesLogic.GetById(filmId) == null ? false : true;

        public bool Add(ExperienceModel experience)
        {
            if (!Validate(experience))
            {
                return false;
            }
            if (this.GetById(experience.Id) == null) experience.Id = IdGenerator.GetNextId(_experiences);
            _experiences.Add(experience);
            _DataAccess.WriteAll(_experiences);
            return true;
        }

        public List<ExperienceModel> GetAll()
        {
            _experiences = _DataAccess.LoadAll();
            return _experiences;
        }

        public List<ExperienceModel> GetExperiencesByUserPreferences(UserModel currentUser, DateTime date)
        {
            GetAll();

            List<ExperienceModel> experiences = new List<ExperienceModel>();

            foreach (ExperienceModel experience in _experiences)
            {
                if (experience.Status == Status.Archived) continue;

                MovieModel movie = MoviesLogic.GetById(experience.FilmId);

                if (movie == null) continue;

                bool genreMatch = currentUser.Genres.Count == 0 || movie.Genres.Intersect(currentUser.Genres).Any();
                bool ageMatch = currentUser.AgeCategory == AgeCategory.Undefined || currentUser.AgeCategory == AgeCategory.All || movie.AgeCategory == currentUser.AgeCategory;
                bool intensityMatch = currentUser.Intensity == Intensity.Undefined || currentUser.Intensity == Intensity.All || experience.Intensity == currentUser.Intensity;
                bool hasScheduldedExperience = ScheduleLogic.HasScheduledExperience(experience.Id, date);

                if (genreMatch && ageMatch && intensityMatch && hasScheduldedExperience)
                {
                    experiences.Add(experience);
                }
            }
            return experiences;
        }

        public bool Edit(ExperienceModel experience)
        {
            if (!Validate(experience) || this.GetById(experience.Id) == null)
            {
                return false;
            }
            UpdateList(experience);
            return true;
        }

        public bool Edit(int id, string name, string description, Intensity intensity, int timeLength, int filmId)
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

        public void Archive(int id)
        {
            ExperienceModel experience = GetById(id);
            if (experience != null)
            {
                experience.Status = Status.Archived;
                _DataAccess.WriteAll(_experiences);
            }
            else
            {
                return;
            }
        }

        public void Unarchive(int id)
        {
            ExperienceModel experience = GetById(id);
            if (experience != null)
            {
                experience.Status = Status.Active;
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
            return _experiences.FindAll(e => e.Status == Status.Archived);
        }

        public List<ExperienceModel> GetAllActiveExperiences()
        {
            _experiences = _DataAccess.LoadAll();
            return _experiences.FindAll(e => e.Status == Status.Active);
        }
    }
}