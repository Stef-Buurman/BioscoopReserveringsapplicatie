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

            if (!reservation.IsCanceled && schedule.ScheduledDateTimeStart > DateTime.Now)
            {
                options = new List<Option<string>>
                {
                    new Option<string>("Annuleer reservering", () => {

                        Console.Clear();

                        ColorConsole.WriteColorLine($"Weet u zeker dat u de reservering wilt [annuleren]?", Globals.ColorInputcClarification);

                        List<Option<string>> options2 = new List<Option<string>>
                        {
                            new Option<string>("Ja", () => {
                                reservationLogic.Cancel(reservation);
                                Console.Clear();
                                ColorConsole.WriteColorLine($"De reservering is geannuleerd!", Globals.SuccessColor);
                                WaitUtil.WaitTime(2000);
                                Start(reservationId);
                            }),
                            new Option<string>("Nee", () => {
                                Start(reservationId);
                            }),
                        };

                        new SelectionMenuUtil<string>(options2, new Option<string>("Nee")).Create();

                    }),
                    new Option<string>("Terug", () => UserReservations.Start()),
                };
            }
            else
            {
                options = new List<Option<string>>
                {
                    new Option<string>("Terug", () => UserReservations.Start()),
                };
            }

            Print(reservation, schedule, experience, location, room);

            new SelectionMenuUtil<string>(options).Create();
        }

        private static void Print(ReservationModel reservation, ScheduleModel schedule, ExperienceModel experience, LocationModel location, RoomModel room)
        {
            if (schedule != null && experience != null && location != null && room != null)
            {
                ColorConsole.WriteColorLine("[Reservering details]", Globals.ReservationColor);
                ColorConsole.WriteColorLine($"[Experience Naam: ]{experience.Name}", Globals.ReservationColor);
                ColorConsole.WriteColorLine($"[Locatie: ]{location.Name}", Globals.ReservationColor);
                ColorConsole.WriteColorLine($"[Zaalnummer: ]{room.RoomNumber}", Globals.ReservationColor);
                ColorConsole.WriteColorLine($"[Starttijd: ]{schedule.ScheduledDateTimeStart.ToString("dd-MM-yyyy HH:mm")}", Globals.ReservationColor);
                ColorConsole.WriteColorLine($"[Eindtijd: ]{schedule.ScheduledDateTimeEnd.ToString("dd-MM-yyyy HH:mm")}", Globals.ReservationColor);
                ColorConsole.WriteColorLine($"[Status: ]{(reservation.IsCanceled ? "Geannuleerd" : "Actief")}", Globals.ReservationColor);
                ColorConsole.WriteColorLine($"[Prijs:] € {Math.Round(Globals.pricePerSeat * reservation.Seat.Count, 2)}\n", Globals.ReservationColor);

                Console.WriteLine("Wat wil je doen?");
            }
        }
    }
}