//This class is not static so later on we can use inheritance and interfaces
namespace BioscoopReserveringsapplicatie
{
    public class UserLogic
    {
        private List<UserModel> _accounts;

        //Static properties are shared across all instances of the class
        //This can be used to get the current logged in account from anywhere in the program
        //private set, so this can only be set by the class itself
        static public UserModel? CurrentUser { get; private set; }
        //private IDataAccess<UserModel> UserAccess;

        public UserModel? GetCurrentUser { get => CurrentUser; }

        public UserLogic()
        {
            //if (userAccess != null) UserAccess = userAccess;
            //else UserAccess = new UserAccess();

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
                UserModel newAccount = new UserModel(_accounts.Count + 1, false, true, email, password, name, new List<Genre>(), 0, default, "");
                UpdateList(newAccount);
                CheckLogin(email, password);
            }
            else
            {
                UserRegister.Start(errorMessage);
            }
        }

        public UserModel? CheckLogin(string email, string password)
        {

            if (email == null || password == null)
            {
                return null;
            }

            if (!ValidateEmail(email.ToLower()))
            {
                return null;
            }

            if (!LoginUser(email.ToLower(), password))
            {
                return null;
            }

            if (CurrentUser != null)
            {
                if (CurrentUser.IsAdmin)
                {
                    AdminMenu.Start();
                }
                else
                {
                    if (CurrentUser.FirstTimeLogin)
                    {
                        Preferences.Start();
                    }
                    Console.WriteLine($"Welkom {CurrentUser.FullName}!");
                    //UserMenu.Start();
                }
            }
            return CurrentUser;
        }

        public bool LoginUser(string email, string password)
        {
            UserModel? account = _accounts.Find(i => i.EmailAddress == email);
            if (account != null && account.Password == password)
            {
                CurrentUser = account;
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool ValidateEmail(string email)
        {
            return email.Contains("@");
        }

        public void addPreferencesToAccount(List<Genre> genres, int ageCategory, Intensity intensity, string language)
        {
            if (CurrentUser != null)
            {
                CurrentUser.Genres = genres;
                CurrentUser.AgeCategory = ageCategory;
                CurrentUser.Intensity = intensity;
                CurrentUser.Language = language;
                CurrentUser.FirstTimeLogin = false;
                UpdateList(CurrentUser);
            }
        }

        public bool ValidateGenres(List<Genre> genres)
        {
            List<Genre> CorrectGenre = Globals.GetAllEnum<Genre>();

            if (genres.Count > 3)
            {
                Console.WriteLine("U kunt maximaal 3 genres selecteren.");
                return false;
            }

            if (genres.Distinct().Count() != genres.Count)
            {
                Console.WriteLine("U mag niet een genre meerdere keren selecteren.");
                return false;
            }

            foreach (Genre genre in genres)

            {
                if (!CorrectGenre.Contains(genre))
                {
                    Console.WriteLine("Ongeldige input, Selecteer genres uit de lijst.");
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

        public bool ValidateIntensity(Intensity intensity)
        {
            if (!Enum.IsDefined(typeof(Intensity), intensity))
            {
                return false;
            }

            return true;
        }

        public bool ValidateLanguage(string language)
        {
            if (language.ToLower() != "english" && language.ToLower() != "nederlands")
            {
                return false;
            }
            return true;
        }


    }
}