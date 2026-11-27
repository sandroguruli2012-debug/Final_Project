namespace Final_Project.Exceptions
{
    public class NoBorrowedBooksException : Exception
    {
        public NoBorrowedBooksException() : base("შენ არ გაქვს აღებული წიგნები.")
        {
        }
    }
}