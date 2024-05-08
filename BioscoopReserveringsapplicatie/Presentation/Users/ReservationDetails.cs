namespace BioscoopReserveringsapplicatie
{
    public static class ReservationDetails
    {
        private static ReservationLogic reservationLogic = new ReservationLogic();
        private static ScheduleLogic scheduleLogic = new ScheduleLogic();
        private static ExperienceLogic experienceLogic = new ExperienceLogic();
        private static LocationLogic locationLogic = new LocationLogic();
        private static RoomLogic roomLogic = new RoomLogic();

        public static void Start(int reservationId)
        {
            Console.Clear();

            ReservationModel reservation = reservationLogic.GetById(reservationId);

            ScheduleModel schedule = scheduleLogic.GetById(reservation.ScheduleId);

            ExperienceModel experience = experienceLogic.GetById(schedule.ExperienceId);

            LocationModel location = locationLogic.GetById(schedule.LocationId);

            RoomModel room = roomLogic.GetById(schedule.RoomId);

            List<Option<string>> options;

            options = new List<Option<string>>
            {
                new Option<string>("Annuleer reservering", () => {}),
                new Option<string>("Terug", () => UserReservations.Start()),
            };

            Print(schedule, experience, location, room);

            new SelectionMenuUtil2<string>(options).Create();
        }

        private static void Print(ScheduleModel schedule, ExperienceModel experience, LocationModel location, RoomModel room)
        {
            if (schedule != null && experience != null && location != null && room != null)
            {
                ColorConsole.WriteColorLine("[Reservering details]", Globals.ReservationColor);
                ColorConsole.WriteColorLine($"[Experience Naam: ]{experience.Name}", Globals.ReservationColor);
                ColorConsole.WriteColorLine($"[Locatie: ]{location.Name}", Globals.ReservationColor);
                ColorConsole.WriteColorLine($"[Zaalnummer: ]{room.RoomNumber}", Globals.ReservationColor);
                ColorConsole.WriteColorLine($"[Starttijd: ]{schedule.ScheduledDateTimeStart.ToString("dd-MM-yyyy HH:mm")}", Globals.ReservationColor);
                ColorConsole.WriteColorLine($"[Eindtijd: ]{schedule.ScheduledDateTimeEnd.ToString("dd-MM-yyyy HH:mm")}\n", Globals.ReservationColor);
                Console.WriteLine("Wat wil je doen?");
            }
        }
    }
}