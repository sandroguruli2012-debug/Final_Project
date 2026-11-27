namespace Final_Project.Exceptions
{
    public class WeakPasswordException : Exception
    {
        public WeakPasswordException(string message) : base(message)
        {
        }
    }
}