using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

class ExperiencesLogic
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
        else if (experience.Name == null) return false;
        else if (experience.Intensity < 0 || experience.Intensity > 10) return false;
        else if (experience.TimeLength < 0) return false;
        return true;
    }

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