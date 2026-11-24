namespace Final_Project.Exceptions
{
    public class DuplicateBookException : Exception
    {
        public DuplicateBookException(string title)
            : base($"წიგნი \"{title}\" უკვე არსებობს სიაში.")
        {
        }
    }
}