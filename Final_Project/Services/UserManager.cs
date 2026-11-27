using Final_Project.Exceptions;
using Final_Project.Helpers;
using Final_Project.Models;

namespace Final_Project.Services
{
    public class UserManager
    {
        private const string FileName = "users.json";

        private readonly List<User> _users = new List<User>();

        public User? CurrentUser { get; private set; }

        public async Task LoadAsync()
        {
            List<User> loaded = await JsonHelper.LoadListAsync<User>(FileName);

            _users.Clear();
            _users.AddRange(loaded);
        }

        public async Task RegisterAsync(string username, string password, string secretWord)
        {
            await Task.Delay(300);

            Validator.EnsureUsername(username);
            Validator.EnsurePassword(username, password);
            Validator.EnsureSecretWord(secretWord);

            bool exists = _users.Any(u =>
                u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));

            if (exists)
                throw new UserAlreadyExistsException(username);

            User user = new User(
                username,
                PasswordHasher.Hash(password),
                PasswordHasher.Hash(NormalizeSecret(secretWord)),
                DateTime.Now);

            _users.Add(user);
            await SaveAsync();
        }

        public async Task LoginAsync(string username, string password)
        {
            await Task.Delay(300);

            User? user = _users.FirstOrDefault(u =>
                u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase));

            if (user == null || !user.CheckPassword(password))
                throw new InvalidCredentialsException();

            CurrentUser = user;
        }

        public async Task VerifySecretWordAsync(string username, string secretWord)
        {
            await Task.Delay(300);

            User user = FindUser(username);

            CheckSecretWord(user, secretWord);
        }

        public async Task ResetPasswordAsync(string username, string secretWord, string newPassword)
        {
            await Task.Delay(300);

            User user = FindUser(username);

            CheckSecretWord(user, secretWord);
            Validator.EnsurePassword(user.Username, newPassword);

            user.ChangePassword(PasswordHasher.Hash(newPassword));
            await SaveAsync();
        }

        public void Logout()
        {
            CurrentUser = null;
        }

        private User FindUser(string username)
        {
            User? user = _users.FirstOrDefault(u =>
                u.Username.Equals(username.Trim(), StringComparison.OrdinalIgnoreCase));

            if (user == null)
                throw new UserNotFoundException(username);

            return user;
        }

        private static void CheckSecretWord(User user, string secretWord)
        {
            string hash = PasswordHasher.Hash(NormalizeSecret(secretWord));

            if (user.SecretWordHash != hash)
                throw new WrongSecretWordException();
        }

        private static string NormalizeSecret(string secretWord)
        {
            return secretWord.Trim().ToLowerInvariant();
        }

        private async Task SaveAsync()
        {
            await JsonHelper.SaveAsync(FileName, _users);
        }
    }
}