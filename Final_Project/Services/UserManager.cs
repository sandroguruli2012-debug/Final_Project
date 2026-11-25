using Final_Project.Models;

namespace Final_Project.Services
{
    public class UserManager
    {
        private readonly List<User> _users = new List<User>();

        public async Task RegisterAsync(string username, string password)
        {
            await Task.Delay(300);

            bool exists = _users.Any(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (exists)
                throw new Exception("ასეთი მომხმარებელი უკვე არსებობს!");

            _users.Add(new User(username, password));
        }

        public async Task<bool> LoginAsync(string username, string password)
        {
            await Task.Delay(300);

            return _users.Any(u =>
                u.Username == username && u.Password == password);
        }
    }
}