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

    public AccountModel? GetById(int id)
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
            AccountModel newAccount = new AccountModel(_accounts.Count + 1, email, password, name);
            UpdateList(newAccount);
        }
        else
        {
            UserRegister.Start(errorMessage);
        }
    }

    public AccountModel? CheckLogin(string email, string password)
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

}




