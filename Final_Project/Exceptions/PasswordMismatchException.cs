namespace Final_Project.Exceptions
{
    public class PasswordMismatchException : Exception
    {
        public PasswordMismatchException() : base("პაროლები არ ემთხვევა!")
        {
        }
    }
}