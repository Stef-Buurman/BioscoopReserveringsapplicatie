namespace BioscoopReserveringsapplicatie
{
    public class MoviesLogic
    {
        private List<MovieModel> _Movies;

        public MoviesLogic()
        {
            _Movies = MoviesAccess.LoadAll();
        }

        public List<MovieModel> GetAllMovies()
        {
            _Movies = MoviesAccess.LoadAll();
            return _Movies;
        }

        public bool AddMovie(string title, string description, List<Genre> genres, AgeCategory rating)
        {
            GetAllMovies();
            
            if (title.Trim() == "" || description.Trim() == "" || genres.Count == 0 || rating == AgeCategory.Undefined)
            {
                Console.WriteLine("Vul alstublieft alle velden in.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(description) && genres.Any())
            {
                MovieModel movie = new MovieModel(IdGenerator.GetNextId(_Movies), title, description, genres, rating, false);
                UpdateList(movie);
                return true;
            }

            return false;
        }

        public bool EditMovie(int id, string title, string description, List<Genre> genres, AgeCategory rating)
        {
            if (id == 0 || title.Trim() == "" || description.Trim() == "" || genres.Count == 0 || rating == AgeCategory.Undefined)
            {
                Console.WriteLine("Vul alstublieft alle velden in.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(description) && genres.Any())
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
            MoviesAccess.WriteAll(_Movies);
        }

        public MovieModel GetMovieById(int id)
        {
            _Movies = MoviesAccess.LoadAll();
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

        public List<MovieModel> GetAllArchivedMovies()
        {
            _Movies = MoviesAccess.LoadAll();
            return _Movies.FindAll(m => m.Archived);
        }

        public List<MovieModel> GetAllActiveMovies()
        {
            _Movies = MoviesAccess.LoadAll();
            return _Movies.FindAll(m => !m.Archived);
        }
    }
}