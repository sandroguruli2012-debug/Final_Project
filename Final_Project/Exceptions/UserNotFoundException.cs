namespace Final_Project.Exceptions
{
    public class UserNotFoundException : Exception
    {
        public UserNotFoundException(string username)
            : base($"მომხმარებელი \"{username}\" ვერ მოიძებნა!")
        {
        }
    }
}