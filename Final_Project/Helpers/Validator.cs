namespace Final_Project.Helpers
{
    public static class Validator
    {
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
    }
}