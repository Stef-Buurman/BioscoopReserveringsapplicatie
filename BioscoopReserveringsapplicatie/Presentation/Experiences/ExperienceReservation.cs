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
                ColorConsole.WriteColorLine("Experience reserveren\n", Globals.ExperienceColor);

                Console.WriteLine("Naam experience: " + ExperienceLogic.GetById(experienceId).Name);

                if (location == null)
                {
                    List<LocationModel> locations = LocationLogic.GetLocationsForScheduledExperienceById(experienceId);

                    ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [locaties]:", Globals.ColorInputcClarification);

                    var options = new List<Option<int>>();

                    foreach (LocationModel locationSelected in locations)
                    {
                        options.Add(new Option<int>(locationSelected.Id, locationSelected.Name, () => ExperienceReservation.Start(experienceId, locationSelected.Id)));
                    }
                    options.Add(new Option<int>(0, "Terug", () => ExperienceDetails.Start(experienceId)));

                    new SelectionMenuUtil2<int>(options).Create();
                }
                else
                {
                    Console.WriteLine("Locatie: " + LocationLogic.GetById((int)location).Name);
                }

                if (date == null)
                {
                    List<ScheduleModel> schedules = ScheduleLogic.GetScheduledExperienceDatesForLocationById(experienceId, location);

                    schedules.Sort((x, y) => x.ScheduledDateTime.CompareTo(y.ScheduledDateTime));

                    schedules = schedules.DistinctBy(s => s.ScheduledDateTime.Date).ToList();

                    ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [datums]:", Globals.ColorInputcClarification);

                    var options = new List<Option<int>>();

                    foreach (ScheduleModel schedule in schedules)
                    {
                        options.Add(new Option<int>(schedule.Id, schedule.ScheduledDateTime.Date.ToString("dd-MM-yyyy"), () => ExperienceReservation.Start(experienceId, location, schedule.ScheduledDateTime.Date)));
                    }
                    options.Add(new Option<int>(0, "Terug", () => ExperienceReservation.Start(experienceId)));

                    new SelectionMenuUtil2<int>(options).Create();
                }
                else
                {
                    Console.WriteLine("Datum: " + date.Value.ToString("dd-MM-yyyy"));
                }

                if (time == null)
                {
                    List<ScheduleModel> schedules = ScheduleLogic.GetScheduledExperienceTimeSlotsForLocationById(experienceId, location, date);

                    schedules.Sort((x, y) => x.ScheduledDateTime.CompareTo(y.ScheduledDateTime));

                    ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [tijden]:", Globals.ColorInputcClarification);

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
                else
                {
                    Console.WriteLine("Tijd: " + TimeSpan.Parse(time.ToString()).ToString(@"hh\:mm") + " - " + TimeSpan.FromMinutes(time.Value.TotalMinutes + ExperienceLogic.GetById(experienceId).TimeLength).ToString(@"hh\:mm"));
                }

                if (room == null)
                {
                    ScheduleModel scheduledExperience = ScheduleLogic.GetRoomForScheduledExperience(experienceId, location, date, time);

                    room = scheduledExperience.RoomId;

                    Console.WriteLine("Zaal: " + room.Value);
                }

                var options2 = new List<Option<int>>
                {
                    new Option<int>(0, "Bevestig reservering", () =>
                        {
                            DateTime dateTime = new DateTime(date.Value.Year, date.Value.Month, date.Value.Day, time.Value.Hours, time.Value.Minutes, time.Value.Seconds);

                            int scheduleId = ScheduleLogic.GetRelatedScheduledExperience(experienceId, location, dateTime, room);

                            if (UserLogic.CurrentUser != null)
                            {
                                if (!ReservationLogic.HasUserAlreadyReservatedScheduledExperience(scheduleId, UserLogic.CurrentUser.Id))
                                {
                                    if (ReservationLogic.Complete(scheduleId, UserLogic.CurrentUser.Id))
                                    {
                                        Console.WriteLine("\nReservering bevestigd");

                                        Console.WriteLine("Druk op een toets om terug te gaan naar de experience details");

                                        Console.ReadKey();

                                        ExperienceDetails.Start(experienceId);
                                    }
                                    else
                                    {
                                        Console.WriteLine("\nReservering mislukt");

                                        Console.WriteLine("Druk op een toets om terug te gaan naar de experience details om het opnieuw te proberen.");

                                        Console.ReadKey();

                                        ExperienceDetails.Start(experienceId);

                                    }
                                }
                                else
                                {
                                    Console.WriteLine("\nReservering mislukt");

                                    Console.WriteLine("Je hebt voor deze experience al gereserveerd");

                                    Console.WriteLine("Druk op een toets om terug te gaan naar de experience details");

                                    Console.ReadKey();

                                    ExperienceDetails.Start(experienceId);
                                }

                            }
                        }),
                    new Option<int>(0, "Terug", () => ExperienceReservation.Start(experienceId, location, date))
                };

                Console.WriteLine("\nReservering bevestigen");
                Console.WriteLine("Is de reservering correct?\n");

                new SelectionMenuUtil2<int>(options2).Create();
            }
        }
    }
}