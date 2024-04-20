namespace BioscoopReserveringsapplicatie
{
    static class ExperienceReservation
    {
        private static ExperiencesLogic ExperienceLogic = new ExperiencesLogic();

        public static void Start(int experienceId, int scheduldedExperienceId = 0)
        {
            Console.Clear();

            if (experienceId != 0 && scheduldedExperienceId == 0)
            {
                List<ScheduleModel> schedules = ExperienceLogic.GetScheduledExperiencesById(experienceId);

                Print();

                var options = new List<Option<int>>();

                foreach (ScheduleModel schedule in schedules)
                {
                    options.Add(new Option<int>(schedule.Id, schedule.ScheduledDateTime.ToString() + " - " + ExperienceLogic.GetEndTimeForScheduledExperience(schedule.Id), () => ExperienceReservation.Start(experienceId, schedule.Id)));
                }
                options.Add(new Option<int>(0, "Terug", () => ExperienceDetails.Start(experienceId)));

                new SelectionMenuUtil2<int>(options).Create();
            }
            else
            {
                Console.WriteLine("Je hebt een keuze gemaakt");
            }
        }

        private static void Print()
        {
            Console.WriteLine("Experience reserveren");
            Console.WriteLine("Maak een keuze uit de onderstaande mogelijkheden:");
        }
    }
}