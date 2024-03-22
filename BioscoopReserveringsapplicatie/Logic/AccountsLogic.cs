using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;


//This class is not static so later on we can use inheritance and interfaces
class AccountsLogic
{
    private List<AccountModel> _accounts;

    //Static properties are shared across all instances of the class
    //This can be used to get the current logged in account from anywhere in the program
    //private set, so this can only be set by the class itself
    static public AccountModel? CurrentAccount { get; private set; }

    public AccountsLogic()
    {
        _accounts = AccountsAccess.LoadAll();
    }


    public void UpdateList(AccountModel acc)
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
        AccountsAccess.WriteAll(_accounts);

    }

    public AccountModel GetById(int id)
    {
        return _accounts.Find(i => i.Id == id);
    }

    private bool ValidatePassword(string email, string password)
    {
        AccountModel account = _accounts.Find(i => i.EmailAddress == email);
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

    public AccountModel CheckLogin(string email, string password)
    {
        if (email == null || password == null)
        {
            return null;
        }

        if (!ValidateEmail(email))
        {
            return null;
        }

        if (!ValidatePassword(email, password))
        {
            return null;
        }

        CurrentAccount = _accounts.Find(i => i.EmailAddress == email);
        return CurrentAccount;
    }
    public void addPreferencesToAccount(List<string> genres, int ageCategory, string intensity, string language)
    {   
        if (CurrentAccount != null)
        {
            CurrentAccount.Genres = genres;
            CurrentAccount.AgeCategory = ageCategory;
            CurrentAccount.Intensity = intensity;
            CurrentAccount.Language = language;
            UpdateList(CurrentAccount);
        }
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



