namespace Final_Project.Exceptions
{
    public class NoAvailableBooksException : Exception
    {
        public NoAvailableBooksException() : base("თავისუფალი წიგნები არ არის.")
        {
        }
    }
}