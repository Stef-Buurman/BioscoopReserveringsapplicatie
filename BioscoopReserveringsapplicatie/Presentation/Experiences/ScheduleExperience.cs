using System.Globalization;

namespace BioscoopReserveringsapplicatie
{
    public static class ScheduleExperience
    {
        private static ExperienceLogic experiencesLogic = new ExperienceLogic();
        private static LocationLogic locationLogic = new LocationLogic();
        private static RoomLogic roomLogic = new RoomLogic();
        private static ScheduleLogic scheduleLogic = new ScheduleLogic();

        private static int locationId = 0;
        private static int roomId = 0;
        private static string scheduleDate = "";
        private static string scheduleHour = "";
        private static string scheduleTime = "";

        private static string _returnToLocation = "Location";
        private static string _returnToRoom = "Room";
        private static string _returnToDate = "Date";
        private static string _returnToHour = "Hour";
        private static string _returnToTime = "Time";

        private static string _slotNotOpenError = "";
        private static List<ScheduleModel> schedules = new List<ScheduleModel>();

        public static void Start(int experienceId, string returnTo = "")
        {
            //locationId = 0;
            //roomId = 0;
            //scheduleDate = "";
            //scheduleHour = "";
            //scheduleTime = "";

            if(UserLogic.IsAdmin())
            {
                Console.Clear();

                if(returnTo == "" || returnTo == _returnToLocation)
                {
                    locationId = 0;
                    locationId = SelectLocation(experienceId);
                    if(returnTo != "") returnTo = "";
                }

                if(returnTo == "" || returnTo == _returnToRoom)
                {
                    roomId = 0;
                    roomId = SelectRoom(locationId, experienceId);
                    if(returnTo != "") returnTo = "";
                }

                if(returnTo == "" || returnTo == _returnToDate)
                {
                    scheduleDate = "";
                    scheduleDate = SelectDate(experienceId);
                    if(returnTo != "") returnTo = "";
                }

                if (returnTo == "" || returnTo == _returnToHour)
                {
                    scheduleHour = "";
                    scheduleHour = SelectHour(experienceId);
                    if (returnTo != "") returnTo = "";
                }

                if (returnTo == "" || returnTo == _returnToTime)
                {
                    scheduleTime = "";
                    scheduleTime = SelectMinute(scheduleHour, experienceId);
                    if (returnTo != "") returnTo = "";
                }

                string scheduledDateTime = $"{scheduleDate} {scheduleTime}";

                if(!scheduleLogic.TimeSlotOpenOnRoom(experienceId, locationId, roomId, scheduledDateTime, out string error))
                {
                    DateTime.TryParseExact(scheduledDateTime, "dd-MM-yyyy HH:mm", CultureInfo.GetCultureInfo("nl-NL"), DateTimeStyles.None, out DateTime dateTime);
                    string formattedTime = dateTime.AddMinutes(experiencesLogic.GetById(experienceId).TimeLength).ToString("HH:mm");
                    _slotNotOpenError = error;
                    Start(experienceId, _returnToHour);
                }

                List<Option<string>> options = new List<Option<string>>
                {
                    
                    new Option<string>("Ja", () => {
                        if(scheduleLogic.Add(scheduleLogic.CreateSchedule(experienceId, roomId, locationId, scheduledDateTime)))
                        {
                            Console.Clear();
                            ColorConsole.WriteColorLine("Experience is ingepland!", Globals.SuccessColor);
                            _slotNotOpenError = "";
                            WaitUtil.WaitTime(2000);
                            AdminMenu.Start();
                        }
                        else
                        {
                            List<Option<string>> options = new List<Option<string>>
                            {
                                new Option<string>("Terug", () => {Console.Clear(); ExperienceDetails.Start(experienceId);}),
                            };

                            Console.Clear();
                            ColorConsole.WriteColorLine("Er is een fout opgetreden tijdens het inplannen. Probeer het opnieuw.\n", Globals.ErrorColor);
                            _slotNotOpenError = "";
                            new SelectionMenuUtil<string>(options).Create();
                        }
                    }),
                    new Option<string>("Nee", () => ExperienceDetails.Start(experienceId))
                };

                Console.Clear();
                PendingSchedule(experienceId, roomId, locationId, scheduledDateTime);
                _slotNotOpenError = "";
                new SelectionMenuUtil<string>(options, new Option<string>("Nee")).Create();
            }
            else ColorConsole.WriteColorLine("User is geen admin!", ConsoleColor.DarkRed);
        }

