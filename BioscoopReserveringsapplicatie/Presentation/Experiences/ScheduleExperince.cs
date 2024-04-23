namespace BioscoopReserveringsapplicatie
{
    public static class ScheduleExperince
    {
        private static ExperiencesLogic experiencesLogic = new ExperiencesLogic();
        private static LocationLogic locationLogic = new LocationLogic();
        private static RoomLogic roomLogic = new RoomLogic();
        private static ScheduleLogic scheduleLogic = new ScheduleLogic();

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
                    dateOptions.Add(new Option<string>(DateTime.Today.AddDays(i).ToString("dd-MM-yyyy")));
                }
                
                string experienceDate = SelectionMenuUtil.Create(dateOptions, () => { Header(); Console.WriteLine("Kies een datum om deze experience op in te plannen.\n"); }, () => ExperienceDetails.Start(experienceId));


                List<Option<string>> hourOptions = new List<Option<string>>();

                for (int i = 7; i <= 23; i++)
                {
                    if(i.ToString().Length == 1)
                    {
                        hourOptions.Add(new Option<string>($"0{i}"));
                    }
                    else
                    {
                        hourOptions.Add(new Option<string>($"{i}"));
                    }
                }

                string experienceHour = SelectionMenuUtil.Create(hourOptions, () => { Header(); Console.WriteLine("Kies een tijd om deze experience op in te plannen.\n"); }, () => ExperienceDetails.Start(experienceId));

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

                string scheduledDateTime = $"{experienceDate} {experienceTime}";

                List<Option<string>> options = new List<Option<string>>
                {
                    
                    new Option<string>("Ja", () => {
                        if(scheduleLogic.Add(experienceId, roomId, locationId, scheduledDateTime))
                        {
                            Console.Clear();
                            ColorConsole.WriteColorLine("Experience is ingepland!", Globals.SuccessColor);
                            Thread.Sleep(2000);
                            AdminMenu.Start();
                        }
                        else
                        {
                            List<Option<string>> options = new List<Option<string>>
                            {
                                new Option<string>("Terug", () => {Console.Clear(); ExperienceDetails.Start(experienceId);}),
                            };
                            SelectionMenuUtil.Create(options, () => ColorConsole.WriteColorLine("Er is een fout opgetreden tijdens het inplannen. Probeer het opnieuw.\n", Globals.ErrorColor));
                        }
                    }),
                    new Option<string>("Nee", () => ExperienceDetails.Start(experienceId))
                };
                SelectionMenuUtil.Create(options, () => PendingSchedule(experienceId, roomId, locationId, scheduledDateTime));
            }
            else ColorConsole.WriteColorLine("User is geen admin!", ConsoleColor.DarkRed);
        }

        private static void Header()
        {
            ColorConsole.WriteColorLine("[Experience inplannen]\n\n", Globals.TitleColor);
        }

        private static void PendingSchedule(int experienceId, int roomId, int locationId, string scheduledDateTime)
        {
            Console.Clear();
            ColorConsole.WriteColorLine("Gegevens ingeplande experience", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Experience: ]{experiencesLogic.GetById(experienceId).Name}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Locatie: ]{locationLogic.GetById(locationId).Name}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Zaal: ]{roomLogic.GetById(roomId)?.RoomNumber}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Datum en tijd: ]{scheduledDateTime}\n", ConsoleColor.Green);
            ColorConsole.WriteColorLine("Weet je zeker dat je de experience wilt inplannen ?", ConsoleColor.Red);
        }
    }
}