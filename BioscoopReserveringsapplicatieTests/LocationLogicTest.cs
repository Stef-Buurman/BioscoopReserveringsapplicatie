using NSubstitute;

namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class LocationLogicTests
    {
        LocationLogic locationLogic;

        [TestInitialize]
        public void Initialize()
        {
            var LocationRepositoryMock = Substitute.For<IDataAccess<LocationModel>>();
            List<LocationModel> Locations = new List<LocationModel>() {
                new LocationModel(1,"Rotterdam-Zuid", Status.Active),
                new LocationModel(2,"Rotterdam-Noord", Status.Archived),
                new LocationModel(3,"Rotterdam-Centrum", Status.Active),
                new LocationModel(4,"Rotterdam-West", Status.Active),
                new LocationModel(5,"Rotterdam-Oost", Status.Active),
                new LocationModel(6,"Wijnhaven", Status.Active),
            };
            LocationRepositoryMock.LoadAll().Returns(Locations);
            LocationRepositoryMock.WriteAll(Arg.Any<List<LocationModel>>());

            locationLogic = new LocationLogic(LocationRepositoryMock);
        }

     // Archive ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Archive_Location()
        {
            locationLogic.Archive(3);
            Assert.AreEqual(locationLogic.GetById(3).Status, Status.Archived);
        }

        [TestMethod]
        public void Incorrect_Archive_Location_Nonexistant_ID()
        {
            try
            {
                bool result = locationLogic.Archive(999);
                Assert.IsFalse(result);
            }
            catch 
            {
                Assert.Fail("Locatie bestaat niet, kan daarom ook niet gearchiveerd worden");
            }
        }

        [TestMethod]
        public void Correct_Location_AlreadyArchived_Still_Archived()
        {
            locationLogic.Archive(2);
            Assert.AreEqual(Status.Archived, locationLogic.GetById(2).Status);
        }

    // UnArchive ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Unarchive_Location()
        {
            locationLogic.UnArchive(3);
            Assert.AreEqual(locationLogic.GetById(3).Status, Status.Active);
        }

        [TestMethod]
        public void Incorrect_Unarchive_Location_Nonexistant_ID()
        {
            try
            {
                bool result = locationLogic.UnArchive(999);
                Assert.IsFalse(result);
            }
            catch 
            {
                Assert.Fail("Locatie bestaat niet, kan daarom ook niet geactiveerd worden");
            }
        }

        [TestMethod]
        public void Correct_Location_AlreadyUnarchived_Still_Unarchived()
        {
            locationLogic.UnArchive(1);
            Assert.AreEqual(Status.Active, locationLogic.GetById(1).Status);
        }

    // Validate ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Name_Location_Validation()
        {
            bool result = locationLogic.Validate(new LocationModel(10, "Rotterdam-TEST", Status.Active));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Incorrect_Name_Location_Validation_Empty_Name()
        {
            bool result = locationLogic.Validate(new LocationModel(10, "", Status.Active));
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Incorrect_Name_Location_Validation_Location_Already_Exists()
        {
            bool result = locationLogic.Validate(new LocationModel(10, "Rotterdam-Zuid", Status.Active));
            Assert.IsFalse(result);
        }
    
    //GetById ------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Get_By_Id()
        {
            LocationModel userName = locationLogic.GetById(1);
            Assert.IsNotNull(userName);
        }

        [TestMethod]
        public void Incorrect_Get_By_Id_Nonexistant_ID()
        {
            LocationModel userName = locationLogic.GetById(999);
            Assert.IsNull(userName);
        }
    }    
}



