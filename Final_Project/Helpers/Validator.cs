using Final_Project.Exceptions;

namespace Final_Project.Helpers
{
    public static class Validator
    {
        private static readonly string[] BannedPasswords =
        {
            "123456", "password", "qwerty", "123456789", "abc123", "111111"
        };

        public static bool IsValidText(string? text)
        {
            return !string.IsNullOrWhiteSpace(text);
        }

        public static bool IsValidYear(int year)
        {
            return year >= 1000 && year <= DateTime.Now.Year;
        }

        public static bool IsValidFileSize(double size)
        {
            return size > 0 && size <= 5000;
        }

        public static string? GetUsernameError(string? username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return "სახელი ცარიელია!";

            if (username.Length < 4 || username.Length > 15)
                return "სახელი უნდა იყოს 4-დან 15 სიმბოლომდე!";

            if (!username.All(c => char.IsLetterOrDigit(c) || c == '_'))
                return "სახელში დაშვებულია მხოლოდ ასოები, ციფრები და _ !";

            if (username.All(char.IsDigit))
                return "სახელი არ შეიძლება იყოს მხოლოდ ციფრები!";

            return null;
        }

        public static string? GetPasswordError(string? username, string? password)
        {
            if (string.IsNullOrEmpty(password))
                return "პაროლი ცარიელია!";

            if (password.Length < 6)
                return "პაროლი უნდა იყოს მინიმუმ 6 სიმბოლო!";

            if (password.Contains(' '))
                return "პაროლში ინტერვალი არ შეიძლება!";

            if (!password.Any(char.IsLetter))
                return "პაროლში უნდა იყოს მინიმუმ ერთი ასო!";

            if (!password.Any(char.IsDigit))
                return "პაროლში უნდა იყოს მინიმუმ ერთი ციფრი!";

            if (username != null && password.Equals(username, StringComparison.OrdinalIgnoreCase))
                return "პაროლი არ უნდა ემთხვეოდეს სახელს!";

            if (BannedPasswords.Contains(password.ToLower()))
                return "ძალიან მარტივი პაროლია!";

            return null;
        }

        public static string? GetSecretWordError(string? word)
        {
            if (string.IsNullOrWhiteSpace(word))
                return "საიდუმლო სიტყვა ცარიელია!";

            if (word.Trim().Length < 3)
                return "საიდუმლო სიტყვა უნდა იყოს მინიმუმ 3 სიმბოლო!";

            return null;
        }

        public static void EnsureUsername(string? username)
        {
            string? error = GetUsernameError(username);

            if (error != null)
                throw new InvalidUsernameException(error);
        }

        public static void EnsurePassword(string? username, string? password)
        {
            string? error = GetPasswordError(username, password);

            if (error != null)
                throw new WeakPasswordException(error);
        }

        public static void EnsureSecretWord(string? word)
        {
            string? error = GetSecretWordError(word);

            if (error != null)
                throw new InvalidSecretWordException(error);
        }
    }
}