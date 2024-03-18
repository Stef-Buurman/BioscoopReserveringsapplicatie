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

    public AccountModel CheckLogin(string email, string password)
    {
        if (email == null || password == null)
        {
            return null;
        }
        CurrentAccount = _accounts.Find(i => i.EmailAddress == email && i.Password == password);
        return CurrentAccount;
    }

    public bool ValidateLogin(string email, string password)
    {
        if (email == null || password == null)
        {
            return false;
        }

        AccountModel account = _accounts.Find(i => i.EmailAddress == email);

        if (account != null)
        {
            
        }
        return false;
    }

    public AccountModel Login(string email, string password)
    {
        if (ValidateLogin(email, password))
        {
            Console.WriteLine("Inloggen gelukt.");
            CurrentAccount = _accounts.Find(i => i.EmailAddress == email);
        // Redirect user to the homepage or perform other actions
            return CurrentAccount; // Return the logged-in account
        }
        else
        {
            Console.WriteLine("Inloggen mislukt.");
            // Handle failed login (e.g., reload login page)
            return null; // Return null since login failed
        }
    }
}




