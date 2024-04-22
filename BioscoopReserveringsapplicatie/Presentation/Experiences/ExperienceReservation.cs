namespace BioscoopReserveringsapplicatie
{
    static class ExperienceReservation
    {
        private static ExperiencesLogic ExperienceLogic = new ExperiencesLogic();
        private static LocationLogic LocationLogic = new LocationLogic();
        private static ReservationLogic ReservationLogic = new ReservationLogic();
        private static ScheduleLogic ScheduleLogic = new ScheduleLogic();

        public static void Start(int experienceId, int? location = null, DateTime? date = null, TimeSpan? time = null, int? room = null)
        {
            Console.Clear();

            if (experienceId != 0)
            {
                if (location == null)
                {
                    List<LocationModel> locations = LocationLogic.GetLocationsForScheduledExperienceById(experienceId);

                    Print();

                    Console.WriteLine("Maak een keuze uit een van de onderstaande locaties:");

                    var options = new List<Option<int>>();

                    foreach (LocationModel locationSelected in locations)
                    {
                        options.Add(new Option<int>(locationSelected.Id, locationSelected.Name, () => ExperienceReservation.Start(experienceId, locationSelected.Id)));
                    }
                    options.Add(new Option<int>(0, "Terug", () => ExperienceDetails.Start(experienceId)));

                    new SelectionMenuUtil2<int>(options).Create();
                }

                if (date == null)
                {
                    List<ScheduleModel> schedules = ScheduleLogic.GetScheduledExperienceDatesForLocationById(experienceId, location);

                    schedules.Sort((x, y) => x.ScheduledDateTime.CompareTo(y.ScheduledDateTime));

                    schedules = schedules.DistinctBy(s => s.ScheduledDateTime.Date).ToList();

                    Print();

                    Console.WriteLine("Maak een keuze uit een van de onderstaande datums:");

                    var options = new List<Option<int>>();

                    foreach (ScheduleModel schedule in schedules)
                    {
                        options.Add(new Option<int>(schedule.Id, schedule.ScheduledDateTime.Date.ToString("dd-MM-yyyy"), () => ExperienceReservation.Start(experienceId, location, schedule.ScheduledDateTime.Date)));
                    }
                    options.Add(new Option<int>(0, "Terug", () => ExperienceReservation.Start(experienceId)));

                    new SelectionMenuUtil2<int>(options).Create();
                }

                if (time == null)
                {
                    List<ScheduleModel> schedules = ScheduleLogic.GetScheduledExperienceTimeSlotsForLocationById(experienceId, location, date);

                    schedules.Sort((x, y) => x.ScheduledDateTime.CompareTo(y.ScheduledDateTime));

                    Print();

                    Console.WriteLine("Maak een keuze uit een van de onderstaande tijden:");

                    var options = new List<Option<int>>();

                    foreach (ScheduleModel schedule in schedules)
                    {
                        if (schedule.ScheduledDateTime.Date == date)
                        {
                            options.Add(new Option<int>(schedule.Id, schedule.ScheduledDateTime.ToString("HH:mm"), () => ExperienceReservation.Start(experienceId, location, date, schedule.ScheduledDateTime.TimeOfDay)));
                        }
                    }
                    options.Add(new Option<int>(0, "Terug", () => ExperienceReservation.Start(experienceId, location)));

                    new SelectionMenuUtil2<int>(options).Create();
                }

                if (room == null)
                {
                    Print();

                    ScheduleModel scheduledExperience = ScheduleLogic.GetRoomForScheduledExperience(experienceId, location, date, time);

                    room = scheduledExperience.RoomId;
                }

                ReservationOverview(experienceId, location, date, time, room);

                var options2 = new List<Option<int>>();

                options2.Add(new Option<int>(0, "Bevestig reservering", () =>
                {
                    DateTime dateTime = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, time.Value.Hours, time.Value.Minutes, time.Value.Seconds);

                    int scheduleId = ScheduleLogic.GetRelatedScheduledExperience(experienceId, location, dateTime, room);

                    if (UserLogic.CurrentUser != null)
                    {
                        if (!ReservationLogic.HasUserAlreadyReservatedScheduledExperience(scheduleId, UserLogic.CurrentUser.Id))
                        {
                            if (ReservationLogic.Complete(scheduleId, UserLogic.CurrentUser.Id))
                            {
                                Console.WriteLine("Reservering bevestigd");

                                Console.WriteLine("Druk op een toets om terug te gaan naar de experience details");

                                Console.ReadKey();

                                ExperienceDetails.Start(experienceId);
                            }
                            else
                            {
                                Console.WriteLine("Reservering mislukt");

                                Console.WriteLine("Druk op een toets om terug te gaan naar de experience details om het opnieuw te proberen.");

                                Console.ReadKey();

                                ExperienceDetails.Start(experienceId);

                            }
                        }
                        else
                        {
                            Console.WriteLine("Je hebt al voor deze experience gereserveerd");

                            Console.WriteLine("Druk op een toets om terug te gaan naar de experience details");

                            Console.ReadKey();

                            ExperienceDetails.Start(experienceId);
                        }

                    }
                }));
                options2.Add(new Option<int>(0, "Terug", () => ExperienceReservation.Start(experienceId, location, date)));

                new SelectionMenuUtil2<int>(options2).Create();
            }
        }

        private static void ReservationOverview(int experienceId, int? location, DateTime? date, TimeSpan? time, int? room)
        {
            Console.WriteLine("Reserveringsoverzicht");

            Console.WriteLine("Experience: " + ExperienceLogic.GetById(experienceId).Name);
            Console.WriteLine("Locatie: " + LocationLogic.GetById((int)location).Name);
            Console.WriteLine("Datum: " + date.Value.ToString("dd-MM-yyyy"));
            Console.WriteLine("Tijd: " + time.Value.ToString("HH:mm"));
            Console.WriteLine("Zaal: " + room.Value);

            Console.WriteLine();
        }

        private static void Print()
        {
            Console.WriteLine("Experience reserveren");
        }
    }
}