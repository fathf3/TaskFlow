using System.Text;
using TaskFlow.Application.Services;

namespace TaskFlow.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            using var sha256 = System.Security.Cryptography.SHA256.Create();
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password + "TaskFlowSalt"));
            return Convert.ToBase64String(hashedBytes);
        }

        public bool VerifyPassword(string password, string hash)
        {
            return HashPassword(password) == hash;
        }
    }

}
