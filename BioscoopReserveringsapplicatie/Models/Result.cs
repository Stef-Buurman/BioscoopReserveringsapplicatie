﻿namespace BioscoopReserveringsapplicatie
{
    public class Result<T>
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; }
        public T? Item { get; set; }
        public Result(bool isValid, string errorMessage, T? item)
        {
            IsValid = isValid;
            ErrorMessage = errorMessage;
            Item = item;
        }

        public Result(bool isValid, string errorMessage) : this(isValid, errorMessage, default)
        {}

        public Result(bool isValid) : this(isValid, "", default)
        { }
    }
}
