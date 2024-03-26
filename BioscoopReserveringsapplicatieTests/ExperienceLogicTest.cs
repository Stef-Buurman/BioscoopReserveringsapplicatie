namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class ExperienceTest
    {
        private ExperiencesLogic _experiencesLogic = new ExperiencesLogic();

        [TestMethod]
        public void IncorrectExperienceNull()
        {
            Assert.IsFalse(_experiencesLogic.ValidateExperience(null));
        }


        [TestMethod]
        public void IncorrectExperienceName()
        {
            ExperiencesModel experience = new ExperiencesModel("", 0, "", 0);
            Assert.IsFalse(_experiencesLogic.ValidateExperience(experience));
            ExperiencesModel experience2 = new ExperiencesModel(null, 0, "", 0);
            Assert.IsFalse(_experiencesLogic.ValidateExperience(experience2));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void IncorrectExperienceName(string name)
        {
            Assert.IsFalse(_experiencesLogic.ValidateExperienceName(name));
        }

        [DataRow("Test")]
        [DataRow("CrazyName")]
        [DataTestMethod]
        public void CorrectExperienceName(string name)
        {
            Assert.IsTrue(_experiencesLogic.ValidateExperienceName(name));
        }

        [TestMethod]
        public void IncorrectExperienceIntensity()
        {
            ExperiencesModel experience = new ExperiencesModel("test1", 0, "", 0);
            Assert.IsFalse(_experiencesLogic.ValidateExperience(experience));
            ExperiencesModel experience2 = new ExperiencesModel("test1", 0, "", 11);
            Assert.IsFalse(_experiencesLogic.ValidateExperience(experience2));
        }

        [DataRow("lager")]
        [DataRow("laagst")]
        [DataRow("hoger")]
        [DataRow("hoogst")]
        [DataTestMethod]
        public void IncorrectExperienceIntensitI(string intensity)
        {
            Assert.IsFalse(_experiencesLogic.ValidateExperienceIntensity(intensity));
        }

        [DataRow("laag")]
        [DataRow("medium")]
        [DataRow("hoog")]
        [DataRow("Laag")]
        [DataRow("Medium")]
        [DataRow("Hoog")]
        [DataRow("lAaG")]
        [DataRow("mEdIuM")]
        [DataRow("HoOg")]
        [DataTestMethod]
        public void CorrectExperienceIntensity(string intensity)
        {
            Assert.IsTrue(_experiencesLogic.ValidateExperienceIntensity(intensity));
        }


        [TestMethod]
        public void IncorrectExperienceTimeLength()
        {
            ExperiencesModel experience = new ExperiencesModel("test1", 0, "", -10);
            Assert.IsFalse(_experiencesLogic.ValidateExperience(experience));
        }

        [DataRow(-6)]
        [DataRow(-5)]
        [DataRow(-4)]
        [DataRow(-3)]
        [DataRow(-2)]
        [DataRow(-1)]
        [DataTestMethod]
        public void IncorrectExperienceTimeLength(int time)
        {
            Assert.IsFalse(_experiencesLogic.ValidateExperienceTimeLength(time));
        }

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
        public void CorrectExperience()
        {
            ExperiencesModel experience = new ExperiencesModel("test1", 0, "laag", 10);
            Assert.IsTrue(_experiencesLogic.ValidateExperience(experience));
        }
    }
}
