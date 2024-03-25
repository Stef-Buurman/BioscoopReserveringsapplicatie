namespace BioscoopReserveringsapplicatie
{
    class MoviesLogic
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

        public bool AddMovie(string title, string description, List<string> genres, string rating)
        {
            if (title.Trim() == "" || description.Trim() == "" || genres.Count == 0 || rating.Trim() == "")
            {
                Console.WriteLine("Vul alstublieft alle velden in.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(description) && genres.Any() && !string.IsNullOrWhiteSpace(rating))
            {
                try
                {
                    MovieModel latestMovie = _Movies.Last();

                    MovieModel movie = new MovieModel(latestMovie.Id + 1, title, description, genres, rating);

                    UpdateList(movie);
                }
                catch (InvalidOperationException)
                {
                    MovieModel movie = new MovieModel(1, title, description, genres, rating);

                    UpdateList(movie);
                }
                return true;
            }

            return false;
        }

        public bool EditMovie(int id, string title, string description, List<string> genres, string rating)
        {
            if (id == 0 || title.Trim() == "" || description.Trim() == "" || genres.Count == 0 || rating.Trim() == "")
            {
                Console.WriteLine("Vul alstublieft alle velden in.");
                return false;
            }

            if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(description) && genres.Any() && !string.IsNullOrWhiteSpace(rating))
            {
                MovieModel movie = GetMovieById(id);
                movie.Title = title;
                movie.Description = description;
                movie.Genres = genres;
                movie.Rating = rating;

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

        public void RemoveMovie(int id)
        {
            _Movies.RemoveAll(i => i.Id == id);
            MoviesAccess.WriteAll(_Movies);
        }
    }
}