namespace GameStore.BusinessLayer.Models
{
    public static class PasswordValidationErrors
    {
        public const string MinLength = "Password should contain at least 8 symbols";
        public const string ShouldContainDigit = "Password should contain at least one digit";
        public const string ShouldContainLowerSymbol = "Password should contain at least one lower case symbol";
        public const string ShouldContainUpperSymbol = "Password should contain at least one upper case symbol";
    }
}