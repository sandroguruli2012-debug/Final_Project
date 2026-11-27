namespace Final_Project.Exceptions
{
    public class UserAlreadyExistsException : Exception
    {
        public UserAlreadyExistsException(string username)
            : base($"მომხმარებელი \"{username}\" უკვე არსებობს!")
        {
        }
    }
}