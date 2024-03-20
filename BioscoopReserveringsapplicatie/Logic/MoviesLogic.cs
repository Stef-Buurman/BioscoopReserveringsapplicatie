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

    public List<MovieModel> getAllMovies()
    {
        return _Movies;
    }

    public bool AddMovie(string title, string description, string genre, string rating)
    {
        if (title.Trim() == "" || description.Trim() == "" || genre.Trim() == "" || rating.Trim() == "")
        {
            Console.WriteLine("Please fill in all fields.");
            return false;
        }

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
        return _Movies.Find(i => i.Id == id);
    }

    public List<Option<string>> getAllMoviesAsOptions()
    {
        List<Option<string>> options = new List<Option<string>>();

        foreach (MovieModel movie in _Movies)
        {
            options.Add(new Option<string>(movie.Title, () => MovieDetails.Start(movie.Id)));
        }

        return options;
    }
}
