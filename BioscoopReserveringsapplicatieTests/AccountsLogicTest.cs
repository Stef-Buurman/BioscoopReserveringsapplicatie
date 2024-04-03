//namespace BioscoopReserveringsapplicatieTests
//{
//    [TestClass]
//    public class AccountsLogicTest
//    {
//        private UserLogic _accountsLogic = new();

//        [DataRow("test@gmail.nl")]
//        [DataRow("ditIsNogEenEmailAdress@hotmail.nl")]
//        [DataTestMethod]
//        public void CorrectEmail(string email)
//        {
//            Assert.IsTrue(this._accountsLogic.ValidateEmail(email));
//        }

//        [DataRow("testGmail.nl")]
//        [DataRow("WaaromEenApestaartje?")]
//        [DataRow("")]
//        [DataTestMethod]
//        public void IncorrectEmail(string email)
//        {
//            Assert.IsFalse(this._accountsLogic.ValidateEmail(email));
//        }


//        [DataRow(AgeCategory.AGE_6)]
//        [DataRow(AgeCategory.AGE_9)]
//        [DataRow(AgeCategory.AGE_12)]
//        [DataRow(AgeCategory.AGE_14)]
//        [DataRow(AgeCategory.AGE_16)]
//        [DataRow(AgeCategory.AGE_18)]
//        [DataRow(AgeCategory.ALL)]
//        [DataTestMethod]
//        public void CorrectAgeCategory(AgeCategory age)
//        {
//            Assert.IsTrue(this._accountsLogic.ValidateAgeCategory(age));
//        }

//        [DataRow((AgeCategory)909)]
//        [DataRow((AgeCategory)1002)]
//        [DataTestMethod]
//        public void IncorrectAgeCategory(AgeCategory age)
//        {
//            Assert.IsFalse(this._accountsLogic.ValidateAgeCategory(age));
//        }

//        [DataRow(Intensity.Low)]
//        [DataRow(Intensity.Medium)]
//        [DataRow(Intensity.High)]
//        [DataTestMethod]
//        public void CorrectIntensity(Intensity value)
//        {
//            Assert.IsTrue(this._accountsLogic.ValidateIntensity(value));
//        }

//        [DataRow((Intensity)909)]
//        [DataRow((Intensity)1002)]
//        [DataTestMethod]
//        public void IncorrectIntensity(Intensity value)
//        {
//            Assert.IsFalse(this._accountsLogic.ValidateIntensity(value));
//        }

//        [DataRow(Language.Nederlands)]
//        [DataRow(Language.English)]
//        [DataTestMethod]
//        public void CorrectLanguage(Language value)
//        {
//            Assert.IsTrue(this._accountsLogic.ValidateLanguage(value));
//        }

//        [DataRow((Language)909)]
//        [DataRow((Language)1002)]
//        [DataTestMethod]
//        public void IncorrectLanguage(Language value)
//        {
//            Assert.IsFalse(this._accountsLogic.ValidateLanguage(value));
//        }
//    }
//}