using BioscoopReserveringsapplicatie;
using NSubstitute;

namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class UserManage
    {
        //private UserLogic userLogic;
        //[TestMethod]
        public UserLogic Initialize()
        {
            var userRepositoryMock = Substitute.For<IDataAccess<UserModel>>();
            List<UserModel> users = new List<UserModel>() {
                new UserModel(1, false, false, "Henk@henk.henk", "testtest", "henk", null, default, default, default),
                new UserModel(2, false, true, "Gerda@Gerda.Gerda", "testtest", "Gerda", null, default, default, default),
                new UserModel(3, true, false, "Petra@Petra.Petra", "testtest", "Petra",new List<Genre>() { Genre.Horror, Genre.Mystery, Genre.Family }, AgeCategory.AGE_9, Intensity.Low, Language.English),
                new UserModel(4, false, false, "Nick@Nick.Nick", "NickPassword", "Nick", null, default, default, default),
                new UserModel(5, true, true, "Pieter@Pieter.Pieter", "PieterPassword", "Pieter", new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery }, AgeCategory.AGE_6, Intensity.High, Language.Nederlands),
                new UserModel(6, false, false, "Tim@Tim.Tim", "TimPassword", "Tim", null, default, default, default),
                new UserModel(1, false, false, "Stef@Stef.Stef", "StefPassword", "Stef", new List<Genre>() { Genre.Horror, Genre.Western, Genre.Romance }, AgeCategory.AGE_18, Intensity.Medium, Language.English),
                new UserModel(1, false, false, "Menno@Menno.Menno", "MennoPassword", "Menno", null, default, default, default),
            };
            userRepositoryMock.LoadAll().Returns(users);
            userRepositoryMock.WriteAll(Arg.Any<List<UserModel>>());

            UserAccess.NewDataAccess(userRepositoryMock);
            return new UserLogic();
        }

        [TestMethod]
        public void GetUserNameById_Returns_UserName1()
        {
            UserLogic userLogic = Initialize();

            var x = userLogic.LoginUser("Henk@henk.henk", "testtest");
            userLogic.addPreferencesToAccount(new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery }, AgeCategory.AGE_6, Intensity.Low, Language.Nederlands);
            UserModel userName = userLogic.GetById(1);

            Assert.IsNotNull(userName);
            Assert.IsTrue(x);
            Assert.AreEqual("henk", userName.FullName);
            Assert.AreEqual(AgeCategory.AGE_6, userName.AgeCategory);
            Assert.AreEqual(Intensity.Low, userName.Intensity);
            Assert.AreEqual(Language.Nederlands, userName.Language);
        }

        [TestMethod]
        public void Correct_GetUser_By_Id()
        {
            UserLogic userLogic = Initialize();
            UserModel userName = userLogic.GetById(1);
            Assert.IsNotNull(userName);
        }

        [TestMethod]
        public void Incorrect_GetUser_By_Id()
        {
            UserLogic userLogic = Initialize();
            UserModel userName = userLogic.GetById(999);
            Assert.IsNull(userName);
        }

        // -------------------------------------------------------------------------------------------------------------------------------
        // AddPreferences ----------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------------------
        
        public UserModel Initialize_Preferences_For_User()
        {
            UserLogic userLogic = Initialize();

            var x = userLogic.LoginUser("Henk@henk.henk", "testtest");
            Assert.IsTrue(x);
            userLogic.addPreferencesToAccount(new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery, Genre.Crime, Genre.Animation }, AgeCategory.AGE_6, Intensity.Low, Language.Nederlands);
            return userLogic.GetById(1);
        }

        [TestMethod]
        public void Correct_GetUser_By_Id1()
        {
            UserModel userName = Initialize_Preferences_For_User();

            Assert.IsNotNull(userName);
            Assert.AreEqual("henk", userName.FullName);
            Assert.AreEqual(AgeCategory.AGE_6, userName.AgeCategory);
            Assert.AreEqual(Intensity.Low, userName.Intensity);
            Assert.AreEqual(Language.Nederlands, userName.Language);
            Assert.IsTrue(userName.Genres.Contains(Genre.Animation));
            Assert.IsTrue(userName.Genres.Contains(Genre.Adventure));
            Assert.IsTrue(userName.Genres.Contains(Genre.Drama));
            Assert.IsTrue(userName.Genres.Contains(Genre.Mystery));
            Assert.IsTrue(userName.Genres.Contains(Genre.Crime));
        }

        // AgeCategory ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_AgeCatagory_After_Preferences_Added()
        {
            UserModel userName = Initialize_Preferences_For_User();
            Assert.AreEqual(AgeCategory.AGE_6, userName.AgeCategory);
        }

        [TestMethod]
        public void Incorrect_AgeCatagory_After_Preferences_Added()
        {
            UserModel userName = Initialize_Preferences_For_User();
            Assert.AreNotEqual(AgeCategory.Undefined, userName.AgeCategory);
        }

        // Intensity ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Intensity_After_Preferences_Added()
        {
            UserModel userName = Initialize_Preferences_For_User();
            Assert.AreEqual(Intensity.Low, userName.Intensity);
        }

        [TestMethod]
        public void Incorrect_Intensity_After_Preferences_Added()
        {
            UserModel userName = Initialize_Preferences_For_User();
            Assert.AreNotEqual(Intensity.Undefined, userName.Intensity);
        }

        // -------------------------------------------------------------------------------------------------------------------------------
        // User login --------------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------------------

        // Correct User Login ------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_LoginUser_Validation_NoAdmin()
        {
            UserLogic userLogic = Initialize();
            Assert.IsTrue(userLogic.LoginUser("Henk@henk.henk", "testtest"));
        }

        [TestMethod]
        public void Correct_LoginUser_Validation_Admin()
        {
            UserLogic userLogic = Initialize();
            Assert.IsTrue(userLogic.LoginUser("Petra@Petra.Petra", "testtest"));
        }

        // Incorrect NoAdmin Login -------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Incorrect_LoginUser_Validation_NoAdmin_WrongPassword()
        {
            UserLogic userLogic = Initialize();
            Assert.IsFalse(userLogic.LoginUser("Tim@Tim.Tim", "testtest"));
        }

        [TestMethod]
        public void Incorrect_LoginUser_Validation_NoAdmin_WrongEmail()
        {
            UserLogic userLogic = Initialize();
            Assert.IsFalse(userLogic.LoginUser("DitIsGewoonEenHeelAnderEmail@Adress.nl", "testtest"));
        }

        // Incorrect Admin Login ---------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Incorrect_LoginUser_Validation_Admin_WrongPassword()
        {
            UserLogic userLogic = Initialize();
            Assert.IsFalse(userLogic.LoginUser("Pieter@Pieter.Pieter", "lalalalalalalalalala"));
        }

        [TestMethod]
        public void Incorrect_LoginUser_Validation_Admin_WrongEmail()
        {
            UserLogic userLogic = Initialize();
            Assert.IsFalse(userLogic.LoginUser("DitIsGewoonEenHeelAnderEmail@Adress.nl", "PieterPassword"));
        }

        // -------------------------------------------------------------------------------------------------------------------------------
        // User validations --------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------------------

        // Email ------------------------------------------------------------------------------------------------------------------------

        [DataRow("test@gmail.nl")]
        [DataRow("ditIsNogEenEmailAdress@hotmail.nl")]
        [DataTestMethod]
        public void Correct_Email_Validation(string email)
        {
            UserLogic userLogic = Initialize();
            Assert.IsTrue(userLogic.ValidateEmail(email));
        }

        [DataRow("testGmail.nl")]
        [DataRow("WaaromEenApestaartje?")]
        [DataRow("")]
        [DataTestMethod]
        public void Incorrect_Email_Validation(string email)
        {
            UserLogic userLogic = Initialize();
            Assert.IsFalse(userLogic.ValidateEmail(email));
        }

        // AgeCategory ------------------------------------------------------------------------------------------------------------------

        [DataRow(AgeCategory.AGE_6)]
        [DataRow(AgeCategory.AGE_9)]
        [DataRow(AgeCategory.AGE_12)]
        [DataRow(AgeCategory.AGE_14)]
        [DataRow(AgeCategory.AGE_16)]
        [DataRow(AgeCategory.AGE_18)]
        [DataRow(AgeCategory.ALL)]
        [DataTestMethod]
        public void Correct_AgeCategory_Validation(AgeCategory age)
        {
            UserLogic userLogic = Initialize();
            Assert.IsTrue(userLogic.ValidateAgeCategory(age));
        }

        [DataRow((AgeCategory)909)]
        [DataRow((AgeCategory)1002)]
        [DataTestMethod]
        public void Incorrect_AgeCategory_Validation(AgeCategory age)
        {
            UserLogic userLogic = Initialize();
            Assert.IsFalse(userLogic.ValidateAgeCategory(age));
        }

        // Intensity ------------------------------------------------------------------------------------------------------------------

        [DataRow(Intensity.Low)]
        [DataRow(Intensity.Medium)]
        [DataRow(Intensity.High)]
        [DataTestMethod]
        public void Correct_Intensity_Validation(Intensity value)
        {
            UserLogic userLogic = Initialize();
            Assert.IsTrue(userLogic.ValidateIntensity(value));
        }

        [DataRow((Intensity)909)]
        [DataRow((Intensity)1002)]
        [DataTestMethod]
        public void Incorrect_Intensity_Validation(Intensity value)
        {
            UserLogic userLogic = Initialize();
            Assert.IsFalse(userLogic.ValidateIntensity(value));
        }

        // Language ------------------------------------------------------------------------------------------------------------------

        [DataRow(Language.Nederlands)]
        [DataRow(Language.English)]
        [DataTestMethod]
        public void Correct_Language_Validation(Language value)
        {
            UserLogic userLogic = Initialize();
            Assert.IsTrue(userLogic.ValidateLanguage(value));
        }

        [DataRow((Language)909)]
        [DataRow((Language)1002)]
        [DataTestMethod]
        public void Incorrect_Language_Validation(Language value)
        {
            UserLogic userLogic = Initialize();
            Assert.IsFalse(userLogic.ValidateLanguage(value));
        }
    }
}