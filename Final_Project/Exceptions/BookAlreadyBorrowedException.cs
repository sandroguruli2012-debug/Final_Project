namespace Final_Project.Exceptions
{
    public class BookAlreadyBorrowedException : Exception
    {
        public BookAlreadyBorrowedException(string title, string username)
            : base($"წიგნი \"{title}\" უკვე გაცემულია მომხმარებელთან: {username}.")
        {
        }
    }
}