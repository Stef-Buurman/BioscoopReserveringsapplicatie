namespace BioscoopReserveringsapplicatie
{
    public class ExperiencesLogic
    {
        private List<ExperiencesModel> _experiences;

        public ExperiencesLogic()
        {
            _experiences = ExperiencesAccess.LoadAll();
        }

        public ExperiencesModel GetById(int id)
        {
            return _experiences.Find(i => i.Id == id);
        }

        public bool ValidateExperience(ExperiencesModel experience)
        {
            if (experience == null) return false;
            else if (!ValidateExperienceName(experience.Name)) return false;
            else if (experience.Intensity < 0 || experience.Intensity > 10) return false;
            else if (!ValidateExperienceTimeLength(experience.TimeLength)) return false;
            return true;
        }

        public bool ValidateExperienceName(string name) => (name == null || name == "") ? false : true;
        public bool ValidateExperienceTimeLength(int timeLength) => timeLength < 0 ? false : true;
        public bool ValidateExperienceTimeLength(string timeLength) => (int.TryParse(timeLength, out int _)) ? true : false;

        public bool AddExperience(ExperiencesModel experience)
        {
            if (experience == null || !this.ValidateExperience(experience))
            {
                return false;
            }
            if (this.GetById(experience.Id) == null) experience.Id = GetNextId.GetNextIdForExperience(_experiences);
            _experiences.Add(experience);
            ExperiencesAccess.WriteAll(_experiences);
            return true;
        }
    }
}
