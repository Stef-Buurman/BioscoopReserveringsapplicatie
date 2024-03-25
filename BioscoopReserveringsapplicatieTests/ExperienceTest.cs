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
            ExperiencesModel experience = new ExperiencesModel("", 0, 0, 0);
            Assert.IsFalse(_experiencesLogic.ValidateExperience(experience));
            ExperiencesModel experience2 = new ExperiencesModel(null, 0, 0, 0);
            Assert.IsFalse(_experiencesLogic.ValidateExperience(experience2));
        }

        //[TestMethod]
        //public void IncorrectExperienceIntensity()
        //{
        //}

        [TestMethod]
        public void IncorrectExperienceTimeLength()
        {
            ExperiencesModel experience = new ExperiencesModel("test1", 0, 0, -10);
            Assert.IsFalse(_experiencesLogic.ValidateExperience(experience));
        }

        [TestMethod]
        public void CorrectExperience()
        {
            ExperiencesModel experience = new ExperiencesModel("test1", 0, 0, 10);
            Assert.IsTrue(_experiencesLogic.ValidateExperience(experience));
        }
    }
}
