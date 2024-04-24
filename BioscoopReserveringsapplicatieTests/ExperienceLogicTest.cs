using NSubstitute;

namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class ExperienceTest
    {
        ExperienceLogic experiencesLogic;

        [TestInitialize]
        public void Initialize()
        {
            var experienceRepositoryMock = Substitute.For<IDataAccess<ExperienceModel>>();
            List<ExperienceModel> experiences = new List<ExperienceModel>() {
                new ExperienceModel(0, "", "", 0, Intensity.Low, 20, false),
                new ExperienceModel(1, "", "", 0, Intensity.Medium, 20, true),
                new ExperienceModel(2, "", "", 0, Intensity.High, 20, false),
            };
            experienceRepositoryMock.LoadAll().Returns(experiences);
            experienceRepositoryMock.WriteAll(Arg.Any<List<ExperienceModel>>());

            experiencesLogic = new ExperienceLogic(experienceRepositoryMock);
        }

        // Name ------------------------------------------------------------------------------------------------------------------

        [DataRow("Test")]
        [DataRow("CrazyName")]
        [DataTestMethod]
        public void Correct_Experience_Name_Validation(string name)
        {
            Assert.IsTrue(experiencesLogic.ValidateExperienceName(name));
        }

        [TestMethod]
        public void Incorrect_Experience_Name_Validation_With_Experience()
        {
            ExperienceModel experience = new ExperienceModel("", "", 0, Intensity.Low, 0, false);
            Assert.IsFalse(experiencesLogic.ValidateExperience(experience));
            ExperienceModel experience2 = new ExperienceModel(null, null, 0, Intensity.Low, 0, false);
            Assert.IsFalse(experiencesLogic.ValidateExperience(experience2));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Experience_Name_Validation_With_Name(string name)
        {
            Assert.IsFalse(experiencesLogic.ValidateExperienceName(name));
        }

        [TestMethod]
        public void Incorrect_Experience_Intensity_With_Experience()
        {
            ExperienceModel experience = new ExperienceModel("test1", "test1", 0, (Intensity)909, 10, false);
            Assert.IsFalse(experiencesLogic.ValidateExperience(experience));
            ExperienceModel experience2 = new ExperienceModel("test1", "test1", 0, (Intensity)1002, 10, false);
            Assert.IsFalse(experiencesLogic.ValidateExperience(experience2));
        }

        // Intensity ------------------------------------------------------------------------------------------------------------------

        [DataRow(Intensity.Low)]
        [DataRow(Intensity.Medium)]
        [DataRow(Intensity.High)]
        [DataTestMethod]
        public void Correct_Experience_Intensity_Validation_With_Intensity(Intensity intensity)
        {
            Assert.IsTrue(experiencesLogic.ValidateExperienceIntensity(intensity));
        }

        [DataRow((Intensity)909)]
        [DataRow((Intensity)1002)]
        [DataTestMethod]
        public void Incorrect_Experience_Intensity_Validation_With_Intensity(Intensity intensity)
        {
            Assert.IsFalse(experiencesLogic.ValidateExperienceIntensity(intensity));
        }

        // TimeLength ------------------------------------------------------------------------------------------------------------------

        [DataRow(0)]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(20)]
        [DataTestMethod]
        public void CorrectExperienceTimeLength(int time)
        {
            Assert.IsTrue(experiencesLogic.ValidateExperienceTimeLength(time));
        }

        [TestMethod]
        public void Incorrect_Experience_TimeLength_Validation_With_Experience()
        {
            ExperienceModel experience = new ExperienceModel("test1", "test1", 0, Intensity.Low, -10, false);
            Assert.IsFalse(experiencesLogic.ValidateExperience(experience));
        }

        [DataRow(-6)]
        [DataRow(-5)]
        [DataRow(-4)]
        [DataRow(-3)]
        [DataRow(-2)]
        [DataRow(-1)]
        [DataTestMethod]
        public void Incorrect_Experience_TimeLength_Validation_With_Time(int time)
        {
            Assert.IsFalse(experiencesLogic.ValidateExperienceTimeLength(time));
        }

        // Experience ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CorrectExperience()
        {
            ExperienceModel experience = new ExperienceModel("test1", "test1", 0, Intensity.High, 10, false);
            Assert.IsTrue(experiencesLogic.ValidateExperience(experience));
        }

        [TestMethod]
        public void Incorrect_Experience_Null()
        {
            Assert.IsFalse(experiencesLogic.ValidateExperience(null));
        }

        // Archive ----------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Experience_Archive_Success()
        {
            experiencesLogic.Archive(0);
            Assert.IsTrue(experiencesLogic.GetById(0).Archived);
        }

        [TestMethod]
        public void Correct_Experience_AlreadyArchived_Still_Archived()
        {
            experiencesLogic.Archive(1);
            Assert.IsTrue(experiencesLogic.GetById(1).Archived);
        }

        [TestMethod]
        public void Correct_Multiple_Experience_Archive_Success()
        {
            experiencesLogic.Archive(0);
            experiencesLogic.Archive(2);
            Assert.IsTrue(experiencesLogic.GetById(0).Archived);
            Assert.IsTrue(experiencesLogic.GetById(2).Archived);
        }

        [TestMethod]
        public void Incorrect_Experience_Archive_Nonexistent_ID()
        {
            int nonexistentID = 999;
            experiencesLogic.Archive(nonexistentID);
            Assert.IsNull(experiencesLogic.GetById(nonexistentID));
        }
    }
}