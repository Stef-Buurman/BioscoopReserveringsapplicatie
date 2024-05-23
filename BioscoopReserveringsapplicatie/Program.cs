using System.Text;

namespace BioscoopReserveringsapplicatie
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            LandingPage.Start();


            Option<string>[,] options = new Option<string>[7, 7]
            {
                { null, null, new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), null,null },
                { null, new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), null },
                { new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X") },
                { new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X") },
                { new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X") },
                { null, new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), null },
                { null,null, new Option<string>("X"), new Option<string>("X"), new Option<string>("X"), null,null }
            };

            List<(int, int)> selectedOptions = new List<(int, int)>
            {
                (0, 0),
                (1, 1),
                (0, 6),
                (0, 1),
                (0, 2),
                (0, 3),
                (0, 4),
                (0, 5),
                (6,5)
            };

            List<(int, int)> selectionMenu = new SelectionMenuUtil<string>(options, selectedOptions, true).CreateGridSelect();
            Console.WriteLine("\nSelected options:");
            foreach (var index in selectionMenu)
            {
                Console.WriteLine($"Selected option at index: ({index.Item1}, {index.Item2})");
            }
            Console.WriteLine(new SelectionMenuUtil<string>(options, selectedOptions).GetMaxColWith(0));
        }
    }
}

//using System;

//namespace BioscoopReserveringsapplicatie
//{
//    static class ExperienceReservation
//    {
//        private static ExperienceLogic ExperienceLogic = new ExperienceLogic();
//        private static LocationLogic LocationLogic = new LocationLogic();
//        private static ReservationLogic ReservationLogic = new ReservationLogic();
//        private static ScheduleLogic ScheduleLogic = new ScheduleLogic();

//        public static void Start(int experienceId, int location = 0, DateTime? dateTime = null, int room = 0)
//        {
//            Console.Clear();

//            if (experienceId != 0)
//            {
//                ColorConsole.WriteColorLine("Experience reserveren\n", Globals.ExperienceColor);

//                ColorConsole.WriteColorLine("[Naam experience:] " + ExperienceLogic.GetById(experienceId).Name, Globals.ExperienceColor);

//                ChooseLocation(experienceId, location);

//                ChooseDateTime(experienceId, location, dateTime);

//                if (room == 0)
//                {
//                    ScheduleModel scheduledExperience = ScheduleLogic.GetRoomForScheduledExperience(experienceId, location, dateTime);

//                    room = scheduledExperience.RoomId;

//                    ColorConsole.WriteColorLine("[Zaal:] " + room, Globals.ExperienceColor);
//                }

//                var optionsConfirm = new List<Option<int>>
//                {
//                    new Option<int>(0, "Bevestig reservering", () =>
//                        {
//                            int scheduleId = ScheduleLogic.GetRelatedScheduledExperience(experienceId, location, dateTime, room);

//                            if (UserLogic.CurrentUser != null && scheduleId != 0)
//                            {
//                                if (!ReservationLogic.HasUserAlreadyReservedScheduledExperience(scheduleId, UserLogic.CurrentUser.Id))
//                                {
//                                    PaymentSimulation.Start();

//                                    if (ReservationLogic.Complete(scheduleId, UserLogic.CurrentUser.Id))
//                                    {
//                                        ColorConsole.WriteColorLine("\nReservering bevestigd", Globals.SuccessColor);

//                                        ColorConsole.WriteColorLine("Druk op een [toets] om terug te gaan naar de experience details", Globals.ColorInputcClarification);

//                                        Console.ReadKey();

//                                        ExperienceDetails.Start(experienceId);
//                                    }
//                                    else
//                                    {
//                                        ColorConsole.WriteColorLine("\nReservering mislukt", Globals.ErrorColor);

//                                        ColorConsole.WriteColorLine("Druk op een [toets] om terug te gaan naar de experience details om het opnieuw te proberen.", Globals.ColorInputcClarification);

//                                        Console.ReadKey();

//                                        ExperienceDetails.Start(experienceId);
//                                    }
//                                }
//                                else
//                                {
//                                    ColorConsole.WriteColorLine("\nReservering mislukt", Globals.ErrorColor);

//                                    Console.WriteLine("Je hebt al voor deze experience gereserveerd");

//                                    ColorConsole.WriteColorLine("Druk op een [toets] om terug te gaan naar de experience details", Globals.ColorInputcClarification);

//                                    Console.ReadKey();

//                                    ExperienceDetails.Start(experienceId);
//                                }

//                            }
//                        }),
//                    new Option<int>(0, "Terug", () => Start(experienceId, location, dateTime.Value.Date))
//                };

//                ColorConsole.WriteColorLine("\nReservering bevestigen", Globals.ExperienceColor);
//                Console.WriteLine("Is de reservering correct?\n");

//                new SelectionMenuUtil<int>(optionsConfirm).Create();
//            }
//        }

//        private static void ChooseDateTime(int experienceId, int location, DateTime? dateTime = null)
//        {
//            if (dateTime == null)
//            {
//                List<ScheduleModel> schedules = ScheduleLogic.GetScheduledExperienceDatesForLocationById(experienceId, location);

//                schedules.Sort((x, y) => x.ScheduledDateTimeStart.CompareTo(y.ScheduledDateTimeStart));

//                schedules = schedules.DistinctBy(s => s.ScheduledDateTimeStart.Date).ToList();

//                ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [datums]:", Globals.ColorInputcClarification);

//                var options = new List<Option<int>>();

//                foreach (ScheduleModel schedule in schedules)
//                {
//                    if (ScheduleLogic.GetScheduledExperienceTimeSlotsForLocationById(experienceId, location, schedule.ScheduledDateTimeStart.Date).Count > 0)
//                    {
//                        options.Add(new Option<int>(schedule.Id, schedule.ScheduledDateTimeStart.Date.ToString("dd-MM-yyyy"), () => Start(experienceId, location, schedule.ScheduledDateTimeStart.Date)));
//                    }
//                }
//                options.Add(new Option<int>(0, "Terug", () => Start(experienceId)));

//                new SelectionMenuUtil<int>(options).Create();
//            }
//            else
//            {
//                ColorConsole.WriteColorLine("[Datum:] " + dateTime.Value.ToString("dd-MM-yyyy"), Globals.ExperienceColor);
//            }
//        }

//        private static void ChooseLocation(int experienceId, int location)
//        {
//            if (location == 0)
//            {
//                List<LocationModel> locations = LocationLogic.GetLocationsForScheduledExperienceById(experienceId);

//                ColorConsole.WriteColorLine("\nMaak een keuze uit een van de onderstaande [locaties]:", Globals.ColorInputcClarification);

//                var options = new List<Option<int>>();

//                foreach (LocationModel locationSelected in locations)
//                {
//                    if (ReservationLogic.HasUserReservedAvailableOptionsForLocation(experienceId, locationSelected.Id, UserLogic.CurrentUser.Id) == false)
//                    {
//                        options.Add(new Option<int>(locationSelected.Id, locationSelected.Name, () => Start(experienceId, locationSelected.Id)));
//                    }
//                }
//                options.Add(new Option<int>(0, "Terug", () => ExperienceDetails.Start(experienceId)));

//                new SelectionMenuUtil<int>(options).Create();
//            }
//            else
//            {
//                ColorConsole.WriteColorLine("[Locatie:] " + LocationLogic.GetById((int)location).Name, Globals.ExperienceColor);
//            }
//        }
//    }
//}