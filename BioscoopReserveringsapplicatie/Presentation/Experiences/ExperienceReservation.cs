namespace BioscoopReserveringsapplicatie
{
    static class ExperienceReservation
    {
        private static ExperienceLogic ExperienceLogic = new ExperienceLogic();
        private static LocationLogic LocationLogic = new LocationLogic();
        private static ReservationLogic ReservationLogic = new ReservationLogic();
        private static ScheduleLogic ScheduleLogic = new ScheduleLogic();
        private static RoomLogic RoomLogic = new RoomLogic();

        private static bool _singleScheduled = false;

        private static List<(int, int)> SelectedValues = new List<(int, int)>();

        public static void Start(int experienceId, int location = 0, DateTime? dateTime = null, int room = 0, List<(int, int)> seats = default)
        {
            Console.Clear();

            if (experienceId != 0)
            {
                ColorConsole.WriteColorLine("Experience reserveren\n", Globals.TitleColor);
                ColorConsole.WriteColorLine("[Naam experience:] " + ExperienceLogic.GetById(experienceId).Name, Globals.ExperienceColor);

                ChooseLocation(experienceId, location);
                ChooseDate(experienceId, location, dateTime);
                ChooseTime(experienceId, location, dateTime);
                ChooseRoom(experienceId, location, dateTime, room);
                ChooseSeat(experienceId, location, dateTime, room, seats);

                var optionsConfirm = new List<Option<int>>
                {
                    new Option<int>(0, "Bevestig reservering", () =>
                        {
                            SelectedValues = new List<(int, int)>();
                            int scheduleId = ScheduleLogic.GetRelatedScheduledExperience(experienceId, location, dateTime, room);

                            if (UserLogic.CurrentUser != null && scheduleId != 0)
                            {
                                if (!ReservationLogic.HasUserAlreadyReservedScheduledExperience(scheduleId, UserLogic.CurrentUser.Id))
                                {
                                    PaymentSimulation.Start(seats.Count);
                                    if (seats == null) Start(experienceId, location, dateTime, room);
                                    if (ReservationLogic.Complete(scheduleId, UserLogic.CurrentUser.Id, seats))
                                    {
                                        WaitUtil.WaitTime(1000);

                                        Console.Clear();
                                        ColorConsole.WriteColorLine("Experience gereserveerd\n", Globals.TitleColor);
                                        ColorConsole.WriteColorLine("[Naam experience:] " + ExperienceLogic.GetById(experienceId).Name, Globals.ExperienceColor);
                                        ColorConsole.WriteColorLine("[Locatie:] " + LocationLogic.GetById((int)location).Name, Globals.ExperienceColor);
                                        ColorConsole.WriteColorLine("[Datum:] " + dateTime.Value.ToString("dd-MM-yyyy"), Globals.ExperienceColor);
                                        ColorConsole.WriteColorLine("[Tijd:] " + dateTime.Value.ToString("HH:mm"), Globals.ExperienceColor);
                                        ColorConsole.WriteColorLine("[Zaal:] " + room, Globals.ExperienceColor);
                                        ColorConsole.WriteColorLine("[Rij:]   " + string.Join(" | ", seats.Select(tuple => tuple.Item1)), Globals.ExperienceColor);
                                        ColorConsole.WriteColorLine("[Stoel:] " + string.Join(" | ", seats.Select(tuple => tuple.Item2)), Globals.ExperienceColor);
                                        ColorConsole.WriteColorLine($"[Prijs:] € {Math.Round(Globals.pricePerSeat * seats.Count, 2)}", Globals.ExperienceColor);

                                        ColorConsole.WriteColorLine("\nReservering geslaagd", Globals.SuccessColor);

                                        ColorConsole.WriteColorLine("Druk op een [toets] om terug te gaan naar het hoofdmenu", Globals.ColorInputcClarification);

                                        Console.ReadKey();

                                        UserMenu.Start();
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
                        if(_singleScheduled == false)
                        {
                            Start(experienceId, location, dateTime, room);
                        }
                        else
                        {
                            Start(experienceId, location, dateTime.Value.Date, room);
                        }
                        })
                };

                ColorConsole.WriteColorLine($"[Prijs:] € {Math.Round(Globals.pricePerSeat * seats.Count, 2)}", Globals.ExperienceColor);

                ColorConsole.WriteColorLine("\nReservering bevestigen", Globals.ExperienceColor);
                Console.WriteLine("Is de reservering correct?\n");

                new SelectionMenuUtil<int>(optionsConfirm).Create();
            }
        }

        private static void ChooseLocation(int experienceId, int location)
        {
            if (location == 0)
            {
                HorizontalLine.Print();
                ColorConsole.WriteColorLine("\nKies een locatie: ", Globals.ExperienceColor);
                List<LocationModel> locations = LocationLogic.GetLocationsForScheduledExperienceById(experienceId);

                ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [locaties]:", Globals.ColorInputcClarification);

                var options = new List<Option<int>>();

                foreach (LocationModel locationSelected in locations)
                {
                    if (ReservationLogic.HasUserReservedAvailableOptionsForLocation(experienceId, locationSelected.Id, UserLogic.CurrentUser.Id) == false)
                    {
                        options.Add(new Option<int>(locationSelected.Id, locationSelected.Name, () => Start(experienceId, locationSelected.Id)));
                    }
                }
                options.Add(new Option<int>(0, "Terug", () => ExperienceDetails.Start(experienceId)));

                new SelectionMenuUtil<int>(options).Create();
            }
            else
            {
                ColorConsole.WriteColorLine("[Locatie:] " + LocationLogic.GetById((int)location).Name, Globals.ExperienceColor);
            }
        }

        private static void ChooseDate(int experienceId, int location, DateTime? dateTime = null)
        {
            if (dateTime == null)
            {
                HorizontalLine.Print();
                ColorConsole.WriteColorLine("\nKies een datum: ", Globals.ExperienceColor);
                List<ScheduleModel> schedules = ScheduleLogic.GetScheduledExperienceDatesForLocationById(experienceId, location);

                schedules.Sort((x, y) => x.ScheduledDateTimeStart.CompareTo(y.ScheduledDateTimeStart));

                schedules = schedules.DistinctBy(s => s.ScheduledDateTimeStart.Date).ToList();

                ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [datums]:", Globals.ColorInputcClarification);

                var options = new List<Option<int>>();

                foreach (ScheduleModel schedule in schedules)
                {
                    if (ScheduleLogic.GetScheduledExperienceTimeSlotsForLocationById(experienceId, location, schedule.ScheduledDateTimeStart.Date).Count > 0)
                    {
                        options.Add(new Option<int>(schedule.Id, schedule.ScheduledDateTimeStart.Date.ToString("dd-MM-yyyy"), () => Start(experienceId, location, schedule.ScheduledDateTimeStart.Date)));
                    }
                }
                options.Add(new Option<int>(0, "Terug", () => Start(experienceId)));

                new SelectionMenuUtil<int>(options).Create();
            }
            else
            {
                ColorConsole.WriteColorLine("[Datum:] " + dateTime.Value.ToString("dd-MM-yyyy"), Globals.ExperienceColor);
            }
        }

        private static void ChooseTime(int experienceId, int location, DateTime? dateTime = null)
        {
            if (dateTime.Value.TimeOfDay == TimeSpan.Zero)
            {
                HorizontalLine.Print();
                ColorConsole.WriteColorLine("\nKies een tijd: ", Globals.ExperienceColor);
                List<ScheduleModel> schedules = ScheduleLogic.GetScheduledExperienceTimeSlotsForLocationById(experienceId, location, dateTime.Value.Date);

                schedules.Sort((x, y) => x.ScheduledDateTimeStart.CompareTo(y.ScheduledDateTimeStart));

                schedules = schedules.DistinctBy(s => s.ScheduledDateTimeStart.TimeOfDay).ToList();

                ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [tijden]:", Globals.ColorInputcClarification);

                var options = new List<Option<int>>();

                foreach (ScheduleModel schedule in schedules)
                {
                    if (schedule.ScheduledDateTimeStart.Date == dateTime.Value.Date && ReservationLogic.HasUserAlreadyReservedScheduledExperienceOnDateTimeForLocation(UserLogic.CurrentUser.Id, schedule.ScheduledDateTimeStart, location) == false)
                    {
                        options.Add(new Option<int>(schedule.Id, schedule.ScheduledDateTimeStart.ToString("HH:mm"), () => Start(experienceId, location, schedule.ScheduledDateTimeStart)));
                    }
                }
                options.Add(new Option<int>(0, "Terug", () => Start(experienceId, location)));

                new SelectionMenuUtil<int>(options).Create();
            }
            else
            {
                ColorConsole.WriteColorLine("[Tijd:] " + dateTime.Value.ToString("HH:mm"), Globals.ExperienceColor);
            }
        }

        private static void ChooseRoom(int experienceId, int location, DateTime? dateTime, int room)
        {
            if (room == 0)
            {
                HorizontalLine.Print();
                ColorConsole.WriteColorLine("\nKies een zaal: ", Globals.ExperienceColor);
                List<ScheduleModel> scheduledExperience = ScheduleLogic.GetRoomForScheduledExperience(experienceId, location, dateTime);

                if (scheduledExperience.Count > 1)
                {
                    ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [zalen]:", Globals.ColorInputcClarification);

                    var options = new List<Option<int>>();

                    foreach (ScheduleModel schedule in scheduledExperience)
                    {

                        options.Add(new Option<int>(schedule.Id, $"Zaal {RoomLogic.GetById(schedule.RoomId).RoomNumber}", () => Start(experienceId, location, dateTime, schedule.RoomId)));

                    }
                    options.Add(new Option<int>(0, "Terug", () => Start(experienceId, location, dateTime.Value.Date)));

                    new SelectionMenuUtil<int>(options).Create();
                }
                else
                {
                    _singleScheduled = true;
                    room = scheduledExperience[0].RoomId;
                    ColorConsole.WriteColorLine("[Zaal:] " + room, Globals.ExperienceColor);
                    Start(experienceId, location, dateTime, room);
                }
            }
            else
            {
                ColorConsole.WriteColorLine("[Zaal:] " + room, Globals.ExperienceColor);
            }
        }
        private static void ChooseSeat(int experienceId, int location, DateTime? dateTime, int room, List<(int, int)> seat = default)
        {
            if (dateTime == null)
            {
                Start(experienceId, location, dateTime);
                return;
            }
            if (seat == null)
            {
                HorizontalLine.Print();
                ColorConsole.WriteColorLine("\nKies uw stoelen: ", Globals.ExperienceColor);
                RoomModel chosenRoom = RoomLogic.GetById(room);
                Option<string>[,] options = OptionGrid.GenerateOptionGrid(10, 10, chosenRoom.RoomType == RoomType.Round);

                int scheduleId = ScheduleLogic.GetRelatedScheduledExperience(experienceId, location, dateTime, room);
                List<(int, int)> selectedOptions = ReservationLogic.GetAllReservedSeatsOfSchedule(scheduleId);

                List<(int, int)> chosenSeats = new SelectionMenuUtil<string>(options, selectedOptions, true,
                    () => Start(experienceId, location, dateTime),
                    () => Start(experienceId, location, dateTime, room), true, SelectedValues).CreateGridSelect(out SelectedValues);

                Start(experienceId, location, dateTime, room, chosenSeats);
            }
            else
            {
                ColorConsole.WriteColorLine("[Rij:]   " + string.Join(" | ", seat.Select(tuple => tuple.Item1)), Globals.ExperienceColor);
                ColorConsole.WriteColorLine("[Stoel:] " + string.Join(" | ", seat.Select(tuple => tuple.Item2)), Globals.ExperienceColor);
            }
        }
    }
}