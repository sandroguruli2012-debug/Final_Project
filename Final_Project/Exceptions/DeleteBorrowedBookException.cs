namespace Final_Project.Exceptions
{
    public class DeleteBorrowedBookException : Exception
    {
        public DeleteBorrowedBookException(string title)
            : base($"წიგნი \"{title}\" გაცემულია, ამიტომ მის წაშლას ვერ შეძლებ.")
        {
        }
    }
}