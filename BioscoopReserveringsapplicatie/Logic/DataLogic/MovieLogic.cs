namespace BioscoopReserveringsapplicatie
{
    public class MovieLogic
    {
        private List<MovieModel> _Movies;
        private IDataAccess<MovieModel> _DataAccess = new DataAccess<MovieModel>();
        public MovieLogic(IDataAccess<MovieModel> dataAccess = null)
        {
            if (dataAccess != null) _DataAccess = dataAccess;
            else _DataAccess = new DataAccess<MovieModel>();

            _Movies = _DataAccess.LoadAll();
        }

        public List<MovieModel> GetAllMovies()
        {
            _Movies = _DataAccess.LoadAll();
            return _Movies;
        }

        public bool AddMovie(string title, string description, List<Genre> genres, AgeCategory rating)
        {
            GetAllMovies();

            if (ValidateMovieTitle(title) && ValidateMovieDescription(description) && ValidateMovieGenres(genres) && ValidateMovieAgeCategory(rating))
            {
                MovieModel movie = new MovieModel(IdGenerator.GetNextId(_Movies), title, description, genres, rating, false);

                if (this.ValidateMovie(movie))
                {
                    UpdateList(movie);
                    return true;
                }
            }

            return false;
        }

        public bool EditMovie(int id, string title, string description, List<Genre> genres, AgeCategory rating)
        {
            if (ValidateMovieTitle(title) && ValidateMovieDescription(description) && ValidateMovieGenres(genres) && ValidateMovieAgeCategory(rating) && id != 0)
            {
                MovieModel movie = GetMovieById(id);
                movie.Title = title;
                movie.Description = description;
                movie.Genres = genres;
                movie.AgeCategory = rating;

                UpdateList(movie);
                return true;
            }

            return false;
        }

        public bool ValidateMovie(MovieModel movie)
        {
            if (movie == null) return false;
            else if (!ValidateMovieTitle(movie.Title)) return false;
            else if (!ValidateMovieDescription(movie.Description)) return false;
            else if (!ValidateMovieGenres(movie.Genres)) return false;
            else if (!ValidateMovieAgeCategory(movie.AgeCategory)) return false;
            return true;
        }

        public bool ValidateMovieTitle(string title)
        {
            return !string.IsNullOrWhiteSpace(title);
        }

        public bool ValidateMovieDescription(string description)
        {
            return !string.IsNullOrWhiteSpace(description);
        }

        public bool ValidateMovieGenres(List<Genre> genres)
        {
            foreach (Genre genre in genres)
            {
                if (!Enum.IsDefined(typeof(Genre), genre))
                {
                    return false;
                }
            }
            return true;
        }

        public bool ValidateMovieAgeCategory(AgeCategory rating)
        {
            return Enum.IsDefined(typeof(AgeCategory), rating);
        }

        public bool ValidateMovieArchived(bool archived)
        {
            return true;
        }

        public void UpdateList(MovieModel movie)
        {
            //Find if there is already an model with the same id
            int index = _Movies.FindIndex(s => s.Id == movie.Id);

            if (index != -1)
            {
                //update existing model
                _Movies[index] = movie;
            }
            else
            {
                //add new model
                _Movies.Add(movie);
            }
            _DataAccess.WriteAll(_Movies);
        }

        public MovieModel GetMovieById(int id)
        {
            _Movies = _DataAccess.LoadAll();
            return _Movies.Find(i => i.Id == id);
        }

        public void Archive(int id)
        {
            MovieModel movie = GetMovieById(id);
            if (movie != null)
            {
                movie.Archived = true;
                UpdateList(movie);
            }
            else
            {
                return;
            }
        }

        public void Unarchive(int id)
        {
            MovieModel movie = GetMovieById(id);
            if (movie != null)
            {
                movie.Archived = false;
                UpdateList(movie);
            }
            else
            {
                return;
            }
        }

        public List<MovieModel> GetAllArchivedMovies()
        {
            _Movies = _DataAccess.LoadAll();
            return _Movies.FindAll(m => m.Archived);
        }

        public List<MovieModel> GetAllActiveMovies()
        {
            _Movies = _DataAccess.LoadAll();
            return _Movies.FindAll(m => !m.Archived);
        }
    }
}