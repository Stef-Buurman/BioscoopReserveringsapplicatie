namespace BioscoopReserveringsapplicatie
{
    public static class ScheduledExperienceDetails
    {
        private static ExperienceLogic ExperienceLogic = new ExperienceLogic();
        private static LocationLogic LocationLogic = new LocationLogic();
        private static RoomLogic RoomLogic = new RoomLogic();
        private static ScheduleLogic ScheduleLogic = new ScheduleLogic();
        private static Func<ScheduleModel, string[]> scheduleDataExtractor = ExtractScheduleData;

        public static void Start(int experienceId, DateTime date)
        {
            ShowSchedules(experienceId, date);
        }

        private static void ShowScheduleDetails(int experienceId, int scheduleId, DateTime date)
        {
            if (experienceId != 0)
            {
                ScheduledExperienceSlotDetails.Start(experienceId, scheduleId, date);
            }
        }

        private static int ShowSchedules(int experienceId, DateTime date)
        {
            if (UserLogic.CurrentUser == null)
            {
                Console.WriteLine("Geen gebruiker gevonden");
                return 0;
            }

            Console.Clear();

            List<Option<int>> options = new List<Option<int>>();

            List<string> columnHeaders = new List<string>
            {
                "Locatie  ",
                "Zaal",
                "Begintijd",
                "Eindtijd",
            };

            List<ScheduleModel> schedules = ScheduleLogic.GetSchedulesById(experienceId, date);

            if (schedules.Count > 0)
            {
                int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, schedules, scheduleDataExtractor);

                foreach (ScheduleModel schedule in schedules)
                {
                    string locationName = LocationLogic.GetById(schedule.LocationId).Name;
                    if (locationName.Length > 25)
                    {
                        locationName = locationName.Substring(0, 25) + "...";
                    }

                    int roomNumber = RoomLogic.GetById(schedule.RoomId).RoomNumber;

                    DateTime startTime = schedule.ScheduledDateTimeStart;
                    DateTime endTime = schedule.ScheduledDateTimeEnd;

                    string experienceInfo = string.Format("{0,-" + (columnWidths[0] + 1) + "} {1,-" + (columnWidths[1] + 1) + "} {2,-" + (columnWidths[2] + 1) + "} {3,-" + (columnWidths[3] + 1) + "}",
                    locationName, roomNumber, startTime, endTime);
                    options.Add(new Option<int>(schedule.Id, experienceInfo));
                }
                ColorConsole.WriteLineInfoHighlight("*Klik op [Escape] om terug te gaan*", Globals.ColorInputcClarification);

                ColorConsole.WriteColorLine($"Dit zijn de ingeplande slots voor {ExperienceLogic.GetById(experienceId).Name} in deze week:\n", Globals.TitleColor);

                Print(experienceId, date);

                int scheduleId = new SelectionMenuUtil<int>(options,
                    () =>
                    {
                        ScheduledExperiences.Start(date);
                    },
                    () =>
                    {
                        ShowSchedules(experienceId, date);
                    }, showEscapeabilityText: false).Create();

                ShowScheduleDetails(experienceId, scheduleId, date);
                return experienceId;
            }
            return 0;
        }

        private static void Print(int experienceId, DateTime date)
        {
            // Defineer de kolom koppen voor de tabel
            List<string> columnHeaders = new List<string>
            {
                "Locatie  ",
                "Zaal",
                "Begintijd",
                "Eindtijd",
            };

            List<ScheduleModel> schedules = ScheduleLogic.GetSchedulesById(experienceId, date);

            // Bereken de breedte voor elke kolom op basis van de koptekst en schedules
            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, schedules, scheduleDataExtractor);

            // Print de kop van de tabel
            Console.Write("".PadRight(3));
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write(columnHeaders[i].PadRight(columnWidths[i] + 2));
            }
            Console.WriteLine();

            // Print de "----"-lijn tussen de kop en de data
            Console.Write("".PadRight(3));
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write(new string('-', columnWidths[i]));
                if (i < columnHeaders.Count - 1) Console.Write("  ");
            }
            Console.WriteLine();
        }

        private static string[] ExtractScheduleData(ScheduleModel schedule)
        {
            LocationModel location = LocationLogic.GetById(schedule.LocationId);
            RoomModel room = RoomLogic.GetById(schedule.RoomId);

            string[] experienceInfo = {
                location.Name,
                room.RoomNumber.ToString(),
                schedule.ScheduledDateTimeStart.ToString(),
                schedule.ScheduledDateTimeEnd.ToString(),
            };
            return experienceInfo;
        }
    }
}