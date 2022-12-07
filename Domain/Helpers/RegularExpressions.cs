namespace Domain.Helpers
{
    public static class RegularExpressions
    {
        public static class Email
        {
            public const string Pattern = @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$";
            public const string ErrorMessage = "Invalid e-mail!";
        }

        public static class Password
        {
            public const string Pattern = @"((?=.*\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\W]).{8,64})";
            public const string ErrorMessage = "Password invalid! Ensure that password is 8 to 64 characters long and contains a mix of upper and lower case characters, one numeric and one special character.";
        }

        public static class IpAdress
        {
            public const string Pattern = @"^(2[0-5]{2}\.|1?[0-9]{1,2}\.){3}(2[0-5]{2}|1?[0-9]{1,2})$";
            public const string ErrorMessage = "Invalid IP adress!";
        }
    }
}