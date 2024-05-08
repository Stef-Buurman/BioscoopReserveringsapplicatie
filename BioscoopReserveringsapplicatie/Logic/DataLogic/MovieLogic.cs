namespace BioscoopReserveringsapplicatie
{
    public class MovieLogic : ILogic<MovieModel>
    {
        private List<MovieModel> _Movies;
        public IDataAccess<MovieModel> _DataAccess { get; }
        public MovieLogic(IDataAccess<MovieModel> dataAccess = null)
        {
            if (dataAccess != null) _DataAccess = dataAccess;
            else _DataAccess = new DataAccess<MovieModel>();

            _Movies = _DataAccess.LoadAll();
        }

        public int GetNextId() => IdGenerator.GetNextId(_Movies);

        public List<MovieModel> GetAll()
        {
            _Movies = _DataAccess.LoadAll();
            return _Movies;
        }

        public bool Add(MovieModel movie)
        {
            GetAll();

            if (!Validate(movie))
            {
                return false;
            }

            UpdateList(movie);
            return true;
        }

        public bool Edit(MovieModel movie)
        {
            if (!Validate(movie))
            {
                return false;
            }

            UpdateList(movie);
            return true;
        }

        public bool Edit(int id, string title, string description, List<Genre> genres, AgeCategory rating)
        {
            if (ValidateMovieTitle(title) && ValidateMovieDescription(description) && ValidateMovieGenres(genres) && ValidateMovieAgeCategory(rating) && id != 0)
            {
                MovieModel movie = GetById(id);
                movie.Title = title;
                movie.Description = description;
                movie.Genres = genres;
                movie.AgeCategory = rating;

                UpdateList(movie);
                return true;
            }

            return false;
        }

        public bool Validate(MovieModel movie)
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

        public MovieModel GetById(int id)
        {
            _Movies = _DataAccess.LoadAll();
            return _Movies.Find(i => i.Id == id);
        }

        public void Archive(int id)
        {
            MovieModel movie = GetById(id);
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
            MovieModel movie = GetById(id);
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