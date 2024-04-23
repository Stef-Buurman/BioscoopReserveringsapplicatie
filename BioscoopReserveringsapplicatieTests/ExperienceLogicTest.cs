﻿using NSubstitute;

namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class ExperienceTest
    {
        public ExperiencesLogic Initialize()
        {
            var experienceRepositoryMock = Substitute.For<IDataAccess<ExperienceModel>>();
            List<ExperienceModel> experiences = new List<ExperienceModel>() {
                new ExperienceModel(0, "", 0, Intensity.Low, 20, false),
                new ExperienceModel(1, "", 0, Intensity.Medium, 20, true),
                new ExperienceModel(2, "", 0, Intensity.High, 20, false),
            };
            experienceRepositoryMock.LoadAll().Returns(experiences);
            experienceRepositoryMock.WriteAll(Arg.Any<List<ExperienceModel>>());

            return new ExperiencesLogic(experienceRepositoryMock);
        }

        // Name ------------------------------------------------------------------------------------------------------------------

        [DataRow("Test")]
        [DataRow("CrazyName")]
        [DataTestMethod]
        public void Correct_Experience_Name_Validation(string name)
        {
            ExperiencesLogic experiencesLogic = Initialize();
            Assert.IsTrue(experiencesLogic.ValidateExperienceName(name));
        }

        [TestMethod]
        public void Incorrect_Experience_Name_Validation_With_Experience()
        {
            ExperiencesLogic experiencesLogic = Initialize();
            ExperienceModel experience = new ExperienceModel("", 0, Intensity.Low, 0, false);
            Assert.IsFalse(experiencesLogic.ValidateExperience(experience));
            ExperienceModel experience2 = new ExperienceModel(null, 0, Intensity.Low, 0, false);
            Assert.IsFalse(experiencesLogic.ValidateExperience(experience2));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Experience_Name_Validation_With_Name(string name)
        {
            ExperiencesLogic experiencesLogic = Initialize();
            Assert.IsFalse(experiencesLogic.ValidateExperienceName(name));
        }

        [TestMethod]
        public void Incorrect_Experience_Intensity_With_Experience()
        {
            ExperiencesLogic experiencesLogic = Initialize();
            ExperienceModel experience = new ExperienceModel("test1", 0, (Intensity)909, 10, false);
            Assert.IsFalse(experiencesLogic.ValidateExperience(experience));
            ExperienceModel experience2 = new ExperienceModel("test1", 0, (Intensity)1002, 10, false);
            Assert.IsFalse(experiencesLogic.ValidateExperience(experience2));
        }

        // Intensity ------------------------------------------------------------------------------------------------------------------

        [DataRow(Intensity.Low)]
        [DataRow(Intensity.Medium)]
        [DataRow(Intensity.High)]
        [DataTestMethod]
        public void Correct_Experience_Intensity_Validation_With_Intensity(Intensity intensity)
        {
            ExperiencesLogic experiencesLogic = Initialize();
            Assert.IsTrue(experiencesLogic.ValidateExperienceIntensity(intensity));
        }

        [DataRow((Intensity)909)]
        [DataRow((Intensity)1002)]
        [DataTestMethod]
        public void Incorrect_Experience_Intensity_Validation_With_Intensity(Intensity intensity)
        {
            ExperiencesLogic experiencesLogic = Initialize();
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
            ExperiencesLogic experiencesLogic = Initialize();
            Assert.IsTrue(experiencesLogic.ValidateExperienceTimeLength(time));
        }

        [TestMethod]
        public void Incorrect_Experience_TimeLength_Validation_With_Experience()
        {
            ExperiencesLogic experiencesLogic = Initialize();
            ExperienceModel experience = new ExperienceModel("test1", 0, Intensity.Low, -10, false);
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
            ExperiencesLogic experiencesLogic = Initialize();
            Assert.IsFalse(experiencesLogic.ValidateExperienceTimeLength(time));
        }

        // Experience ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void CorrectExperience()
        {
            ExperiencesLogic experiencesLogic = Initialize();
            ExperienceModel experience = new ExperienceModel("test1", 0, Intensity.High, 10, false);
            Assert.IsTrue(experiencesLogic.ValidateExperience(experience));
        }

        [TestMethod]
        public void Incorrect_Experience_Null()
        {
            ExperiencesLogic experiencesLogic = Initialize();
            Assert.IsFalse(experiencesLogic.ValidateExperience(null));
        }
        
        // Archive ----------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Experience_Archive_Success()
        {
            ExperiencesLogic experiencesLogic = Initialize();
            experiencesLogic.ArchiveExperience(0); 
            Assert.IsTrue(experiencesLogic.GetById(0).Archived);
        }

        [TestMethod]
        public void Correct_Experience_AlreadyArchived_Still_Archived()
        {
            ExperiencesLogic experiencesLogic = Initialize();
            experiencesLogic.ArchiveExperience(1);
            Assert.IsTrue(experiencesLogic.GetById(1).Archived);
        }

        [TestMethod]
        public void Correct_Multiple_Experience_Archive_Success()
        {
            ExperiencesLogic experiencesLogic = Initialize();
            experiencesLogic.ArchiveExperience(0);
            experiencesLogic.ArchiveExperience(2);
            Assert.IsTrue(experiencesLogic.GetById(0).Archived);
            Assert.IsTrue(experiencesLogic.GetById(2).Archived);
        }

        [TestMethod]
        public void Incorrect_Experience_Archive_Nonexistent_ID()
        {
            ExperiencesLogic experiencesLogic = Initialize();
            int nonexistentID = 999;
            experiencesLogic.ArchiveExperience(nonexistentID);
            Assert.IsNull(experiencesLogic.GetById(nonexistentID));
}
    }
}