        private static int SelectLocation(int experienceId)
        {
            Console.Clear();
            Header(); 
            CurrentSchedule(experienceId);
            Console.WriteLine("Kies de locatie om deze experience op in te plannen.\n"); 

            List<Option<int>> locationOptions = new List<Option<int>>();

            foreach (LocationModel location in locationLogic.GetAll())
            {
                locationOptions.Add(new Option<int>(location.Id, location.Name));
            }

            int locationId = new SelectionMenuUtil<int>(locationOptions,() => ExperienceDetails.Start(experienceId), () => Start(experienceId, _returnToLocation)).Create();
            return locationId;
        }

        private static int SelectRoom(int locationId, int experienceId)
        {
            Console.Clear();
            Header();
            CurrentSchedule(experienceId);
            Console.WriteLine("Kies de zaal om deze experience op in te plannen.\n");

            List<Option<int>> roomOptions = new List<Option<int>>();

            foreach (RoomModel room in roomLogic.GetByLocationId(locationId))
            {
                roomOptions.Add(new Option<int>(room.Id, $"Zaal: {room.RoomNumber}"));
            }

            int roomId = new SelectionMenuUtil<int>(roomOptions,() => Start(experienceId, _returnToLocation), () => Start(experienceId, _returnToRoom)).Create();
            return roomId;
        }

        private static string SelectDate(int experienceId)
        {
            Console.Clear();
            Header();
            CurrentSchedule(experienceId);
            Console.WriteLine("Kies een datum om deze experience op in te plannen.\n");

            List<Option<string>> dateOptions = new List<Option<string>>();

            for (int i = 1; i < 15; i++)
            {
                dateOptions.Add(new Option<string>(DateTime.Today.AddDays(i).ToString("dd-MM-yyyy")));
            }
                
            string scheduleDate = new SelectionMenuUtil<string>(dateOptions,() => Start(experienceId, _returnToRoom), () => Start(experienceId, _returnToDate)).Create();
            return scheduleDate;
        }

        private static string SelectHour(int experienceId)
        {
            Console.Clear();
            Header();
            CurrentSchedule(experienceId);

            schedules = scheduleLogic.GetScheduledExperiencesByDateAndRoomId(roomId, DateTime.ParseExact(scheduleDate, "dd-MM-yyyy", CultureInfo.GetCultureInfo("nl-NL")).Date);
            schedules = schedules.OrderBy(schedule => schedule.ScheduledDateTimeStart).ToList();
            
            Console.WriteLine("\nIngeplande experiences op deze datum:\n");
            foreach(var schedule in schedules)
            {
                string experienceName = experiencesLogic.GetById(schedule.ExperienceId).Name;
                string startTime = schedule.ScheduledDateTimeStart.ToString("HH:mm");
                string endTime = schedule.ScheduledDateTimeEnd.ToString("HH:mm");

                ColorConsole.WriteColorLine($"{experienceName} : {startTime} - {endTime}", ConsoleColor.Green);
            }

            Console.WriteLine("\nKies een tijd om deze experience op in te plannen.\n");

            if (_slotNotOpenError != "")
            {
                ColorConsole.WriteColorLine($"\n {_slotNotOpenError}", Globals.ErrorColor);
            }

            List<Option<string>> hourOptions = new List<Option<string>>();

            for (int i = 23; i >= 7; i--)
            {
                if (i.ToString().Length == 1)
                {
                    hourOptions.Add(new Option<string>($"0{i}:00"));
                }
                else
                {
                    hourOptions.Add(new Option<string>($"{i}:00"));
                }
            }

            string scheduleHour = new SelectionMenuUtil<string>(hourOptions, 1, () => Start(experienceId, _returnToDate), () => Start(experienceId, _returnToHour), new Option<string>("07:00"), false).Create();
            string[] splitscheduleHour = scheduleHour.Split(":");
            return splitscheduleHour[0];
        }

