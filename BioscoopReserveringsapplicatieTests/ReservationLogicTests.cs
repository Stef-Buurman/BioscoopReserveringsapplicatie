using NSubstitute;

namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class ReservationLogicTests
    {
        ReservationLogic reservationLogic;
        ScheduleLogic scheduleLogic;
        List<ReservationModel> reservations;
        List<ScheduleModel> schedules;

        [TestInitialize]
        public void Initialize()
        {
            var scheduleRepositoryMock = Substitute.For<IDataAccess<ScheduleModel>>();
            schedules = new List<ScheduleModel>()
            {
                new ScheduleModel(3, 1, 1, 1, DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddMinutes(10)),
                new ScheduleModel(2, 2, 2, 2, DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddMinutes(10)),
                new ScheduleModel(1, 3, 3, 3, DateTime.Now.AddDays(1), DateTime.Now.AddDays(1).AddMinutes(10)),
            };
            scheduleRepositoryMock.LoadAll().Returns(schedules);
            scheduleRepositoryMock.WriteAll(Arg.Any<List<ScheduleModel>>());

            scheduleLogic = new ScheduleLogic(scheduleRepositoryMock);


            var reservationRepositoryMock = Substitute.For<IDataAccess<ReservationModel>>();
            reservations = new List<ReservationModel>()
            {
                new ReservationModel(1, 1, 3, new List<(int, int)>(){(1,2)}, false),
                new ReservationModel(4, 2, 2, new List<(int, int)>(){(1,2)}, false),
                new ReservationModel(6, 3, 1, new List<(int, int)>(){(1,2)}, false),
            };
            reservationRepositoryMock.LoadAll().Returns(reservations);
            reservationRepositoryMock.WriteAll(Arg.Any<List<ReservationModel>>());

            reservationLogic = new ReservationLogic(reservationRepositoryMock, scheduleRepositoryMock);
        }

        public int scheduleId = 1;
        public int userId = 2;
        public List<(int, int)> seats = new List<(int, int)>() { (1, 2) };

        public void Complete_Reservation()
        {
            reservationLogic.Complete(scheduleId, userId, seats);
        }

        // Reserve for experience ----------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Experience_Reserve_Success()
        {
            int nextID = IdGenerator.GetNextId(reservations);
            Assert.IsTrue(reservationLogic.Complete(scheduleId, userId, seats));

            Assert.IsNotNull(reservationLogic.GetById(nextID));
            Assert.AreEqual(reservationLogic.GetById(nextID).ScheduleId, scheduleId);
            Assert.AreEqual(reservationLogic.GetById(nextID).UserId, userId);
            Assert.AreEqual(reservationLogic.GetById(nextID).Seat, seats);
        }

        [TestMethod]
        public void Incorrect_Experience_Reserve_Wrong_Schedule_Id()
        {
            Assert.IsFalse(reservationLogic.Complete(0, 2, seats));
        }

        [TestMethod]
        public void Incorrect_Experience_Reserve_Wrong_User_Id()
        {
            Assert.IsFalse(reservationLogic.Complete(1, 0, seats));
        }

        // Check if user elready reserved on time ----------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Has_User_Already_Reserverd_For_Experience()
        {
            Complete_Reservation();
            ScheduleModel schedule = scheduleLogic.GetById(scheduleId);
            Assert.IsTrue(reservationLogic.HasUserAlreadyReservedScheduledExperienceOnDateTimeForLocation(userId, schedule.ScheduledDateTimeStart, schedule.LocationId));
        }

        [TestMethod]
        public void Correct_Has_Not_User_Already_Reserverd_For_Experience()
        {
            Complete_Reservation();
            ScheduleModel schedule = scheduleLogic.GetById(scheduleId);
            Assert.IsFalse(reservationLogic.HasUserAlreadyReservedScheduledExperienceOnDateTimeForLocation(userId, schedule.ScheduledDateTimeStart.AddHours(2), schedule.LocationId));
        }

        // Get all reserved seats of schedule ----------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Reserverd_Seats_Of_Schedule()
        {
            Complete_Reservation();
            List<(int, int)> selectedSeats = reservationLogic.GetAllReservedSeatsOfSchedule(scheduleId);
            foreach((int, int) selSeat in selectedSeats)
            {
                Assert.IsTrue(seats.Contains(selSeat));
            }
        }

        // Has user already reserver for location ----------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_User_Has_Available_Options_For_Location()
        {
            ScheduleModel schedule = scheduleLogic.GetById(scheduleId);
            Assert.IsFalse(reservationLogic.HasUserReservedAvailableOptionsForLocation(schedule.ExperienceId, schedule.LocationId, userId));
        }

        [TestMethod]
        public void Correct_User_Has_No_Available_Options_For_Location()
        {
            Complete_Reservation();
            ScheduleModel schedule = scheduleLogic.GetById(scheduleId);
            Assert.IsTrue(reservationLogic.HasUserReservedAvailableOptionsForLocation(schedule.ExperienceId, 2, userId));
        }

        // Cancel Reservation -------------------------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Cancel_Reservation()
        {
            Complete_Reservation();
            ReservationModel reservation = reservationLogic.GetById(scheduleId);
            Assert.IsTrue(reservationLogic.Cancel(reservation));
        }

        [TestMethod]
        public void Incorrect_Cancel_Reservation()
        {
            Assert.IsFalse(reservationLogic.Cancel(null));
        }
    }
}
