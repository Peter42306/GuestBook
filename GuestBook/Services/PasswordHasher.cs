using System.Security.Cryptography;
using System.Text;

namespace GuestBook.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password, string salt)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(salt + password);

            var md5 = MD5.Create();

            byte[] hashBytes = md5.ComputeHash(passwordBytes);

            StringBuilder hash = new StringBuilder(hashBytes.Length);

            for (int i = 0; i < hashBytes.Length; i++)
            {
                hash.Append(string.Format("{0:X2}", hashBytes[i]));
            }

            return hash.ToString();
        }
    }
}
