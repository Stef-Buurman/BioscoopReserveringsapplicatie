using BioscoopReserveringsapplicatie;
using NSubstitute;

namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class UserManage
    {
        private UserLogic userManager;
        public void Initialize()
        {
            var userRepositoryMock = Substitute.For<IDataAccess<UserModel>>();
            List<UserModel> users = new List<UserModel>() {
                new UserModel(1, false, false, "Henk@henk.henk", "testtest", "henk", null, -1, null, null),
                new UserModel(2, false, true, "Gerda@Gerda.Gerda", "testtest", "Gerda", null, -1, null, null),
                new UserModel(3, true, false, "Petra@Petra.Petra", "testtest", "Petra",new List<Genre>() { Genre.Horror, Genre.Mystery, Genre.Family }, 6, "laag", "Nederlands"),
                new UserModel(4, false, false, "Nick@Nick.Nick", "Nick2003", "Nick", null, -1, null, null),
                new UserModel(5, true, true, "Pieter@Pieter.Pieter", "Pieter2003", "Pieter", new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery }, 6, "laag", "Nederlands"),
                new UserModel(6, false, false, "Tim@Tim.Tim", "Tim2003", "Tim", null, -1, null, null),
                new UserModel(1, false, false, "Stef@Stef.Stef", "Stef2003", "Stef", new List<Genre>() { Genre.Horror, Genre.Western, Genre.Romance }, 6, "laag", "Nederlands"),
                new UserModel(1, false, false, "Menno@Menno.Menno", "Menno2003", "Menno", null, -1, null, null),
            };
            userRepositoryMock.LoadAll().Returns(users);
            userRepositoryMock.WriteAll(Arg.Any<List<UserModel>>());

            UserAccess.NewDataAccess(userRepositoryMock);
            userManager = new UserLogic();
        }

        [TestMethod]
        public void GetUserNameById_Returns_UserName1()
        {
            var x = userManager.LoginUser("Henk@henk.henk", "testtest");
            userManager.addPreferencesToAccount(new List<Genre>() { Genre.Adventure, Genre.Drama, Genre.Mystery }, 6, "laag", "Nederlands");
            UserModel userName = userManager.GetById(1);

            Assert.IsNotNull(userName);
            Assert.IsTrue(x);
            Assert.AreEqual("henk", userName.FullName);
            Assert.AreEqual(6, userName.AgeCategory);
            Assert.AreEqual("laag", userName.Intensity);
            Assert.AreEqual("Nederlands", userName.Language);
        }

        [TestMethod]
        public void CorrectLoginUser()
        {
            Assert.IsTrue(userManager.LoginUser("Henk@henk.henk", "testtest"));
        }
    }
}