using NSubstitute;

namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class RoomLogicTest
    {
        RoomLogic roomLogic;

        [TestInitialize]
        public void Initialize()
        {
            var roomRepositoryMock = Substitute.For<IDataAccess<RoomModel>>();
            List<RoomModel> rooms = new List<RoomModel>() {
                new RoomModel(0, 0, 1, RoomType.Square, Status.Active),
                new RoomModel(1, 0, 2, RoomType.Round, Status.Archived),
                new RoomModel(2, 0, 3, RoomType.Square, Status.Active),
            };
            roomRepositoryMock.LoadAll().Returns(rooms);
            roomRepositoryMock.WriteAll(Arg.Any<List<RoomModel>>());

            roomLogic = new RoomLogic(roomRepositoryMock);
        }

        // RoomType ------------------------------------------------------------------------------------------------------------------

        [DataRow(RoomType.Square)]
        [DataRow(RoomType.Round)]
        [DataTestMethod]
        public void Correct_Room_RoomType_Validation_With_RoomType(RoomType roomType)
        {
            Assert.IsTrue(roomLogic.ValidateRoomType(roomType));
        }

        [DataRow((RoomType)230)]
        [DataRow((RoomType)1230)]
        [DataTestMethod]
        public void Incorrect_Room_RoomType_Validation_With_RoomType(RoomType roomType)
        {
            Assert.IsFalse(roomLogic.ValidateRoomType(roomType));
        }

        // RoomNumber ------------------------------------------------------------------------------------------------------------------

        [DataRow(1)]
        [DataRow(2)]
        [DataTestMethod]
        public void Correct_Room_RoomNumber_Validation_With_RoomNumber(int roomNumber)
        {
            Assert.IsTrue(roomLogic.ValidateRoomNumber(roomNumber));
        }

        [DataRow(-1)]
        [DataRow(-2)]
        [DataTestMethod]
        public void Incorrect_Room_RoomNumber_Validation_With_RoomNumber(int roomNumber)
        {
            Assert.IsFalse(roomLogic.ValidateRoomNumber(roomNumber));
        }

        // Room ----------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Room()
        {
            RoomModel room = new RoomModel(0, 0, 1, RoomType.Square, Status.Active);
            Assert.IsTrue(roomLogic.Validate(room));
        }

        [TestMethod]
        public void Incorrect_Room_Null()
        {
            Assert.IsFalse(roomLogic.Validate(null));
        }

        // Archive ----------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Room_Archive_Succes()
        {
            roomLogic.Archive(0);
            Assert.AreEqual(Status.Archived, roomLogic.GetById(0).Status);
        }

        [TestMethod]
        public void Correct_Room_AlreadyArchived_Still_Archived()
        {
            roomLogic.Archive(1);
            roomLogic.Archive(1);
            Assert.AreEqual(Status.Archived, roomLogic.GetById(1).Status);
        }

        [TestMethod]
        public void Incorrect_Room_Archive_Nonexistent_ID()
        {
            int nonexistentID = 999;
            roomLogic.Archive(nonexistentID);
            Assert.IsNull(roomLogic.GetById(nonexistentID));
        }

    }
}