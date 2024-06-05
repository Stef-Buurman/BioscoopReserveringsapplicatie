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
                new LocationModel(1,"Rotterdam-Zuid"),
                new LocationModel(2,"Rotterdam-Noord"),
                new LocationModel(3,"Rotterdam-Centrum"),
                new LocationModel(4,"Rotterdam-West"),
                new LocationModel(5,"Rotterdam-Oost"),
                new LocationModel(6,"Wijnhaven"),
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
<<<<<<< HEAD
<<<<<<< HEAD
        public void Incorrect_Archive_Location()
=======
        public void Incorrect_Archive_Location_Nonexistant_ID()
>>>>>>> Made sure everything is TestMethod & fixed some names and added extra test
=======
        public void Incorrect_Archive_Location_Nonexistant_ID()
>>>>>>> 7555db47d79e5ef48b6b22c9a800f24345888e24
        {
            try
            {
                locationLogic.Archive(999);
            }
            catch 
            {
                Assert.Fail("Locatie bestaat niet, kan daarom ook niet gearchiveerd worden");
            }
        }

    // UnArchive ------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void Correct_UnArchive_Location()
        {
            locationLogic.UnArchive(3);
            Assert.AreEqual(locationLogic.GetById(3).Status, Status.Active);
        }

        [TestMethod]
        public void Incorrect_UnArchive_Location()
        {
            try
            {
                locationLogic.UnArchive(999);
            }
            catch 
            {
                Assert.Fail("Locatie bestaat niet, kan daarom ook niet geactiveerd worden");
            }
        }

    // Validate ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Name_Location_Validation()
        {
            bool result = locationLogic.Validate(new LocationModel(10, "Rotterdam-TEST"));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Incorrect_Name_Location_Validation_Empty_Name()
        {
            bool result = locationLogic.Validate(new LocationModel(10, ""));
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Incorrect_Name_Location_Validation_Location_Already_Exists()
        {
            bool result = locationLogic.Validate(new LocationModel(10, "Rotterdam-Zuid"));
            Assert.IsFalse(result);
        }
    
    //GetById------------------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void Correct_Get_By_Id()
        {
            LocationModel userName = locationLogic.GetById(1);
            Assert.IsNotNull(userName);
        }

        [TestMethod]
        public void Incorrect_Get_By_Id()
        {
            LocationModel userName = locationLogic.GetById(999);
            Assert.IsNull(userName);
        }
    }    
}



