namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class AccountsLogicTest
    {
        private UserLogic _accountsLogic = new();

        [DataRow("test@gmail.nl")]
        [DataRow("ditIsNogEenEmailAdress@hotmail.nl")]
        [DataTestMethod]
        public void CorrectEmail(string email)
        {
            Assert.IsTrue(this._accountsLogic.ValidateEmail(email));
        }

        [DataRow("testGmail.nl")]
        [DataRow("WaaromEenApestaartje?")]
        [DataRow("")]
        [DataTestMethod]
        public void IncorrectEmail(string email)
        {
            Assert.IsFalse(this._accountsLogic.ValidateEmail(email));
        }


        [DataRow(6)]
        [DataRow(9)]
        [DataRow(12)]
        [DataRow(14)]
        [DataRow(16)]
        [DataRow(18)]
        [DataTestMethod]
        public void CorrectAgeCategory(int age)
        {
            Assert.IsTrue(this._accountsLogic.ValidateAgeCategory(age));
        }

        [DataRow(-18)]
        [DataRow(-16)]
        [DataRow(-14)]
        [DataRow(-12)]
        [DataRow(-9)]
        [DataRow(-6)]
        [DataRow(-3)]
        [DataRow(-2)]
        [DataRow(-1)]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(2)]
        [DataRow(3)]
        [DataRow(4)]
        [DataRow(5)]
        [DataRow(7)]
        [DataRow(8)]
        [DataRow(10)]
        [DataRow(11)]
        [DataRow(13)]
        [DataRow(15)]
        [DataRow(17)]
        [DataRow(19)]
        [DataRow(20)]
        [DataTestMethod]
        public void IncorrectAgeCategory(int age)
        {
            Assert.IsFalse(this._accountsLogic.ValidateAgeCategory(age));
        }

        [DataRow("Laag")]
        [DataRow("Medium")]
        [DataRow("Hoog")]
        [DataRow("laag")]
        [DataRow("medium")]
        [DataRow("hoog")]
        [DataRow("laaG")]
        [DataRow("mediuM")]
        [DataRow("hooG")]
        [DataTestMethod]
        public void CorrectIntensity(string value)
        {
            Assert.IsTrue(this._accountsLogic.ValidateIntensity(value));
        }

        [DataRow("Extra Laag")]
        [DataRow("Extra Medium")]
        [DataRow("Extra Hoog")]
        [DataRow("extra laag")]
        [DataRow("extra medium")]
        [DataRow("extra hoog")]
        [DataRow("eXtRa laaG")]
        [DataRow("eXtRa mediuM")]
        [DataRow("eXtRa hooG")]
        [DataRow("")]
        [DataTestMethod]
        public void IncorrectIntensity(string value)
        {
            Assert.IsFalse(this._accountsLogic.ValidateIntensity(value));
        }

        [DataRow("English")]
        [DataRow("Nederlands")]
        [DataRow("english")]
        [DataRow("nederlands")]
        [DataRow("eNgLiSh")]
        [DataRow("NeDeRlAnDs")]
        [DataTestMethod]
        public void CorrectLanguage(string value)
        {
            Assert.IsTrue(this._accountsLogic.ValidateLanguage(value));
        }

        [DataRow("andereTaaal")]
        [DataRow("Vlaams")]
        [DataRow("Fries")]
        [DataRow("test")]
        [DataRow("")]
        [DataTestMethod]
        public void IncorrectLanguage(string value)
        {
            Assert.IsFalse(this._accountsLogic.ValidateLanguage(value));
        }
    }
}