namespace BioscoopReserveringsapplicatie
{
    public class RegistrationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public UserModel User { get; set; }
        public RegistrationResult(bool isValid, string errorMessage, UserModel user)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
            User = user;
        }
    }
}
