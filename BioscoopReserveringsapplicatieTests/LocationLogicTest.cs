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

        public void Incorrect_Archive_Location()
        {
            locationLogic.Archive(999);
            Assert.IsNull(locationLogic.GetById(999).Status = Status.Archived);
        }

    // UnArchive ------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void Correct_UnArchive_Location()
        {
            locationLogic.UnArchive(3);
            Assert.AreEqual(locationLogic.GetById(3).Status, Status.Active);
        }

        public void Incorrect_UnArchive_Location()
        {
            locationLogic.UnArchive(999);
            Assert.IsNull(locationLogic.GetById(999).Status = Status.Active);
        }



    // Validate ------------------------------------------------------------------------------------------------------------------

        [DataTestMethod]
        public void Correct_Name_Location_Validation()
        {
            bool result = locationLogic.Validate(new LocationModel(10, "Rotterdam-TEST"));
            Assert.IsTrue(result);
        }

        [TestMethod]
        public void Incorrect_Name_Location_Validation()
        {
            bool result = locationLogic.Validate(new LocationModel(10, ""));
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



