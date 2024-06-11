namespace BioscoopReserveringsapplicatie
{
    public static class ScheduledExperienceSlotDetails
    {
        private static RoomLogic RoomLogic = new RoomLogic();
        private static LocationLogic LocationLogic = new LocationLogic();
        private static ScheduleLogic ScheduleLogic = new ScheduleLogic();
        private static ExperienceLogic ExperienceLogic = new ExperienceLogic();
        private static ReservationLogic ReservationLogic = new ReservationLogic();

        public static void Start(int experienceId, int scheduleId, DateTime date)
        {
            Console.Clear();

            ExperienceModel experience = ExperienceLogic.GetById(experienceId);

            ScheduleModel schedule = ScheduleLogic.GetById(scheduleId);

            RoomModel chosenRoom = RoomLogic.GetById(schedule.RoomId);

            ColorConsole.WriteColorLine("Details van ingeplande experience tijdslot:", Globals.ExperienceColor);
            ColorConsole.WriteColorLine("[Experience: ]" + experience.Name, Globals.ExperienceColor);
            ColorConsole.WriteColorLine("[Locatie: ]" + LocationLogic.GetById(schedule.LocationId).Name, Globals.ExperienceColor);
            ColorConsole.WriteColorLine("[Zaal: ]" + chosenRoom.RoomNumber, Globals.ExperienceColor);
            ColorConsole.WriteColorLine("[Begintijd: ]" + schedule.ScheduledDateTimeStart.ToString("dd-MM-yyyy HH:mm"), Globals.ExperienceColor);
            ColorConsole.WriteColorLine("[Eindtijd: ]" + schedule.ScheduledDateTimeEnd.ToString("dd-MM-yyyy HH:mm"), Globals.ExperienceColor);
            ColorConsole.WriteColorLine($"\nEr zijn nog [{100 - ReservationLogic.GetAllReservedSeatsOfSchedule(scheduleId).Count} stoelen] beschikbaar.", Globals.ExperienceColor);
            ColorConsole.WriteColorLine("\n[Overzicht van de zaal: ]", Globals.ExperienceColor);

            ReservationLogic.GetAll();

            List<(int, int)> selectedOptions = ReservationLogic.GetAllReservedSeatsOfSchedule(scheduleId);

            if (chosenRoom.RoomType == RoomType.Square)
            {
                RenderGridSquare(selectedOptions);
            }
            else if (chosenRoom.RoomType == RoomType.Round)
            {
                RenderGridRound(selectedOptions);
            }

            Console.WriteLine("\nWat wil je doen?");
            List<Option<string>> options = new List<Option<string>>
            {
                new Option<string>("Ga terug", () => ScheduledExperienceDetails.Start(experienceId, date))

            };
            new SelectionMenuUtil<string>(options).Create();
        }

        private static void RenderGridSquare(List<(int, int)> selectedOptions)
        {
            int rows = 10;
            int columns = 10;

            Console.WriteLine("    1 2 3 4 5 6 7 8 9 10");

            for (int row = 0; row < rows; row++)
            {
                Console.Write((row + 1).ToString().PadLeft(2) + ":");

                for (int column = 0; column < columns; column++)
                {
                    if (selectedOptions.Contains((row, column)))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.Write(" X");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.ForegroundColor = ConsoleColor.White;
                        Console.Write(" X");
                        Console.ResetColor();
                    }
                }

                Console.WriteLine();
            }
        }

        private static void RenderGridRound(List<(int, int)> selectedOptions)
        {
            int rows = 10;
            int columns = 10;

            Console.WriteLine("    1 2 3 4 5 6 7 8 9 10");

            for (int row = 0; row < rows; row++)
            {
                Console.Write((row + 1).ToString().PadLeft(2) + ":");

                for (int column = 0; column < columns; column++)
                {
                    bool isSeat = false;

                    if ((row == 0 && column >= 3 && column <= 6) ||
                    ((row == 1 || row == 2) && column >= 2 && column <= 7) ||
                    (row >= 3 && row <= 6) ||
                    ((row == 7 || row == 8) && column >= 2 && column <= 7) ||
                    (row == 9 && column >= 3 && column <= 6) ||
                    (row == 1 && (column == 2 || column == 3)) ||
                    (row == 7 && (column == 2 || column == 3)) ||
                    (row == 1 && column == 8) ||
                    (row == 2 && column == 8) ||
                    (row == 3 && column == 8) ||
                    (row == 7 && column == 8) ||
                    (row == 8 && column == 8) ||
                    (row == 1 && column == 1) ||
                    (row == 2 && column == 1) ||
                    (row == 7 && column == 1) ||
                    (row == 8 && column == 1))
                    {
                        isSeat = true;
                    }

                    if (isSeat)
                    {
                        if (selectedOptions.Contains((row, column)))
                        {
                            Console.ForegroundColor = ConsoleColor.Red;
                            Console.Write(" X");
                            Console.ResetColor();
                        }
                        else
                        {
                            Console.ForegroundColor = ConsoleColor.White;
                            Console.Write(" X");
                            Console.ResetColor();
                        }
                    }
                    else
                    {
                        Console.Write("  ");
                    }
                }

                Console.WriteLine();
            }
        }
    }
}