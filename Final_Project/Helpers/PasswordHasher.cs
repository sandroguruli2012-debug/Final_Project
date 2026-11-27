using System.Security.Cryptography;
using System.Text;

namespace Final_Project.Helpers
{
    public static class PasswordHasher
    {
        public static string Hash(string text)
        {
            byte[] bytes = SHA256.HashData(Encoding.UTF8.GetBytes(text));

            return Convert.ToHexString(bytes);
        }
    }
}