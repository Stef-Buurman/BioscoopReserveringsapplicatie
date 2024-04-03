namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class ExperienceTest
    {
        private ExperiencesLogic _experiencesLogic = new ExperiencesLogic();

        // Name ------------------------------------------------------------------------------------------------------------------

        [DataRow("Test")]
        [DataRow("CrazyName")]
        [DataTestMethod]
        public void Correct_Experience_Name_Validation(string name)
        {
            Assert.IsTrue(_experiencesLogic.ValidateExperienceName(name));
        }

        [TestMethod]
        public void Incorrect_Experience_Name_Validation_With_Experience()
        {
            ExperiencesModel experience = new ExperiencesModel("", 0, Intensity.Low, 0);
            Assert.IsFalse(_experiencesLogic.ValidateExperience(experience));
            ExperiencesModel experience2 = new ExperiencesModel(null, 0, Intensity.Low, 0);
            Assert.IsFalse(_experiencesLogic.ValidateExperience(experience2));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Experience_Name_Validation_With_Name(string name)
        {
            Assert.IsFalse(_experiencesLogic.ValidateExperienceName(name));
        }

        [TestMethod]
        public void Incorrect_Experience_Intensity_With_Experience()
        {
            ExperiencesModel experience = new ExperiencesModel("test1", 0, (Intensity)909, 10);
            Assert.IsFalse(_experiencesLogic.ValidateExperience(experience));
            ExperiencesModel experience2 = new ExperiencesModel("test1", 0, (Intensity)1002, 10);
            Assert.IsFalse(_experiencesLogic.ValidateExperience(experience2));
        }

        // Intensity ------------------------------------------------------------------------------------------------------------------

        [DataRow(Intensity.Low)]
        [DataRow(Intensity.Medium)]
        [DataRow(Intensity.High)]
        [DataTestMethod]
        public void Correct_Experience_Intensity_Validation_With_Intensity(Intensity intensity)
        {
            Assert.IsTrue(_experiencesLogic.ValidateExperienceIntensity(intensity));
        }

        [DataRow((Intensity)909)]
        [DataRow((Intensity)1002)]
        [DataTestMethod]
        public void Incorrect_Experience_Intensity_Validation_With_Intensity(Intensity intensity)
        {
            Assert.IsFalse(_experiencesLogic.ValidateExperienceIntensity(intensity));
        }

        // TimeLength ------------------------------------------------------------------------------------------------------------------

        [DataRow(0)]
        [DataRow(1)]
        [DataRow(10)]
        [DataRow(20)]
        [DataTestMethod]
        public void CorrectExperienceTimeLength(int time)
        {
            Assert.IsTrue(_experiencesLogic.ValidateExperienceTimeLength(time));
        }

        [TestMethod]
        public void Incorrect_Experience_TimeLength_Validation_With_Experience()
        {
            ExperiencesModel experience = new ExperiencesModel("test1", 0, Intensity.Low, -10);
            Assert.IsFalse(_experiencesLogic.ValidateExperience(experience));
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
            Assert.IsFalse(_experiencesLogic.ValidateExperienceTimeLength(time));
        }

        // Experience ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CorrectExperience()
        {
            ExperiencesModel experience = new ExperiencesModel("test1", 0, Intensity.High, 10);
            Assert.IsTrue(_experiencesLogic.ValidateExperience(experience));
        }

        [TestMethod]
        public void Incorrect_Experience_Null()
        {
            Assert.IsFalse(_experiencesLogic.ValidateExperience(null));
        }
    }
}
