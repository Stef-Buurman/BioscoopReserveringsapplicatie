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
                new MovieModel(2, "Movie2", "Description2", new List<Genre> { Genre.Western, Genre.Horror }, AgeCategory.AGE_12, true)
            };
            movieRepositoryMock.LoadAll().Returns(movies);
            movieRepositoryMock.WriteAll(Arg.Any<List<MovieModel>>());

            MoviesAccess.NewDataAccess(movieRepositoryMock);
            return new MoviesLogic();
        }

        // Archive ------------------------------------------------------------------------------------------------------------------
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