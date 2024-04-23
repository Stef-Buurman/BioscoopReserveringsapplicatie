using NSubstitute;

namespace BioscoopReserveringsapplicatieTests
{
    [TestClass]
    public class MovieLogicTest
    {
        public MoviesLogic Initialize()
        {
            var movieRepositoryMock = Substitute.For<IDataAccess<MovieModel>>();
            List<MovieModel> movies = new List<MovieModel>() {
                new MovieModel(1, "Movie1", "Description1", new List<Genre> { Genre.Actie, Genre.Komedie }, AgeCategory.AGE_9, false),
                new MovieModel(2, "Movie2", "Description2", new List<Genre> { Genre.Western, Genre.Horror }, AgeCategory.AGE_12, true),
                new MovieModel(3, "Movie3", "Description3", new List<Genre> { Genre.War, Genre.Documentary }, AgeCategory.AGE_16, false),
            };
            movieRepositoryMock.LoadAll().Returns(movies);
            movieRepositoryMock.WriteAll(Arg.Any<List<MovieModel>>());

            return new MoviesLogic(movieRepositoryMock);
        }

        // Title ------------------------------------------------------------------------------------------------------------------

        [DataRow("Test")]
        [DataRow("CrazyName")]
        [DataTestMethod]
        public void Correct_Movie_Title_Validation(string title)
        {
            MoviesLogic moviesLogic = Initialize();
            Assert.IsTrue(moviesLogic.ValidateMovieTitle(title));
        }

