﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
class UserLogic
{
    private List<UserModel> _accounts;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    static public UserModel? CurrentUser { get; private set; }

    public UserLogic()
    {
        _accounts = UserAccess.LoadAll();
    }


    public void UpdateList(UserModel acc)
    {
        //Find if there is already an model with the same id
        int index = _accounts.FindIndex(s => s.Id == acc.Id);

        if (index != -1)
        {
            //update existing model
            _accounts[index] = acc;
        }
        else
        {
            //add new model
            _accounts.Add(acc);
        }
        UserAccess.WriteAll(_accounts);

    }

    public UserModel? GetById(int id)
    {
        return _accounts.Find(i => i.Id == id);
    }

    public void RegisterNewUser(string name, string email, string password)
    {
        bool validated = false;
        string errorMessage = "";

        if (name == "")
        {
            errorMessage += "Naam mag niet leeg zijn!\n";
        }
        else if (email == "")
        {
            errorMessage += "Email mag niet leeg zijn!\n";
        }
        else if (_accounts.Exists(i => i.EmailAddress == email))
        {
            errorMessage += "Email is al in gebruik!\n";
        }
        else if (email.Length < 6)
        {
            errorMessage += "Email moet minimaal 6 karakters bevatten!\n";
        }
        else if (!email.Contains("@"))
        {
            errorMessage += "Email moet een @ bevatten!\n";
        }
        else if (!email.Contains("."))
        {
            errorMessage += "Email moet een . bevatten!\n";
        }
        else if (password == "")
        {
            errorMessage += "Wachtwoord mag niet leeg zijn\n";
        }
        else if (password.Length < 5)
        {
            errorMessage += "Wachtwoord moet minimaal 5 karakters bevatten!\n";
        }
        else
        {
            validated = true;
        }

        if (validated)
        {
            UserModel newAccount = new UserModel(_accounts.Count + 1,false , email, password, name, new List<string>(), 0, "", "");
            UpdateList(newAccount);
        }
        else
        {
            UserRegister.Start(errorMessage);
        }
    }

    public UserModel? CheckLogin(string ?email, string ?password)
    {
        
        if (email == null || password == null)
        {
            return null;
        }

        if (!ValidateEmail(email.ToLower()))
        {
            return null;
        }

        if (!ValidatePassword(email.ToLower(), password))
        {
            return null;
        }

        CurrentUser = _accounts.Find(i => i.EmailAddress == email.ToLower());
        return CurrentUser;
    }
    
    private bool ValidatePassword(string email, string password)
    {
        UserModel ?account = _accounts.Find(i => i.EmailAddress == email);
        if (account != null && account.Password == password)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool ValidateEmail(string email)
    {
        return email.Contains("@");
    }

    public void addPreferencesToAccount(List<string> genres, int ageCategory, string intensity, string language)
    {   
        if (CurrentUser != null)
        {
            CurrentUser.Genres = genres;
            CurrentUser.AgeCategory = ageCategory;
            CurrentUser.Intensity = intensity;
            CurrentUser.Language = language;
            UpdateList(CurrentUser);
        }
    }

    public bool ValidateGenres(List<string> genres)
    {
    List <string> CorrectGenre = new List<string>
        {
            "Horror", "Komedie", "Actie", "Drama", "Thriller", "Romantiek", "Sci-fi",
            "Fantasie", "Avontuur", "Animatie", "Misdaad", "Mysterie", "Familie",
            "Oorlog", "Geschiedenis", "Muziek", "Documentaire", "Westers", "TV-film"
        };
    
        if (genres.Count > 3)
        {
            Console.WriteLine("U kunt maximaal 3 genres selecteren.");
            return false;
        }

        if (genres.Distinct().Count() != genres.Count)
        {
            Console.WriteLine("U mag niet een genre meerderekeeren selecteren.");
            return false;
        }

        foreach (string genre in genres)
        
        {
            if (!CorrectGenre.Contains(genre))
            {
                Console.WriteLine("Ongeldige input, Selecteer genres van de lijst.");
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
        if (intensity.ToLower() != "low" && intensity.ToLower() != "medium" && intensity.ToLower() != "high")
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



