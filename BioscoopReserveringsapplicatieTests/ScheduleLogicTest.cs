using NSubstitute;

namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class ScheduleLogicTest
    {
        ScheduleLogic scheduleLogic;
        LocationLogic locationLogic;
        RoomLogic roomLogic;
        ExperienceLogic experiencesLogic;
        MovieLogic moviesLogic;
        UserLogic userLogic;

        List<ScheduleModel> schedules;

        Action initializeScheduleLogic;

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

            var LocationRepositoryMock = Substitute.For<IDataAccess<LocationModel>>();
            List<LocationModel> Locations = new List<LocationModel>() {
                new LocationModel(1,"Rotterdam-Zuid", Status.Active),
                new LocationModel(2,"Rotterdam-Noord", Status.Active),
                new LocationModel(3,"Rotterdam-Centrum", Status.Active),
                new LocationModel(4,"Rotterdam-West", Status.Active),
                new LocationModel(5,"Rotterdam-Oost", Status.Active),
                new LocationModel(6,"Wijnhaven", Status.Active),
            };
            LocationRepositoryMock.LoadAll().Returns(Locations);
            LocationRepositoryMock.WriteAll(Arg.Any<List<LocationModel>>());

            var roomRepositoryMock = Substitute.For<IDataAccess<RoomModel>>();
            List<RoomModel> rooms = new List<RoomModel>() {
                new RoomModel(0, 0, 1, RoomType.Square, Status.Active),
                new RoomModel(1, 0, 2, RoomType.Round, Status.Archived),
                new RoomModel(2, 0, 3, RoomType.Square, Status.Active),
            };
            roomRepositoryMock.LoadAll().Returns(rooms);
            roomRepositoryMock.WriteAll(Arg.Any<List<RoomModel>>());

            var experienceRepositoryMock = Substitute.For<IDataAccess<ExperienceModel>>();
            List<ExperienceModel> experiences = new List<ExperienceModel>() {
                new ExperienceModel(1, "Dit Is Experience 1", "", 0, Intensity.Low, 20, Status.Active),
                new ExperienceModel(2, "Dit Is Experience 2", "", 0, Intensity.Medium, 20, Status.Archived),
                new ExperienceModel(3, "Dit Is Experience 3", "", 0, Intensity.High, 20, Status.Active),
            };
            experienceRepositoryMock.LoadAll().Returns(experiences);
            experienceRepositoryMock.WriteAll(Arg.Any<List<ExperienceModel>>());

            var movieRepositoryMock = Substitute.For<IDataAccess<MovieModel>>();
            List<MovieModel> movies = new List<MovieModel>() {
                new MovieModel(1, "Movie1", "Description1", new List<Genre> { Genre.Action, Genre.Comedy }, AgeCategory.AGE_9, Status.Active),
                new MovieModel(2, "Movie2", "Description2", new List<Genre> { Genre.Western, Genre.Horror }, AgeCategory.AGE_12, Status.Archived),
                new MovieModel(3, "Movie3", "Description3", new List<Genre> { Genre.War, Genre.Documentary }, AgeCategory.AGE_16, Status.Active),
            };
            movieRepositoryMock.LoadAll().Returns(movies);
            movieRepositoryMock.WriteAll(Arg.Any<List<MovieModel>>());

            var userRepositoryMock = Substitute.For<IDataAccess<UserModel>>();
            List<UserModel> users = new List<UserModel>() {
                new UserModel(1, true, "admin@mail.com", PasswordHasher.HashPassword("admin", out var salt), salt, "admin", null, default, default, default, null),
            };
            userRepositoryMock.LoadAll().Returns(users);
            userRepositoryMock.WriteAll(Arg.Any<List<UserModel>>());

            initializeScheduleLogic = () => scheduleLogic = new ScheduleLogic(scheduleRepositoryMock, LocationRepositoryMock, roomRepositoryMock, experienceRepositoryMock);
            scheduleLogic = new ScheduleLogic(scheduleRepositoryMock, LocationRepositoryMock, roomRepositoryMock, experienceRepositoryMock);

            locationLogic = new LocationLogic(LocationRepositoryMock, scheduleRepositoryMock);

            roomLogic = new RoomLogic(roomRepositoryMock);

            experiencesLogic = new ExperienceLogic(experienceRepositoryMock, movieRepositoryMock, scheduleRepositoryMock);

            moviesLogic = new MovieLogic(movieRepositoryMock);

            userLogic = new UserLogic(userRepositoryMock);
        }

        // Schedule an experience ----------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Schedule_Experience()
        {
            userLogic.Login("admin@mail.com", "admin");

            int experienceId = 1;
            int locationId = 1;
            int roomId = 1;

            string scheduleDate = DateTime.Now.ToString("dd-MM-yyyy");
            string scheduleTime = DateTime.Now.ToString("HH:mm");
            string scheduledDateTime = $"{scheduleDate} {scheduleTime}";

            ScheduleModel scheduleResult = scheduleLogic.CreateSchedule(experienceId, roomId, locationId, scheduledDateTime);

            Assert.AreEqual(experienceId, scheduleResult.ExperienceId);
            Assert.AreEqual(roomId, scheduleResult.RoomId);
            Assert.AreEqual(locationId, scheduleResult.LocationId);
            Assert.AreEqual(DateTime.Parse(scheduledDateTime), scheduleResult.ScheduledDateTimeStart);
            Assert.AreEqual(DateTime.Parse(scheduledDateTime).AddMinutes(experiencesLogic.GetById(experienceId).TimeLength), scheduleResult.ScheduledDateTimeEnd);
        }

        // Check if timeslot is open ----------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_TimeSlotOpenOnRoom()
        {
            int experienceId = 1;
            int locationId = 1;
            int roomId = 1;

            string scheduleDate = DateTime.Now.ToString("dd-MM-yyyy");
            string scheduleTime = DateTime.Now.ToString("HH:mm");
            string scheduledDateTime = $"{scheduleDate} {scheduleTime}";
            string error = "";

            bool result = scheduleLogic.TimeSlotOpenOnRoom(experienceId, locationId, roomId, scheduledDateTime, out error);

            Assert.IsTrue(result);
            Assert.AreEqual("", error);
        }

        // Check if timeslot is closed ----------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_TimeSlotNotOpenOnRoom()
        {
            int experienceId = 1;
            int locationId = 1;
            int roomId = 1;
            DateTime currentDate = DateTime.Now;
            string scheduleDate = currentDate.AddDays(1).ToString("dd-MM-yyyy");
            string scheduleTime = currentDate.AddDays(1).ToString("HH:mm");
            string scheduledDateTime = $"{scheduleDate} {scheduleTime}";
            string error = "";
            initializeScheduleLogic();
            bool result = scheduleLogic.TimeSlotOpenOnRoom(experienceId, locationId, roomId, scheduledDateTime, out error);
            LocationModel location = locationLogic.GetById(locationId);
            RoomModel room = roomLogic.GetById(roomId);
            ExperienceModel experience = experiencesLogic.GetById(experienceId);
            Assert.IsFalse(result);
            Assert.AreEqual($"Er is al een experience ingepland op {scheduleDate} in {location.Name} Zaal: {room.RoomNumber} van {DateTime.Parse(scheduledDateTime).ToString("HH:mm:ss")} T/M {DateTime.Parse(scheduledDateTime).AddMinutes(experience.TimeLength).ToString("HH:mm:ss")}.", error);
        }
    }
}
