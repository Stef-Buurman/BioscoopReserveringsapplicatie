namespace BioscoopReserveringsapplicatie
{
    public class ExperiencesLogic
    {
        private List<ExperiencesModel> _experiences;
        private List<MovieModel> _Movies;

        private static MoviesLogic MoviesLogic = new MoviesLogic();

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

                if (genreMatch && ageMatch && intensityMatch)
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
        public MovieModel GetMovieById(int id)
        {
            _Movies = MoviesAccess.LoadAll();
            return _Movies.Find(i => i.Id == id);
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
    }
}