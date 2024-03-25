using System;
using System.Threading;

static class LandingPage
{
    //This shows the menu. You can call back to this method to show the menu again
    //after another presentation method is completed.
    //You could edit this to show different menus depending on the user's role
    public static void Start()
    {
        Console.Clear();
        var options = new List<Option<string>>
            {
                new Option<string>("Inloggen", () => UserLogin.Start()),
                new Option<string>("Registreren", () => UserRegister.Start()),
                new Option<string>("Sluit Aplicatie", () => Environment.Exit(0)),
            };
            SelectionMenu.Create(options, () => Console.WriteLine("Welkom bij de FX reserveringsapplicatie\n"));

    }
}