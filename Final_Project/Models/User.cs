using System.Text.Json.Serialization;
using Final_Project.Helpers;

namespace Final_Project.Models
{
    public class User
    {
        public string Username { get; private set; }
        public string PasswordHash { get; private set; }
        public string SecretWordHash { get; private set; }
        public DateTime RegisteredAt { get; private set; }

        [JsonConstructor]
        public User(string username, string passwordHash, string secretWordHash, DateTime registeredAt)
        {
            Username = username;
            PasswordHash = passwordHash;
            SecretWordHash = secretWordHash;
            RegisteredAt = registeredAt;
        }

        public bool CheckPassword(string password)
        {
            return PasswordHash == PasswordHasher.Hash(password);
        }

        public void ChangePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
        }
    }
}