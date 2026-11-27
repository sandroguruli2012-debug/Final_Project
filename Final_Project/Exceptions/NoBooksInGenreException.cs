namespace Final_Project.Exceptions
{
    public class NoBooksInGenreException : Exception
    {
        public NoBooksInGenreException(string genre)
            : base($"ჟანრში \"{genre}\" წიგნები ვერ მოიძებნა.")
        {
        }
    }
}