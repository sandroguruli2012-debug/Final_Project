namespace Final_Project.Exceptions
{
    public class InvalidCredentialsException : Exception
    {
        public InvalidCredentialsException() : base("მომხმარებელი ან პაროლი არასწორია!")
        {
        }
    }
}