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
        AccountModel newAccount = new AccountModel(_accounts.Count + 1, email, password, name);
        UpdateList(newAccount);
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
}
