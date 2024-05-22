namespace BioscoopReserveringsapplicatie
{
    static class ExperienceReservation
    {
        private static ExperienceLogic ExperienceLogic = new ExperienceLogic();
        private static LocationLogic LocationLogic = new LocationLogic();
        private static ReservationLogic ReservationLogic = new ReservationLogic();
        private static ScheduleLogic ScheduleLogic = new ScheduleLogic();
        private static RoomLogic roomLogic = new RoomLogic();

        public static void Start(int experienceId, int location = 0, DateTime? dateTime = null, int room = 0)
        {
            Console.Clear();

            if (experienceId != 0)
            {
                ColorConsole.WriteColorLine("Experience reserveren\n", Globals.ExperienceColor);

                ColorConsole.WriteColorLine("[Naam experience:] " + ExperienceLogic.GetById(experienceId).Name, Globals.ExperienceColor);

                if (location == 0)
                {
                    List<LocationModel> locations = LocationLogic.GetLocationsForScheduledExperienceById(experienceId);

                    ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [locaties]:", Globals.ColorInputcClarification);

                    var options = new List<Option<int>>();

                    foreach (LocationModel locationSelected in locations)
                    {
                        if (ReservationLogic.HasUserReservedAvailableOptionsForLocation(experienceId, locationSelected.Id, UserLogic.CurrentUser.Id) == false)
                        {
                            options.Add(new Option<int>(locationSelected.Id, locationSelected.Name, () => ExperienceReservation.Start(experienceId, locationSelected.Id)));
                        }
                    }
                    options.Add(new Option<int>(0, "Terug", () => ExperienceDetails.Start(experienceId)));

                    new SelectionMenuUtil<int>(options).Create();
                }
                else
                {
                    ColorConsole.WriteColorLine("[Locatie:] " + LocationLogic.GetById((int)location).Name, Globals.ExperienceColor);
                }

                if (dateTime == null)
                {
                    List<ScheduleModel> schedules = ScheduleLogic.GetScheduledExperienceDatesForLocationById(experienceId, location);

                    schedules.Sort((x, y) => x.ScheduledDateTimeStart.CompareTo(y.ScheduledDateTimeStart));

                    schedules = schedules.DistinctBy(s => s.ScheduledDateTimeStart.Date).ToList();

                    ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [datums]:", Globals.ColorInputcClarification);

                    var options = new List<Option<int>>();

                    foreach (ScheduleModel schedule in schedules)
                    {
                        if (ScheduleLogic.GetScheduledExperienceTimeSlotsForLocationById(experienceId, location, schedule.ScheduledDateTimeStart.Date).Count > 0)
                        {
                            options.Add(new Option<int>(schedule.Id, schedule.ScheduledDateTimeStart.Date.ToString("dd-MM-yyyy"), () => ExperienceReservation.Start(experienceId, location, schedule.ScheduledDateTimeStart.Date)));
                        }
                    }
                    options.Add(new Option<int>(0, "Terug", () => ExperienceReservation.Start(experienceId)));

                    new SelectionMenuUtil<int>(options).Create();
                }
                else
                {
                    ColorConsole.WriteColorLine("[Datum:] " + dateTime.Value.ToString("dd-MM-yyyy"), Globals.ExperienceColor);
                }

                if (dateTime.Value.TimeOfDay == TimeSpan.Zero)
                {
                    List<ScheduleModel> schedules = ScheduleLogic.GetScheduledExperienceTimeSlotsForLocationById(experienceId, location, dateTime.Value.Date);

                    schedules.Sort((x, y) => x.ScheduledDateTimeStart.CompareTo(y.ScheduledDateTimeStart));

                    schedules = schedules.DistinctBy(s => s.ScheduledDateTimeStart.TimeOfDay).ToList();

                    ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [tijden]:", Globals.ColorInputcClarification);

                    var options = new List<Option<int>>();

                    foreach (ScheduleModel schedule in schedules)
                    {
                        if (schedule.ScheduledDateTimeStart.Date == dateTime.Value.Date && ReservationLogic.HasUserAlreadyReservedScheduledExperienceOnDateTimeForLocation(UserLogic.CurrentUser.Id, schedule.ScheduledDateTimeStart, location) == false)
                        {
                            options.Add(new Option<int>(schedule.Id, schedule.ScheduledDateTimeStart.ToString("HH:mm"), () => ExperienceReservation.Start(experienceId, location, schedule.ScheduledDateTimeStart)));
                        }
                    }
                    options.Add(new Option<int>(0, "Terug", () => ExperienceReservation.Start(experienceId, location)));

                    new SelectionMenuUtil<int>(options).Create();
                }
                else
                {
                    ColorConsole.WriteColorLine("[Tijd:] " + dateTime.Value.ToString("HH:mm"), Globals.ExperienceColor);
                }

                bool singleScheduled = false;

                if (room == 0)
                {
                    List<ScheduleModel> scheduledExperience = ScheduleLogic.GetRoomForScheduledExperience(experienceId, location, dateTime);

                    if (scheduledExperience.Count > 1)
                    {

                        ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [zalen]:", Globals.ColorInputcClarification);

                        var options = new List<Option<int>>();

                        foreach (ScheduleModel schedule in scheduledExperience)
                        {

                            options.Add(new Option<int>(schedule.Id, $"Zaal {roomLogic.GetById(schedule.RoomId).RoomNumber}", () => ExperienceReservation.Start(experienceId, location, dateTime, roomLogic.GetById(schedule.RoomId).RoomNumber)));

                        }
                        options.Add(new Option<int>(0, "Terug", () => ExperienceReservation.Start(experienceId, location, dateTime.Value.Date)));

                        new SelectionMenuUtil<int>(options).Create();
                    }
                    else
                    {
                        singleScheduled = true;
                        room = roomLogic.GetById(scheduledExperience[0].RoomId).RoomNumber;
                        ColorConsole.WriteColorLine("[Zaal:] " + room, Globals.ExperienceColor);
                    }
                }
                else
                {
                    ColorConsole.WriteColorLine("[Zaal:] " + room, Globals.ExperienceColor);
                }

                var optionsConfirm = new List<Option<int>>
                {
                    new Option<int>(0, "Bevestig reservering", () =>
                        {
                            int scheduleId = ScheduleLogic.GetRelatedScheduledExperience(experienceId, location, dateTime, room);

                            if (UserLogic.CurrentUser != null && scheduleId != 0)
                            {
                                if (!ReservationLogic.HasUserAlreadyReservedScheduledExperience(scheduleId, UserLogic.CurrentUser.Id))
                                {
                                    PaymentSimulation.Start();

                                    if (ReservationLogic.Complete(scheduleId, UserLogic.CurrentUser.Id))
                                    {
                                        ColorConsole.WriteColorLine("\nReservering bevestigd", Globals.SuccessColor);

                                        ColorConsole.WriteColorLine("Druk op een [toets] om terug te gaan naar de experience details", Globals.ColorInputcClarification);

                                        Console.ReadKey();

                                        ExperienceDetails.Start(experienceId);
                                    }
                                    else
                                    {
                                        ColorConsole.WriteColorLine("\nReservering mislukt", Globals.ErrorColor);

                                        ColorConsole.WriteColorLine("Druk op een [toets] om terug te gaan naar de experience details om het opnieuw te proberen.", Globals.ColorInputcClarification);

                                        Console.ReadKey();

                                        ExperienceDetails.Start(experienceId);
                                    }
                                }
                                else
                                {
                                    ColorConsole.WriteColorLine("\nReservering mislukt", Globals.ErrorColor);

                                    Console.WriteLine("Je hebt al voor deze experience gereserveerd");

                                    ColorConsole.WriteColorLine("Druk op een [toets] om terug te gaan naar de experience details", Globals.ColorInputcClarification);

                                    Console.ReadKey();

                                    ExperienceDetails.Start(experienceId);
                                }

                            }
                        }),
                    new Option<int>(0, "Terug", () => {
                        if(singleScheduled == false)
                        {
                            ExperienceReservation.Start(experienceId, location, dateTime);
                        }
                        else
                        {
                            ExperienceReservation.Start(experienceId, location, dateTime.Value.Date);
                        }
                        })
                };

                ColorConsole.WriteColorLine("\nReservering bevestigen", Globals.ExperienceColor);
                Console.WriteLine("Is de reservering correct?\n");

                new SelectionMenuUtil<int>(optionsConfirm).Create();
            }
        }
    }
}