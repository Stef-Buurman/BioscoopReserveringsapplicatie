namespace BioscoopReserveringsapplicatie
{
    public static class GetNextId
    {
        public static int GetNextIdForExperience(List<ExperiencesModel> experiences)
        {
            int maxId = 0;
            foreach (ExperiencesModel experience in experiences)
            {
                if (experience.Id > maxId) maxId = experience.Id;
            }
            return maxId + 1;
        }
    }
}