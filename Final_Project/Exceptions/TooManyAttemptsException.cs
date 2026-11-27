namespace Final_Project.Exceptions
{
    public class TooManyAttemptsException : Exception
    {
        public TooManyAttemptsException() : base("ძალიან ბევრი წარუმატებელი მცდელობა!")
        {
        }
    }
}