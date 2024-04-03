namespace BioscoopReserveringsapplicatie
{
    public class RegistrationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public RegistrationResult(bool isValid, string errorMessage)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
        }
    }
}
