namespace BioscoopReserveringsapplicatie
{
    public static class ScheduleExperince
    {
        private static ExperiencesLogic experiencesLogic = new ExperiencesLogic();
        private static LocationLogic locationLogic = new LocationLogic();
        private static RoomLogic roomLogic = new RoomLogic();
        public static void Start(int experienceId)
        {
            if(UserLogic.CurrentUser != null && UserLogic.CurrentUser.IsAdmin)
            {
                Console.Clear();

                List<Option<int>> locationOptions = new List<Option<int>>();

                foreach (LocationModel location in locationLogic.GetAll())
                {
                    locationOptions.Add(new Option<int>(location.Id, location.Name));
                }

                int locationId = SelectionMenuUtil.Create(locationOptions, () => { Header(); Console.WriteLine("Kies de location om deze experience op in te plannen.\n"); }, () => ExperienceDetails.Start(experienceId));

                List<Option<int>> roomOptions = new List<Option<int>>();

                foreach (RoomModel room in roomLogic.GetByLocationId(locationId))
                {
                    roomOptions.Add(new Option<int>(room.Id, $"Zaal: {room.RoomNumber}"));
                }

                int roomId = SelectionMenuUtil.Create(roomOptions, () => { Header(); Console.WriteLine("Kies de zaal om deze experience op in te plannen.\n"); }, () => ExperienceDetails.Start(experienceId));

                List<Option<string>> dateOptions = new List<Option<string>>();

                for (int i = 1; i < 15; i++)
                {
                    dateOptions.Add(new Option<string>(DateTime.Today.AddDays(i).ToString("dd-M-yyyy")));
                }
                
                string experienceDate = SelectionMenuUtil.Create(dateOptions, () => { Header(); Console.WriteLine("Kies een datum om deze experience op in te plannen.\n"); }, () => ExperienceDetails.Start(experienceId));


                List<Option<int>> hourOptions = new List<Option<int>>();

                for (int i = 7; i <= 23; i++)
                {
                    hourOptions.Add(new Option<int>(i));
                }

                int experienceHour = SelectionMenuUtil.Create(hourOptions, () => { Header(); Console.WriteLine("Kies een tijd om deze experience op in te plannen.\n"); }, () => ExperienceDetails.Start(experienceId));

                List<Option<string>> timeOptions = new List<Option<string>>();

                for (int i = 0; i <= 55; i = i + 5)
                {
                    if(i.ToString().Length == 1)
                    {
                        timeOptions.Add(new Option<string>($"{experienceHour}:0{i}"));
                    }
                    else
                    {
                        timeOptions.Add(new Option<string>($"{experienceHour}:{i}"));
                    }
                }

                string experienceTime = SelectionMenuUtil.Create(timeOptions, () => { Header(); Console.WriteLine("Kies een tijd om deze experience op in te plannen.\n"); }, () => ExperienceDetails.Start(experienceId));
            }
        }

        public static void Header()
        {
            ColorConsole.WriteColorLine("[Experience inplannen]\n\n", Globals.TitleColor);
        }
    }
}