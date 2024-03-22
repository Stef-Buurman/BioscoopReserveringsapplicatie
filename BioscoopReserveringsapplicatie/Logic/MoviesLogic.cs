using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

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

    public bool AddMovie(string title, string description, string genre, string rating)
    {
        if (title.Trim() == "" || description.Trim() == "" || genre.Trim() == "" || rating.Trim() == "")
        {
            Console.WriteLine("Please fill in all fields.");
            return false;
        }

        if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(description) && !string.IsNullOrWhiteSpace(genre) && !string.IsNullOrWhiteSpace(rating))
        {
            try
            {
                MovieModel latestMovie = _Movies.Last();

                MovieModel movie = new MovieModel(latestMovie.Id + 1, title, description, genre, rating);

                UpdateList(movie);
            }
            catch (InvalidOperationException)
            {
                MovieModel movie = new MovieModel(1, title, description, genre, rating);

                UpdateList(movie);
            }
            return true;
        }

        return false;
    }

    public bool EditMovie(int id, string title, string description, string genre, string rating)
    {
        if (id == 0 || title.Trim() == "" || description.Trim() == "" || genre.Trim() == "" || rating.Trim() == "")
        {
            Console.WriteLine("Please fill in all fields.");
            return false;
        }

        if (!string.IsNullOrWhiteSpace(title) && !string.IsNullOrWhiteSpace(description) && !string.IsNullOrWhiteSpace(genre) && !string.IsNullOrWhiteSpace(rating))
        {
            MovieModel movie = GetMovieById(id);
            movie.Title = title;
            movie.Description = description;
            movie.Genre = genre;
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
}
