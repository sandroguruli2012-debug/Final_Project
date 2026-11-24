namespace Final_Project.Exceptions
{
    public class BookNotFoundException : Exception
    {
        public BookNotFoundException(string title)
            : base($"წიგნი \"{title}\" ვერ მოიძებნა.")
        {
        }
    }
}