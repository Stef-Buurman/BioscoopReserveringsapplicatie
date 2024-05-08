namespace BioscoopReserveringsapplicatie
{
    public static class UserReservations
    {
        private static ReservationLogic reservationLogic = new ReservationLogic();
        private static ScheduleLogic scheduleLogic = new ScheduleLogic();
        private static ExperienceLogic experienceLogic = new ExperienceLogic();
        private static LocationLogic locationLogic = new LocationLogic();
        private static RoomLogic roomLogic = new RoomLogic();
        private static Func<ReservationModel, string[]> reservationDataExtractor = ExtractReservationData;

        public static void Start()
        {
            Console.Clear();

            Console.WriteLine("Dit zijn jouw reserveringen:");

            List<ReservationModel> reservations = reservationLogic.GetByUserId(UserLogic.CurrentUser.Id);

            if (reservations.Count == 0)
            {
                Console.WriteLine("Je hebt nog geen reserveringen gemaakt.");
                Console.WriteLine("Druk op een toets om terug te gaan.");
                Console.ReadKey();
                UserMenu.Start();
            }
            else
            {
                ShowReservations(reservations);
            }
        }

        private static void ShowReservationDetails(int reservationId)
        {
            if (reservationId != 0)
            {
                ReservationDetails.Start(reservationId);
            }
        }

        private static void ShowReservations(List<ReservationModel> reservations)
        {
            Console.Clear();
            List<Option<int>> options = new List<Option<int>>();

            List<string> columnHeaders = new List<string>
            {
                "Experience Naam",
                "Locatie",
                "Zaalnummer",
                "Starttijd",
                "Eindtijd"
            };

            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, reservations, reservationDataExtractor);

            foreach (ReservationModel reservation in reservations)
            {
                ScheduleModel schedule = scheduleLogic.GetById(reservation.ScheduleId);

                ExperienceModel experience = experienceLogic.GetById(schedule.ExperienceId);

                LocationModel location = locationLogic.GetById(schedule.LocationId);

                RoomModel room = roomLogic.GetById(schedule.RoomId);

                string reservationTitle = experience.Name;
                if (reservationTitle.Length > 25)
                {
                    reservationTitle = reservationTitle.Substring(0, 25) + "...";
                }

                string reservationLocation = location.Name;
                if (reservationLocation.Length > 25)
                {
                    reservationLocation = reservationLocation.Substring(0, 25) + "...";
                }

                string reservationInfo = string.Format("{0,-" + (columnWidths[0] + 2) + "} {1,-" + (columnWidths[1] + 2) + "} {2,-" + (columnWidths[2] + 2) + "} {3,-" + (columnWidths[3] + 2) + "} {4,-" + columnWidths[4] + "}",
                reservationTitle, reservationLocation, room.RoomNumber.ToString(), schedule.ScheduledDateTimeStart.ToString("dd-MM-yyyy HH:mm"), schedule.ScheduledDateTimeEnd.ToString("dd-MM-yyyy HH:mm"));
                options.Add(new Option<int>(reservation.Id, reservationInfo));
            }
            ColorConsole.WriteLineInfo("*Klik op escape om dit onderdeel te verlaten*\n");
            ColorConsole.WriteColorLine("Dit zijn alle reserveringen die momenteel beschikbaar zijn:\n", Globals.TitleColor);
            Print();
            int reservationId = new SelectionMenuUtil2<int>(options,
                () =>
                {
                    UserDetails.Start();
                },
                () =>
                {
                    ShowReservations(reservations);
                }, showEscapeabilityText: false).Create();
            Console.Clear();
            ShowReservationDetails(reservationId);
        }

        private static void Print()
        {
            List<string> columnHeaders = new List<string>
            {
                "Experience Naam",
                "Locatie",
                "Zaalnummer",
                "Starttijd",
                "Eindtijd"
            };

            List<ReservationModel> allReservations = reservationLogic.GetByUserId(UserLogic.CurrentUser.Id);

            int[] columnWidths = TableFormatUtil.CalculateColumnWidths(columnHeaders, allReservations, reservationDataExtractor);

            Console.Write("".PadRight(3));
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write(columnHeaders[i].PadRight(columnWidths[i] + 3));
            }
            Console.WriteLine();

            Console.Write("".PadRight(3));
            for (int i = 0; i < columnHeaders.Count; i++)
            {
                Console.Write("".PadRight(columnWidths[i], '-').PadRight(columnWidths[i] + 3));
            }
            Console.WriteLine();
        }

        private static string[] ExtractReservationData(ReservationModel reservation)
        {

            ScheduleModel? schedule = scheduleLogic.GetById(reservation.ScheduleId);

            ExperienceModel experience = experienceLogic.GetById(schedule.ExperienceId);

            LocationModel? location = locationLogic.GetById(schedule.LocationId);

            RoomModel? room = roomLogic.GetById(schedule.RoomId);

            string[] reservationInfo = {
                experience.Name,
                location.Name,
                room.RoomNumber.ToString(),
                schedule.ScheduledDateTimeStart.ToString("dd-MM-yyyy HH:mm"),
                schedule.ScheduledDateTimeEnd.ToString("dd-MM-yyyy HH:mm")
            };
            return reservationInfo;
        }
    }
}