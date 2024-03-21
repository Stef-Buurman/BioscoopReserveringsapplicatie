class PrefencesLogic
{

    public void addPreferences(List<string> genres, int ageCategory, string intensity, string language)
    {
        
    }

    public bool ValidateGenres(List<string> genres)
    {
        if (genres.Count > 3)
        {
            Console.WriteLine("You can only select up to 3 genres.");
            return false;
        }

        if (genres.Distinct().Count() != genres.Count)
        {
            Console.WriteLine("Duplicate genres are not allowed.");
            return false;
        }

        foreach (string genre in genres)
        {
            if (genre != "Action" && genre != "Adventure" && genre != "Animation" && genre != "Comedy" && genre != "Crime" && genre != "Drama" && genre != "Fantasy" && genre != "Historical" && genre != "Horror" && genre != "Mystery" && genre != "Romance" && genre != "Science Fiction" && genre != "Thriller" && genre != "Western")
            {
                Console.WriteLine("Invalid genre, please select from the list.");
                return false;
            }
        }
        return true;
    }
    public bool ValidateAgeCategory(int ageCategory)
    {
        if (ageCategory != 6 && ageCategory != 9 && ageCategory != 12 && ageCategory != 14 && ageCategory != 16 && ageCategory != 18)
        {
            return false;
        }
        return true;
    }

    public bool ValidateIntensity(string intensity)
    {
        if (intensity != "Low" && intensity != "Medium" && intensity != "High")
        {
            return false;
        }

        return true;
    }

    public bool ValidateLanguage(string language)
    {
        if (language.ToLower() != "english" && language.ToLower() != "dutch")
        {
            return false;
        }
        return true;
    }


}