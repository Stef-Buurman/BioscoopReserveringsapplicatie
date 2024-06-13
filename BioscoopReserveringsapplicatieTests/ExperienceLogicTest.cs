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
                new ExperienceModel(0, "", "", 0, Intensity.Low, 20, Status.Active),
                new ExperienceModel(1, "", "", 0, Intensity.Medium, 20, Status.Archived),
                new ExperienceModel(2, "", "", 0, Intensity.High, 20, Status.Active),
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
            ExperienceModel experience = new ExperienceModel("", "", 0, Intensity.Low, 0, Status.Active);
            Assert.IsFalse(experiencesLogic.Validate(experience));
            ExperienceModel experience2 = new ExperienceModel(null, null, 0, Intensity.Low, 0, Status.Active);
            Assert.IsFalse(experiencesLogic.Validate(experience2));
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
            ExperienceModel experience = new ExperienceModel("test1", "test1", 0, (Intensity)909, 10, Status.Active);
            Assert.IsFalse(experiencesLogic.Validate(experience));
            ExperienceModel experience2 = new ExperienceModel("test1", "test1", 0, (Intensity)1002, 10, Status.Active);
            Assert.IsFalse(experiencesLogic.Validate(experience2));
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
            ExperienceModel experience = new ExperienceModel("test1", "test1", 0, Intensity.Low, -10, Status.Active);
            Assert.IsFalse(experiencesLogic.Validate(experience));
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
            ExperienceModel experience = new ExperienceModel("test1", "test1", 0, Intensity.High, 10, Status.Active);
            Assert.IsTrue(experiencesLogic.Validate(experience));
        }

        [TestMethod]
        public void Incorrect_Experience_Null()
        {
            Assert.IsFalse(experiencesLogic.Validate(null));
        }

        // Edit -------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Experience_Edit_Success()
        {
            ExperienceModel experience = new ExperienceModel(0, "test1", "test1", 0, Intensity.High, 10, Status.Active);
            experiencesLogic.Edit(experience);
            Assert.AreEqual(experience, experiencesLogic.GetById(0));
        }

        [TestMethod]
        public void Incorrect_Experience_Edit_With_Invalid_Experience_Name()
        {
            ExperienceModel experience = new ExperienceModel(0, "", "test1", 0, Intensity.High, 10, Status.Active);
            experiencesLogic.Edit(experience);
            Assert.AreNotEqual(experience, experiencesLogic.GetById(0));
        }

        [TestMethod]
        public void Incorrect_Experience_Edit_With_Invalid_Experience_Description()
        {
            ExperienceModel experience = new ExperienceModel(0, "test1", "", 0, Intensity.High, 10, Status.Active);
            experiencesLogic.Edit(experience);
            Assert.AreNotEqual(experience, experiencesLogic.GetById(0));
        }

        [TestMethod]
        public void Incorrect_Experience_Edit_With_Invalid_Experience_Intensity()
        {
            ExperienceModel experience = new ExperienceModel(0, "test1", "test1", 0, (Intensity)909, 10, Status.Active);
            experiencesLogic.Edit(experience);
            Assert.AreNotEqual(experience, experiencesLogic.GetById(0));
        }

        [TestMethod]
        public void Incorrect_Experience_Edit_With_Invalid_Experience_TimeLength()
        {
            ExperienceModel experience = new ExperienceModel(0, "test1", "test1", 0, Intensity.High, -10, Status.Active);
            experiencesLogic.Edit(experience);
            Assert.AreNotEqual(experience, experiencesLogic.GetById(0));
        }

        // Archive ----------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Experience_Archive_Success()
        {
            experiencesLogic.Archive(0);
            Assert.AreEqual(Status.Archived, experiencesLogic.GetById(0).Status);
        }

        [TestMethod]
        public void Correct_Experience_AlreadyArchived_Still_Archived()
        {
            experiencesLogic.Archive(1);
            Assert.AreEqual(Status.Archived, experiencesLogic.GetById(1).Status);
        }

        [TestMethod]
        public void Correct_Multiple_Experience_Archive_Success()
        {
            experiencesLogic.Archive(0);
            experiencesLogic.Archive(2);
            Assert.AreEqual(Status.Archived, experiencesLogic.GetById(0).Status);
            Assert.AreEqual(Status.Archived, experiencesLogic.GetById(2).Status);
        }

        [TestMethod]
        public void Incorrect_Experience_Archive_Nonexistent_ID()
        {
            int nonexistentID = 999;
            try
            {
                experiencesLogic.Archive(nonexistentID);
            }
            catch
            {
                Assert.Fail("Experience bestaat niet, kan daarom ook niet gearchiveerd worden");
            }
        }
    }
}