        private static string SelectMinute(string scheduleHour, int experienceId) 
        {
            Console.Clear();
            Header();
            CurrentSchedule(experienceId);
            
            Console.WriteLine("\nIngeplande experiences op deze datum:\n");
            foreach(var schedule in schedules)
            {
                string experienceName = experiencesLogic.GetById(schedule.ExperienceId).Name;
                string startTime = schedule.ScheduledDateTimeStart.ToString("HH:mm");
                string endTime = schedule.ScheduledDateTimeEnd.ToString("HH:mm");

                ColorConsole.WriteColorLine($"{experienceName} : {startTime} - {endTime}", ConsoleColor.Green);
            }

            Console.WriteLine("\nKies een tijd om deze experience op in te plannen.\n");

            if (_slotNotOpenError != "")
            {
                ColorConsole.WriteColorLine($"\n {_slotNotOpenError}", Globals.ErrorColor);
            }

            List<Option<string>> timeOptions = new List<Option<string>>();

            for (int i = 55; i >= 0; i = i - 5)
            {
                if (i.ToString().Length == 1)
                {
                    timeOptions.Add(new Option<string>($"0{i}"));
                }
                else
                {
                    timeOptions.Add(new Option<string>($"{i}"));
                }
            }
            
            string experienceMinutes = new SelectionMenuUtil<string>(timeOptions, 1, () => Start(experienceId, _returnToHour), () => Start(experienceId, _returnToTime), new Option<string>("00"), false, $"{scheduleHour}:").Create();
            scheduleTime = $"{scheduleHour}:{experienceMinutes}";
            return scheduleTime;
        }

         private static void Header()
        {
            ColorConsole.WriteColorLine("[Experience inplannen]\n\n", Globals.TitleColor);
        }
        
        private static void CurrentSchedule(int experienceId)
        {
            ColorConsole.WriteColorLine("Huidige inplanning experience", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Experience: ]{experiencesLogic.GetById(experienceId).Name}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Experience duur: ]{experiencesLogic.GetById(experienceId).TimeLength} Minuten", ConsoleColor.Green);

            if(locationId != 0)
            {
                ColorConsole.WriteColorLine($"[Locatie: ]{locationLogic.GetById(locationId).Name}", ConsoleColor.Green);
            }
            if(roomId != 0)
            {
                ColorConsole.WriteColorLine($"[Zaal: ]{roomLogic.GetById(roomId).RoomNumber}", ConsoleColor.Green);
            }
            if(scheduleDate != "")
            {
                ColorConsole.WriteColorLine($"[Datum: ]{scheduleDate}\n", ConsoleColor.Green);
            }
        }

        private static void PendingSchedule(int experienceId, int roomId, int locationId , string scheduledDateTime)
        {
            DateTime.TryParseExact(scheduledDateTime, "dd-MM-yyyy HH:mm", CultureInfo.GetCultureInfo("nl-NL"), DateTimeStyles.None, out DateTime dateTime);
            string formattedTime = dateTime.AddMinutes(experiencesLogic.GetById(experienceId).TimeLength).ToString("HH:mm");
            
            Console.Clear();
            ColorConsole.WriteColorLine("Gegevens ingeplande experience", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Experience: ]{experiencesLogic.GetById(experienceId).Name}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Locatie: ]{locationLogic.GetById(locationId).Name}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Zaal: ]{roomLogic.GetById(roomId).RoomNumber}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Datum: ]{scheduleDate}", ConsoleColor.Green);
            ColorConsole.WriteColorLine($"[Tijd: ]{scheduleTime} T/M {formattedTime}\n", ConsoleColor.Green);
            ColorConsole.WriteColorLine("Weet je zeker dat je de experience wilt inplannen ?", ConsoleColor.Red);
        }
    }
}