        [DataRow("")]
        [DataRow(null)]
        [TestMethod]
        public void Incorrect_Movie_Title_Validation_With_Movie(string title)
        {
            MoviesLogic moviesLogic = Initialize();
            MovieModel movie = new MovieModel(1, title, "Description", new List<Genre> { Genre.Actie, Genre.Komedie }, AgeCategory.AGE_9, false);
            Assert.IsFalse(moviesLogic.ValidateMovie(movie));
            MovieModel movie2 = new MovieModel(2, title, "Description", new List<Genre> { Genre.Actie, Genre.Komedie }, AgeCategory.AGE_9, false);
            Assert.IsFalse(moviesLogic.ValidateMovie(movie2));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Movie_Title_Validation_With_Title(string title)
        {
            MoviesLogic moviesLogic = Initialize();
            Assert.IsFalse(moviesLogic.ValidateMovieTitle(title));
        }

        // Description ------------------------------------------------------------------------------------------------------------------

        [DataRow("Test")]
        [DataRow("CrazyDescription")]
        [DataTestMethod]
        public void Correct_Movie_Description_Validation(string description)
        {
            MoviesLogic moviesLogic = Initialize();
            Assert.IsTrue(moviesLogic.ValidateMovieDescription(description));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Movie_Description_Validation_With_Movie(string description)
        {
            MoviesLogic moviesLogic = Initialize();
            MovieModel movie = new MovieModel(1, "Title", description, new List<Genre> { Genre.Actie, Genre.Komedie }, AgeCategory.AGE_9, false);
            Assert.IsFalse(moviesLogic.ValidateMovie(movie));
            MovieModel movie2 = new MovieModel(2, "Title", description, new List<Genre> { Genre.Actie, Genre.Komedie }, AgeCategory.AGE_9, false);
            Assert.IsFalse(moviesLogic.ValidateMovie(movie2));
        }

        [DataRow("")]
        [DataRow(null)]
        [DataTestMethod]
        public void Incorrect_Movie_Description_Validation_With_Description(string description)
        {
            MoviesLogic moviesLogic = Initialize();
            Assert.IsFalse(moviesLogic.ValidateMovieDescription(description));
        }

        // Genres ------------------------------------------------------------------------------------------------------------------

        [DataRow(Genre.Western, Genre.Komedie, Genre.War)]
        [DataRow(Genre.Family, Genre.Crime, Genre.Documentary)]
        [DataRow(Genre.Music, Genre.Mystery, Genre.Fantasy)]
        [DataTestMethod]
        public void Correct_Movie_Genres_Validation_With_Genres(Genre genre1, Genre genre2, Genre genre3)
        {
            MoviesLogic moviesLogic = Initialize();
            List<Genre> genresList = new List<Genre> { genre1, genre2, genre3 };
            Assert.IsTrue(moviesLogic.ValidateMovieGenres(genresList));
        }

        [DataRow((Genre)909)]
        [DataRow((Genre)1002)]
        [DataTestMethod]
        public void Incorrect_Movie_Genres_Validation_With_Genres(Genre genre)
        {
            MoviesLogic moviesLogic = Initialize();
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
            MoviesLogic moviesLogic = Initialize();
            Assert.IsTrue(moviesLogic.ValidateMovieAgeCategory(ageCategory));
        }

        [DataRow((AgeCategory)909)]
        [DataRow((AgeCategory)1002)]
        [DataTestMethod]
        public void Incorrect_Movie_AgeCategory_Validation_With_AgeCategory(AgeCategory ageCategory)
        {
            MoviesLogic moviesLogic = Initialize();
            Assert.IsFalse(moviesLogic.ValidateMovieAgeCategory(ageCategory));
        }

        // Edit ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Movie_Edit_Success()
        {
            MoviesLogic moviesLogic = Initialize();
            List<Genre> genresToAdd = new List<Genre> { Genre.Horror, Genre.Crime };

            moviesLogic.EditMovie(1, "NewTitle", "NewDescription", genresToAdd, AgeCategory.AGE_16);
            Assert.AreEqual("NewTitle", moviesLogic.GetMovieById(1).Title);
            Assert.AreEqual("NewDescription", moviesLogic.GetMovieById(1).Description);
            Assert.AreEqual(genresToAdd, moviesLogic.GetMovieById(1).Genres);
            Assert.AreEqual(AgeCategory.AGE_16, moviesLogic.GetMovieById(1).AgeCategory);
        }

        [TestMethod]
        public void Incorrect_Movie_Edit_With_Invalid_Movie_Title()
        {
            MoviesLogic moviesLogic = Initialize();
            moviesLogic.EditMovie(1, "", "NewDescription", new List<Genre> { Genre.Horror, Genre.Crime }, AgeCategory.AGE_16);
            Assert.AreNotEqual("", moviesLogic.GetMovieById(1).Title);
            Assert.AreNotEqual("NewDescription", moviesLogic.GetMovieById(1).Description);
            Assert.AreNotEqual(new List<Genre> { Genre.Horror, Genre.Crime }, moviesLogic.GetMovieById(1).Genres);
            Assert.AreNotEqual(AgeCategory.AGE_16, moviesLogic.GetMovieById(1).AgeCategory);
        }

        [TestMethod]
        public void Incorrect_Movie_Edit_With_Invalid_Movie_Description()
        {
            MoviesLogic moviesLogic = Initialize();
            moviesLogic.EditMovie(1, "NewTitle", "", new List<Genre> { Genre.Horror, Genre.Crime }, AgeCategory.AGE_16);
            Assert.AreNotEqual("NewTitle", moviesLogic.GetMovieById(1).Title);
            Assert.AreNotEqual("", moviesLogic.GetMovieById(1).Description);
            Assert.AreNotEqual(new List<Genre> { Genre.Horror, Genre.Crime }, moviesLogic.GetMovieById(1).Genres);
            Assert.AreNotEqual(AgeCategory.AGE_16, moviesLogic.GetMovieById(1).AgeCategory);
        }

        [TestMethod]
        public void Incorrect_Movie_Edit_With_Invalid_Movie_Genres()
        {
            MoviesLogic moviesLogic = Initialize();
            List<Genre> genresToAdd = new List<Genre> { Genre.Horror, Genre.Crime, (Genre)909 };

            moviesLogic.EditMovie(1, "NewTitle", "NewDescription", genresToAdd, AgeCategory.AGE_16);
            Assert.AreNotEqual("NewTitle", moviesLogic.GetMovieById(1).Title);
            Assert.AreNotEqual("NewDescription", moviesLogic.GetMovieById(1).Description);
            Assert.AreNotEqual(genresToAdd, moviesLogic.GetMovieById(1).Genres);
            Assert.AreNotEqual(AgeCategory.AGE_16, moviesLogic.GetMovieById(1).AgeCategory);
        }

        [TestMethod]
        public void Incorrect_Movie_Edit_With_Invalid_Movie_AgeCategory()
        {
            MoviesLogic moviesLogic = Initialize();
            moviesLogic.EditMovie(1, "NewTitle", "NewDescription", new List<Genre> { Genre.Horror, Genre.Crime }, (AgeCategory)909);
            Assert.AreNotEqual("NewTitle", moviesLogic.GetMovieById(1).Title);
            Assert.AreNotEqual("NewDescription", moviesLogic.GetMovieById(1).Description);
            Assert.AreNotEqual(new List<Genre> { Genre.Horror, Genre.Crime }, moviesLogic.GetMovieById(1).Genres);
            Assert.AreNotEqual((AgeCategory)909, moviesLogic.GetMovieById(1).AgeCategory);
        }

        // Archived ------------------------------------------------------------------------------------------------------------------

        [TestMethod]
        public void Correct_Movie_Archive_Success()
        {
            MoviesLogic moviesLogic = Initialize();
            moviesLogic.Archive(1);
            Assert.IsTrue(moviesLogic.GetMovieById(1).Archived);
        }

        [TestMethod]
        public void Correct_Movie_AlreadyArchived_Still_Archived()
        {
            MoviesLogic moviesLogic = Initialize();
            moviesLogic.Archive(2);
            Assert.IsTrue(moviesLogic.GetMovieById(2).Archived);
        }
    }
}