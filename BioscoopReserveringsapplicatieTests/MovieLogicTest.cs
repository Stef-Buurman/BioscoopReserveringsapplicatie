using NSubstitute;

namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class MovieLogicTest
    {
        MovieLogic moviesLogic;

        [TestInitialize]
        public void Initialize()
        {
            var movieRepositoryMock = Substitute.For<IDataAccess<MovieModel>>();
            List<MovieModel> movies = new List<MovieModel>() {
                new MovieModel(1, "Movie1", "Description1", new List<Genre> { Genre.Action, Genre.Comedy }, AgeCategory.AGE_9, Status.Active),
                new MovieModel(2, "Movie2", "Description2", new List<Genre> { Genre.Western, Genre.Horror }, AgeCategory.AGE_12, Status.Archived),
                new MovieModel(3, "Movie3", "Description3", new List<Genre> { Genre.War, Genre.Documentary }, AgeCategory.AGE_16, Status.Active),
            };
            movieRepositoryMock.LoadAll().Returns(movies);
            movieRepositoryMock.WriteAll(Arg.Any<List<MovieModel>>());

            moviesLogic = new MovieLogic(movieRepositoryMock);
        }

        // Title ------------------------------------------------------------------------------------------------------------------

        [DataRow("Test")]
        [DataRow("CrazyName")]
        [DataTestMethod]
        public void Correct_Movie_Title_Validation(string title)
        {
            Assert.IsTrue(moviesLogic.ValidateMovieTitle(title));
        }

        [DataRow("")]
        [DataRow(null)]
        [TestMethod]
        public void Incorrect_Movie_Title_Validation_With_Movie(string title)
        {
            MovieModel movie = new MovieModel(1, title, "Description", new List<Genre> { Genre.Action, Genre.Comedy }, AgeCategory.AGE_9, Status.Active);
            Assert.IsFalse(moviesLogic.Validate(movie));
            MovieModel movie2 = new MovieModel(2, title, "Description", new List<Genre> { Genre.Action, Genre.Comedy }, AgeCategory.AGE_9, Status.Active);
            Assert.IsFalse(moviesLogic.Validate(movie2));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Movie_Title_Validation_With_Title(string title)
        {
            Assert.IsFalse(moviesLogic.ValidateMovieTitle(title));
        }

        // Description ------------------------------------------------------------------------------------------------------------------

        [DataRow("Test")]
        [DataRow("CrazyDescription")]
        [DataTestMethod]
        public void Correct_Movie_Description_Validation(string description)
        {
            Assert.IsTrue(moviesLogic.ValidateMovieDescription(description));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Movie_Description_Validation_With_Movie(string description)
        {
            MovieModel movie = new MovieModel(1, "Title", description, new List<Genre> { Genre.Action, Genre.Comedy }, AgeCategory.AGE_9, Status.Active);
            Assert.IsFalse(moviesLogic.Validate(movie));
            MovieModel movie2 = new MovieModel(2, "Title", description, new List<Genre> { Genre.Action, Genre.Comedy }, AgeCategory.AGE_9, Status.Active);
            Assert.IsFalse(moviesLogic.Validate(movie2));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Movie_Description_Validation_With_Description(string description)
        {
            Assert.IsFalse(moviesLogic.ValidateMovieDescription(description));
        }

        // Genres ------------------------------------------------------------------------------------------------------------------

        [DataRow(Genre.Western, Genre.Comedy, Genre.War)]
        [DataRow(Genre.Family, Genre.Crime, Genre.Documentary)]
        [DataRow(Genre.Music, Genre.Mystery, Genre.Fantasy)]
        [DataTestMethod]
        public void Correct_Movie_Genres_Validation_With_Genres(Genre genre1, Genre genre2, Genre genre3)
        {
            List<Genre> genresList = new List<Genre> { genre1, genre2, genre3 };
            Assert.IsTrue(moviesLogic.ValidateMovieGenres(genresList));
        }

        [DataRow((Genre)909)]
        [DataRow((Genre)1002)]
        [DataTestMethod]
        public void Incorrect_Movie_Genres_Validation_With_Genres(Genre genre)
        {
            List<Genre> genresList = new List<Genre> { genre };
            Assert.IsFalse(moviesLogic.ValidateMovieGenres(genresList));
        }

        // AgeCategory ------------------------------------------------------------------------------------------------------------------

        [DataRow(AgeCategory.AGE_6)]
        [DataRow(AgeCategory.AGE_12)]
        [DataRow(AgeCategory.AGE_18)]
        [DataTestMethod]
        public void Correct_Movie_AgeCategory_Validation_With_AgeCategory(AgeCategory ageCategory)
        {
            Assert.IsTrue(moviesLogic.ValidateMovieAgeCategory(ageCategory));
        }

        [DataRow((AgeCategory)909)]
        [DataRow((AgeCategory)1002)]
        [DataTestMethod]
        public void Incorrect_Movie_AgeCategory_Validation_With_AgeCategory(AgeCategory ageCategory)
        {
            Assert.IsFalse(moviesLogic.ValidateMovieAgeCategory(ageCategory));
        }

        // Edit ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Movie_Edit_Success()
        {
            List<Genre> genresToAdd = new List<Genre> { Genre.Horror, Genre.Crime };

            moviesLogic.Edit(new MovieModel(1, "NewTitle", "NewDescription", genresToAdd, AgeCategory.AGE_16));
            Assert.AreEqual("NewTitle", moviesLogic.GetById(1).Title);
            Assert.AreEqual("NewDescription", moviesLogic.GetById(1).Description);
            Assert.AreEqual(genresToAdd, moviesLogic.GetById(1).Genres);
            Assert.AreEqual(AgeCategory.AGE_16, moviesLogic.GetById(1).AgeCategory);
        }

        [TestMethod]
        public void Incorrect_Movie_Edit_With_Invalid_Movie_Title()
        {
            moviesLogic.Edit(new MovieModel(1, "", "NewDescription", new List<Genre> { Genre.Horror, Genre.Crime }, AgeCategory.AGE_16));
            Assert.AreNotEqual("", moviesLogic.GetById(1).Title);
            Assert.AreNotEqual("NewDescription", moviesLogic.GetById(1).Description);
            Assert.AreNotEqual(new List<Genre> { Genre.Horror, Genre.Crime }, moviesLogic.GetById(1).Genres);
            Assert.AreNotEqual(AgeCategory.AGE_16, moviesLogic.GetById(1).AgeCategory);
        }

        [TestMethod]
        public void Incorrect_Movie_Edit_With_Invalid_Movie_Description()
        {
            moviesLogic.Edit(new MovieModel(1, "NewTitle", "", new List<Genre> { Genre.Horror, Genre.Crime }, AgeCategory.AGE_16));
            Assert.AreNotEqual("NewTitle", moviesLogic.GetById(1).Title);
            Assert.AreNotEqual("", moviesLogic.GetById(1).Description);
            Assert.AreNotEqual(new List<Genre> { Genre.Horror, Genre.Crime }, moviesLogic.GetById(1).Genres);
            Assert.AreNotEqual(AgeCategory.AGE_16, moviesLogic.GetById(1).AgeCategory);
        }

        [TestMethod]
        public void Incorrect_Movie_Edit_With_Invalid_Movie_Genres()
        {
            List<Genre> genresToAdd = new List<Genre> { Genre.Horror, Genre.Crime, (Genre)909 };

            moviesLogic.Edit(new MovieModel(1, "NewTitle", "NewDescription", genresToAdd, AgeCategory.AGE_16));
            Assert.AreNotEqual("NewTitle", moviesLogic.GetById(1).Title);
            Assert.AreNotEqual("NewDescription", moviesLogic.GetById(1).Description);
            Assert.AreNotEqual(genresToAdd, moviesLogic.GetById(1).Genres);
            Assert.AreNotEqual(AgeCategory.AGE_16, moviesLogic.GetById(1).AgeCategory);
        }

        [TestMethod]
        public void Incorrect_Movie_Edit_With_Invalid_Movie_AgeCategory()
        {
            moviesLogic.Edit(new MovieModel(1, "NewTitle", "NewDescription", new List<Genre> { Genre.Horror, Genre.Crime }, (AgeCategory)909));
            Assert.AreNotEqual("NewTitle", moviesLogic.GetById(1).Title);
            Assert.AreNotEqual("NewDescription", moviesLogic.GetById(1).Description);
            Assert.AreNotEqual(new List<Genre> { Genre.Horror, Genre.Crime }, moviesLogic.GetById(1).Genres);
            Assert.AreNotEqual((AgeCategory)909, moviesLogic.GetById(1).AgeCategory);
        }

        // Archived ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Movie_Archive_Success()
        {
            moviesLogic.Archive(1);
            //Assert.IsTrue(moviesLogic.GetById(1).Archived);
            Assert.AreEqual(Status.Archived, moviesLogic.GetById(1).Status);
        }

        [TestMethod]
        public void Correct_Movie_AlreadyArchived_Still_Archived()
        {
            moviesLogic.Archive(2);
            //Assert.IsTrue(moviesLogic.GetById(2).Archived);
            Assert.AreEqual(Status.Archived, moviesLogic.GetById(2).Status);
        }
    }
}