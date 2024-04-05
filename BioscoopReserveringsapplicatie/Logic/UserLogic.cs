namespace BioscoopReserveringsapplicatie
{
    public class UserLogic
    {
        private List<UserModel> _accounts;

        //Static properties are shared across all instances of the class
        //This can be used to get the current logged in account from anywhere in the program
        //private set, so this can only be set by the class itself
        static public UserModel? CurrentUser { get; private set; }

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

        public RegistrationResult RegisterNewUser(string name, string email, string password)
        {
            bool validated = false;
            string errorMessage = "";

            email = email.ToLower();

            if (name == "")
            {
                errorMessage += $"{RegisterNewUserErrorMessages.NameEmpty}\n";
            }
            if (email == "")
            {
                errorMessage += $"{RegisterNewUserErrorMessages.EmailEmpty}\n";
            }
            if (_accounts.Exists(i => i.EmailAddress == email))
            {
                errorMessage += $"{RegisterNewUserErrorMessages.EmailAlreadyExists}\n";
                email = "";
            }
            else if (!ValidateEmail(email))
            {
                errorMessage += $"{RegisterNewUserErrorMessages.EmailAdressIncomplete}\n";
                email = "";
            }
            if (password == "")
            {
                errorMessage += $"{RegisterNewUserErrorMessages.PasswordEmpty}\n";
            }
            if (password.Length < 5)
            {
                errorMessage += $"{RegisterNewUserErrorMessages.PasswordMinimumChars}\n";
                password = "";
            }

            if (errorMessage == "")
            {
                validated = true;
            }

            UserModel newAccount = null;

            if (validated)
            {
                newAccount = new UserModel(_accounts.Count + 1, false, email, password, name, new List<Genre>(), 0, default, default);
                UpdateList(newAccount);
                CheckLogin(email, password);
            }
            else
            {
                newAccount = new UserModel(_accounts.Count + 1, false, email, password, name, new List<Genre>(), 0, default, default);
            }
            return new RegistrationResult(validated, errorMessage, newAccount);
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

            if (!Login(email.ToLower(), password))
            {
                return null;
            }

            return CurrentUser;
        }

        public bool Login(string email, string password)
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
            return email.Contains("@") && email.Contains(".") && email.Length > 6 && email.Trim() != "";
        }

        public bool ValidateName(string name)
        {
            return name != null && name.Trim() != ""; 
        }

        public void addPreferencesToAccount(List<Genre> genres, AgeCategory ageCategory, Intensity intensity, Language language, UserModel user)
        {
            if (user != null)
            {
                user.Genres = genres;
                user.AgeCategory = ageCategory;
                user.Intensity = intensity;
                user.Language = language;
                UpdateList(user);
            }
        }

        public void addPreferencesToAccount(List<Genre> genres, AgeCategory ageCategory, Intensity intensity, Language language, UserModel user)
        {
            if (user != null)
            {
                user.Genres = genres;
                user.AgeCategory = ageCategory;
                user.Intensity = intensity;
                user.Language = language;
                UpdateList(user);
            }
        }

        public void addPreferencesToAccount(List<Genre> genres, AgeCategory ageCategory, Intensity intensity, Language language)
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
        public bool ValidateAgeCategory(AgeCategory ageCategory)
        {
            if (!Enum.IsDefined(typeof(AgeCategory), ageCategory))
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

        public bool ValidateLanguage(Language language)
        {
            if (!Enum.IsDefined(typeof(Language), language))
            {
                return false;
            }
            return true;
        }

        public static void Logout()
        {
            CurrentUser = null;
            Console.WriteLine("U bent uitgelogd.");
            Thread.Sleep(2000);

        public bool Edit(int id, string newName, string newEmail, List<Genre> newGenres, Intensity newIntensity, AgeCategory newAgeCategory)
        {
            UserModel? user = GetById(id);
            if(user != null)
            {
                if (!ValidateName(newName) || !ValidateEmail(newEmail) || !ValidateGenres(newGenres) ||
                    !ValidateIntensity(newIntensity) || !ValidateAgeCategory(newAgeCategory))
                {
                    Console.WriteLine("Niet alle velden zijn correct ingevuld.");
                    Thread.Sleep(3000);
                    return false;
                }
                else
                {   
                    user.FullName = newName;
                    newEmail = newEmail.ToLower();
                    user.EmailAddress = newEmail;
                    user.Genres = newGenres;
                    user.Intensity = newIntensity;
                    user.AgeCategory = newAgeCategory;

                    UpdateList(user);
                    CurrentUser = user;
                    return true;
                }
            }
            else
            {
                Console.WriteLine("Gebruiker bestaat niet.");
                Thread.Sleep(3000);
                return false;
            }
        }
    }
}