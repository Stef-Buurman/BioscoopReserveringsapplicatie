using NSubstitute;

namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class UserLogicTest
    {
        UserLogic userLogic;
        public static Dictionary<string, (string hashedPassword, byte[] salt)> passwordData;

        [ClassInitialize]
        public static void ClassInitialize(TestContext context)
        {
            passwordData = new Dictionary<string, (string hashedPassword, byte[] salt)>
            {
                { "testtest", (PasswordHasher.HashPassword("testtest", out byte[] salt2), salt2) },
                { "NickPassword", (PasswordHasher.HashPassword("NickPassword", out byte[] salt3), salt3) },
                { "PieterPassword", (PasswordHasher.HashPassword("PieterPassword", out byte[] salt4), salt4) },
                { "TimPassword", (PasswordHasher.HashPassword("TimPassword", out byte[] salt5), salt5) },
                { "StefPassword", (PasswordHasher.HashPassword("StefPassword", out byte[] salt6), salt6) },
                { "MennoPassword", (PasswordHasher.HashPassword("MennoPassword", out byte[] salt7), salt7) }
            };
        }

        [TestInitialize]
        public void Initialize()
        {
            var userRepositoryMock = Substitute.For<IDataAccess<UserModel>>();
            List<UserModel> users = new List<UserModel>() {
                new UserModel(1, false, "Henk@henk.henk", passwordData["testtest"].hashedPassword, passwordData["testtest"].salt, "henk", null, default, default, default, null),
                new UserModel(2, false, "Gerda@Gerda.Gerda", passwordData["testtest"].hashedPassword, passwordData["testtest"].salt, "Gerda", null, default, default, default, null),
                new UserModel(3, true, "Petra@Petra.Petra", passwordData["testtest"].hashedPassword, passwordData["testtest"].salt, "Petra", new List<Genre>() { Genre.Horror, Genre.Mystery, Genre.Family }, AgeCategory.AGE_9, Intensity.Low, Language.English, null),
                new UserModel(4, false, "Nick@Nick.Nick", passwordData["NickPassword"].hashedPassword, passwordData["NickPassword"].salt, "Nick", null, default, default, default, null),
                new UserModel(5, true, "Pieter@Pieter.Pieter", passwordData["PieterPassword"].hashedPassword, passwordData["PieterPassword"].salt, "Pieter", new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery }, AgeCategory.AGE_6, Intensity.High, Language.Nederlands, null),
                new UserModel(6, false, "Tim@Tim.Tim", passwordData["TimPassword"].hashedPassword, passwordData["TimPassword"].salt, "Tim", null, default, default, default, null),
                new UserModel(1, false, "Stef@Stef.Stef", passwordData["StefPassword"].hashedPassword, passwordData["StefPassword"].salt, "Stef", new List<Genre>() { Genre.Horror, Genre.Western, Genre.Romance }, AgeCategory.AGE_18, Intensity.Medium, Language.English, null),
                new UserModel(1, false, "Menno@Menno.Menno", passwordData["MennoPassword"].hashedPassword, passwordData["MennoPassword"].salt, "Menno", null, default, default, default, null),
            };
            userRepositoryMock.LoadAll().Returns(users);
            userRepositoryMock.WriteAll(Arg.Any<List<UserModel>>());

            userLogic = new UserLogic(userRepositoryMock);
        }

        // -------------------------------------------------------------------------------------------------------------------------------
        // GetUser -----------------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_GetUser_By_Id()
        {
            UserModel userName = userLogic.GetById(1);
            Assert.IsNotNull(userName);
        }

        [TestMethod]
        public void Incorrect_GetUser_By_Id()
        {
            UserModel userName = userLogic.GetById(999);
            Assert.IsNull(userName);
        }

        // -------------------------------------------------------------------------------------------------------------------------------
        // RegisterNewUser ---------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------------------

        private string CorrectUserName = "Henk";
        private string CorrectEmail = "Henk@Gerda.nl";
        private string IncorrectEmailCharsCount = "@.";
        private string IncorrectEmailNoAtSign = "HenkGerda.nl";
        private string IncorrectEmailNoDot = "Henk@Gerdanl";
        private string CorrectPassword = "DitIsEenHeelMooiWachtWoord";
        private string IncorrectPasswordCharsCount = "Henk";

        // Name ---------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Incorrect_Register_New_User_Name_Is_Empty()
        {
            Result<UserModel> results = userLogic.RegisterNewUser("", this.CorrectEmail, this.CorrectPassword, this.CorrectPassword);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.ErrorMessage.Contains(RegisterNewUserErrorMessages.NameEmpty));
        }

        // Email --------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Incorrect_Register_New_User_Email_Is_Empty()
        {
            Result<UserModel> results = userLogic.RegisterNewUser(this.CorrectUserName, "", this.CorrectPassword, this.CorrectPassword);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.ErrorMessage.Contains(RegisterNewUserErrorMessages.EmailEmpty));
        }

        [TestMethod]
        public void Incorrect_Register_New_User_Email_Not_Enough_Characters()
        {
            Result<UserModel> results = userLogic.RegisterNewUser(this.CorrectUserName, this.IncorrectEmailCharsCount, this.CorrectPassword, this.CorrectPassword);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.ErrorMessage.Contains(RegisterNewUserErrorMessages.EmailAdressIncomplete));
        }

        [TestMethod]
        public void Incorrect_Register_New_User_Email_No_At_Sign()
        {
            Result<UserModel> results = userLogic.RegisterNewUser(this.CorrectUserName, this.IncorrectEmailNoAtSign, this.CorrectPassword, this.CorrectPassword);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.ErrorMessage.Contains(RegisterNewUserErrorMessages.EmailAdressIncomplete));
        }

        [TestMethod]
        public void Incorrect_Register_New_User_Email_No_Dot()
        {
            Result<UserModel> results = userLogic.RegisterNewUser(this.CorrectUserName, this.IncorrectEmailNoDot, this.CorrectPassword, this.CorrectPassword);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.ErrorMessage.Contains(RegisterNewUserErrorMessages.EmailAdressIncomplete));
        }

        // Password -----------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Incorrect_Register_New_User_Password_Is_Empty()
        {
            Result<UserModel> results = userLogic.RegisterNewUser(this.CorrectUserName, this.CorrectEmail, "", "");
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.ErrorMessage.Contains(RegisterNewUserErrorMessages.PasswordMinimumChars));
        }

        [TestMethod]
        public void Incorrect_Register_New_User_Password_Has_Too_Few_Characters()
        {
            Result<UserModel> results = userLogic.RegisterNewUser(this.CorrectUserName, this.CorrectEmail, this.IncorrectPasswordCharsCount, this.IncorrectPasswordCharsCount);
            Assert.IsFalse(results.IsValid);
            Assert.IsTrue(results.ErrorMessage.Contains(RegisterNewUserErrorMessages.PasswordMinimumChars));
        }

        [TestMethod]
        public void Correct_Old_Password()
        {
            userLogic.Login("Henk@henk.henk", "testtest");
            Assert.IsTrue(userLogic.ValidateOldPassword("testtest"));
        }

        [TestMethod]
        public void Incorrect_Old_Password()
        {
            userLogic.Login("Henk@henk.henk", "testtest");
            Assert.IsFalse(userLogic.ValidateOldPassword("NietHenkZijnWachtwoord"));
        }

        [TestMethod]
        public void Correct_Password_Validation()
        {
            Assert.IsTrue(userLogic.ValidatePassword("SuperGoedWachtwoord"));
        }

        [TestMethod]
        public void Incorrect_Length_Password__Too_Short_Validation()
        {
            Assert.IsFalse(userLogic.ValidatePassword("Henk"));
        }

        [TestMethod]
        public void Correct_Password_Change()
        {
            userLogic.Login("Henk@henk.henk", "testtest");
            Assert.IsTrue(userLogic.EditPassword("NieuwWachtwoord"));
        }

        [TestMethod]
        public void Incorrect_Length_Password_Too_Short_Change()
        {
            userLogic.Login("Henk@henk.henk", "testtest");
            Assert.IsFalse(userLogic.EditPassword("1"));
        }

        // -------------------------------------------------------------------------------------------------------------------------------
        // AddPreferences ----------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------------------

        private List<Genre> TestGenres = new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery, Genre.Crime, Genre.Animation };

        public UserModel Initialize_Preferences_For_User()
        {
            var x = userLogic.Login("Henk@henk.henk", "testtest");
            Assert.IsTrue(x);
            userLogic.addPreferencesToAccount(TestGenres, AgeCategory.AGE_6, Intensity.Low, Language.Nederlands);
            return userLogic.GetById(1);
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

        // Language ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Language_After_Preferences_Added()
        {
            UserModel userName = Initialize_Preferences_For_User();
            Assert.AreEqual(Language.Nederlands, userName.Language);
        }

        [TestMethod]
        public void Incorrect_Language_After_Preferences_Added()
        {
            UserModel userName = Initialize_Preferences_For_User();
            Assert.AreNotEqual(Language.Undefined, userName.Language);
        }

        // Genres ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Genres_After_Preferences_Added()
        {
            UserModel userName = Initialize_Preferences_For_User();

            foreach (Genre genre in TestGenres)
            {
                Assert.IsTrue(userName.Genres.Contains(genre));
            }
        }

        [TestMethod]
        public void Incorrect_Genres_After_Preferences_Added()
        {
            UserModel userName = Initialize_Preferences_For_User();
            Assert.IsNotNull(userName.Genres);
        }

        [TestMethod]
        public void Genres_After_Preferences_Added_Has_No_Other_Genres()
        {
            UserModel userName = Initialize_Preferences_For_User();
            foreach (Genre genre in Enum.GetValues(typeof(Genre)))
            {
                if (!TestGenres.Contains(genre)) Assert.IsFalse(userName.Genres.Contains(genre));
            }
        }

        // -------------------------------------------------------------------------------------------------------------------------------
        // User login --------------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------------------

        // Correct User Login ------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Login_Validation_NoAdmin()
        {
            Assert.IsTrue(userLogic.Login("Henk@henk.henk", "testtest"));
        }

        [TestMethod]
        public void Correct_Login_Validation_Admin()
        {
            Assert.IsTrue(userLogic.Login("Petra@Petra.Petra", "testtest"));
        }

        // Incorrect NoAdmin Login -------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Incorrect_Login_Validation_NoAdmin_WrongPassword()
        {
            Assert.IsFalse(userLogic.Login("Tim@Tim.Tim", "testtest"));
        }

        [TestMethod]
        public void Incorrect_Login_Validation_NoAdmin_WrongEmail()
        {
            Assert.IsFalse(userLogic.Login("DitIsGewoonEenHeelAnderEmail@Adress.nl", "testtest"));
        }

        // Incorrect Admin Login ---------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Incorrect_Login_Validation_Admin_WrongPassword()
        {
            Assert.IsFalse(userLogic.Login("Pieter@Pieter.Pieter", "lalalalalalalalalala"));
        }

        [TestMethod]
        public void Incorrect_Login_Validation_Admin_WrongEmail()
        {
            Assert.IsFalse(userLogic.Login("DitIsGewoonEenHeelAnderEmail@Adress.nl", "PieterPassword"));
        }

        // -------------------------------------------------------------------------------------------------------------------------------
        // User edit ---------------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Edit_Validation()
        {
            var x = userLogic.Login("Tim@Tim.Tim", "TimPassword");
            Assert.IsTrue(x);
            Assert.IsTrue(userLogic.Edit("Tim van Eert", "Tim@Tim.Tim", new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery }, Intensity.High, AgeCategory.AGE_6).IsValid);
        }

        [TestMethod]
        public void Incorrect_Name_Edit_validation()
        {
            var x = userLogic.Login("Tim@Tim.Tim", "TimPassword");
            Assert.IsTrue(x);
            Assert.IsFalse(userLogic.Edit("   ", "Tim@Tim.Tim", new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery }, Intensity.High, AgeCategory.AGE_6).IsValid);
        }

        [DataRow("")]
        [DataRow("     ")]
        [DataRow("MailZonder@eenpunt")]
        [DataRow("GeenAppenstaartje.nl")]
        [TestMethod]
        public void Incorrect_Email_Edit_validation(string email)
        {
            var x = userLogic.Login("Tim@Tim.Tim", "TimPassword");
            Assert.IsTrue(x);
            Assert.IsFalse(userLogic.Edit("Tim van Eert", email, new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery }, Intensity.High, AgeCategory.AGE_6).IsValid);
        }

        [TestMethod]
        public void Incorrect_Genre_Edit_validation()
        {
            var x = userLogic.Login("Tim@Tim.Tim", "TimPassword");
            Assert.IsTrue(x);
            Assert.IsFalse(userLogic.Edit("Tim van Eert", "Tim@Tim.Tim", new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery, Genre.Undefined }, Intensity.High, AgeCategory.AGE_6).IsValid);
        }

        // -------------------------------------------------------------------------------------------------------------------------------
        // User validations --------------------------------------------------------------------------------------------------------------
        // -------------------------------------------------------------------------------------------------------------------------------

        // Name --------------------------------------------------------------------------------------------------------------------------

        [DataRow("TestName")]
        [DataRow("Naam met Achternaam")]
        [DataTestMethod]
        public void Correct_Name_Validation(string name)
        {
            Assert.IsTrue(userLogic.ValidateName(name));
        }

        [DataRow("")]
        [DataRow("            ")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Name_Validation(string name)
        {
            Assert.IsFalse(userLogic.ValidateName(name));
        }

        // Email -------------------------------------------------------------------------------------------------------------------------

        [DataRow("test@gmail.nl")]
        [DataRow("ditIsNogEenEmailAdress@hotmail.nl")]
        [DataTestMethod]
        public void Correct_Email_Validation(string email)
        {
            Assert.IsTrue(userLogic.ValidateEmail(email));
        }

        [DataRow("testGmail.nl")]
        [DataRow("WaaromEenApestaartje?")]
        [DataRow("")]
        [DataRow("      ")]
        [DataTestMethod]
        public void Incorrect_Email_Validation(string email)
        {
            Assert.IsFalse(userLogic.ValidateEmail(email));
        }

        // AgeCategory ------------------------------------------------------------------------------------------------------------------

        [DataRow(AgeCategory.AGE_6)]
        [DataRow(AgeCategory.AGE_9)]
        [DataRow(AgeCategory.AGE_12)]
        [DataRow(AgeCategory.AGE_14)]
        [DataRow(AgeCategory.AGE_16)]
        [DataRow(AgeCategory.AGE_18)]
        [DataRow(AgeCategory.All)]
        [DataTestMethod]
        public void Correct_AgeCategory_Validation(AgeCategory age)
        {
            Assert.IsTrue(userLogic.ValidateAgeCategory(age));
        }

        [DataRow((AgeCategory)909)]
        [DataRow((AgeCategory)1002)]
        [DataTestMethod]
        public void Incorrect_AgeCategory_Validation(AgeCategory age)
        {
            Assert.IsFalse(userLogic.ValidateAgeCategory(age));
        }

        // Intensity ------------------------------------------------------------------------------------------------------------------

        [DataRow(Intensity.Low)]
        [DataRow(Intensity.Medium)]
        [DataRow(Intensity.High)]
        [DataTestMethod]
        public void Correct_Intensity_Validation(Intensity value)
        {
            Assert.IsTrue(userLogic.ValidateIntensity(value));
        }

        [DataRow((Intensity)909)]
        [DataRow((Intensity)1002)]
        [DataTestMethod]
        public void Incorrect_Intensity_Validation(Intensity value)
        {
            Assert.IsFalse(userLogic.ValidateIntensity(value));
        }

        // Language ------------------------------------------------------------------------------------------------------------------

        [DataRow(Language.Nederlands)]
        [DataRow(Language.English)]
        [DataTestMethod]
        public void Correct_Language_Validation(Language value)
        {
            Assert.IsTrue(userLogic.ValidateLanguage(value));
        }

        [DataRow((Language)909)]
        [DataRow((Language)1002)]
        [DataTestMethod]
        public void Incorrect_Language_Validation(Language value)
        {
            Assert.IsFalse(userLogic.ValidateLanguage(value));
        }

        // Edit User --------------------------------------------------------------------------------------------------------------------
        [TestMethod]
        public void Correct_Edit_User()
        {
            UserModel userName = Initialize_Preferences_For_User();
            var x = userLogic.Login("Petra@Petra.Petra", "testtest");
            Assert.IsTrue(x);
            Assert.IsTrue(userLogic.Edit("PetraNieuweNaam", "petranieuwemail@mail.com", new List<Genre>() { Genre.Adventure }, Intensity.High, AgeCategory.AGE_18).IsValid);
            Assert.AreEqual("petranieuwemail@mail.com", userLogic.GetById(3).EmailAddress);
            Assert.AreEqual("PetraNieuweNaam", userLogic.GetById(3).FullName);
            Assert.AreEqual(Genre.Adventure, userLogic.GetById(3).Genres[0]);
            Assert.AreEqual(Intensity.High, userLogic.GetById(3).Intensity);
            Assert.AreEqual(AgeCategory.AGE_18, userLogic.GetById(3).AgeCategory);
        }

        [TestMethod]
        public void Incorrect_Name_Edit_User()
        {
            UserModel userName = Initialize_Preferences_For_User();
            var x = userLogic.Login("Petra@Petra.Petra", "testtest");
            Assert.IsTrue(x);
            Assert.IsFalse(userLogic.Edit("", "Petra@Petra.Petra", new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery }, Intensity.High, AgeCategory.AGE_9).IsValid);
            Assert.AreNotEqual("", userLogic.GetById(3).FullName);
        }
        [DataRow("")]
        [DataRow("     ")]
        [DataRow("MailZonder@eenpunt")]
        [DataRow("GeenAppenstaartje.nl")]
        [DataTestMethod]
        public void Incorrect_Email_Edit_User(string email)
        {
            UserModel userName = Initialize_Preferences_For_User();
            var x = userLogic.Login("Petra@Petra.Petra", "testtest");
            Assert.IsTrue(x);
            Assert.IsFalse(userLogic.Edit("Petra", email, new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery }, Intensity.High, AgeCategory.AGE_9).IsValid);
            Assert.AreNotEqual(email, userLogic.GetById(3).EmailAddress);
        }

        [TestMethod]
        public void Incorrect_Genres_Edit_User()
        {
            UserModel userName = Initialize_Preferences_For_User();
            var x = userLogic.Login("Petra@Petra.Petra", "testtest");
            Assert.IsTrue(x);
            Assert.IsFalse(userLogic.Edit("Petra", "Petra@Petra.Petra", new List<Genre>() { (Genre)123 }, Intensity.High, AgeCategory.AGE_9).IsValid);
            Assert.AreNotEqual(new List<Genre> { (Genre)123 }, userLogic.GetById(3).Genres);
        }

        [TestMethod]
        public void Incorrect_Intensity_Edit_User()
        {
            UserModel userName = Initialize_Preferences_For_User();
            var x = userLogic.Login("Petra@Petra.Petra", "testtest");
            Assert.IsTrue(x);
            Assert.IsFalse(userLogic.Edit("Petra", "Petra@Petra.Petra", new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery }, (Intensity)1000, AgeCategory.AGE_9).IsValid);
            Assert.AreNotEqual((Intensity)1000, userLogic.GetById(3).Intensity);
        }

        [TestMethod]
        public void Incorrect_AgeCategory_Edit_User()
        {
            UserModel userName = Initialize_Preferences_For_User();
            var x = userLogic.Login("Petra@Petra.Petra", "testtest");
            Assert.IsTrue(x);
            Assert.IsFalse(userLogic.Edit("Petra", "Petra", new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery }, Intensity.High, (AgeCategory)999).IsValid);
            Assert.AreNotEqual((AgeCategory)999, userLogic.GetById(3).AgeCategory);
        }

        // Password hashing and verifying -------------------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Password_Hashing()
        {
            string password = "test12345";

            string hashedPassword = PasswordHasher.HashPassword(password, out byte[] salt);

            Assert.IsTrue(PasswordHasher.VerifyPassword(password, hashedPassword, salt));

        }

        [TestMethod]
        public void Incorrect_Password_Hashing()
        {
            string password = "test12345";

            string hashedPassword = PasswordHasher.HashPassword(password, out byte[] salt);

            Assert.IsFalse(PasswordHasher.VerifyPassword("test123456", hashedPassword, salt));
        }
    }
}