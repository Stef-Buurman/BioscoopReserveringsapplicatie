namespace BioscoopReserveringsapplicatie
{
    static class ExperienceReservation
    {
        private static ExperiencesLogic ExperienceLogic = new ExperiencesLogic();
        private static LocationLogic LocationLogic = new LocationLogic();
        private static ReservationLogic ReservationLogic = new ReservationLogic();
        private static ScheduleLogic ScheduleLogic = new ScheduleLogic();

        public static void Start(int experienceId, int location = 0, DateTime? dateTime = null, int room = 0)
        {
            Console.Clear();

            if (experienceId != 0)
            {
                ColorConsole.WriteColorLine("Experience reserveren\n", Globals.ExperienceColor);

                Console.WriteLine("Naam experience: " + ExperienceLogic.GetById(experienceId).Name);

                if (location == 0)
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

                if (dateTime == null)
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
                    Console.WriteLine("Datum: " + dateTime.Value.ToString("dd-MM-yyyy"));
                }

                if (dateTime.Value.TimeOfDay == TimeSpan.Zero)
                {
                    List<ScheduleModel> schedules = ScheduleLogic.GetScheduledExperienceTimeSlotsForLocationById(experienceId, location, dateTime.Value.Date);

                    schedules.Sort((x, y) => x.ScheduledDateTime.CompareTo(y.ScheduledDateTime));

                    ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [tijden]:", Globals.ColorInputcClarification);

                    var options = new List<Option<int>>();

                    foreach (ScheduleModel schedule in schedules)
                    {
                        if (schedule.ScheduledDateTime.Date == dateTime.Value.Date)
                        {
                            options.Add(new Option<int>(schedule.Id, schedule.ScheduledDateTime.ToString("HH:mm"), () => ExperienceReservation.Start(experienceId, location, schedule.ScheduledDateTime)));
                        }
                    }
                    options.Add(new Option<int>(0, "Terug", () => ExperienceReservation.Start(experienceId, location)));

                    new SelectionMenuUtil2<int>(options).Create();
                }
                else
                {
                    Console.WriteLine("Tijd: " + dateTime.Value.ToString("HH:mm"));
                }

                if (room == 0)
                {
                    ScheduleModel scheduledExperience = ScheduleLogic.GetRoomForScheduledExperience(experienceId, location, dateTime);

                    room = scheduledExperience.RoomId;

                    Console.WriteLine("Zaal: " + room);
                }

                var optionsConfirm = new List<Option<int>>
                {
                    new Option<int>(0, "Bevestig reservering", () =>
                        {
                            int scheduleId = ScheduleLogic.GetRelatedScheduledExperience(experienceId, location, dateTime, room);

                            if (UserLogic.CurrentUser != null)
                            {
                                if (!ReservationLogic.HasUserAlreadyReservedScheduledExperience(scheduleId, UserLogic.CurrentUser.Id))
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
                    new Option<int>(0, "Terug", () => ExperienceReservation.Start(experienceId, location, dateTime.Value.Date))
                };

                Console.WriteLine("\nReservering bevestigen");
                Console.WriteLine("Is de reservering correct?\n");

                new SelectionMenuUtil2<int>(optionsConfirm).Create();
            }
        }
    }
}