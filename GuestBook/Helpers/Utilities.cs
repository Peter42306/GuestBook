using System.Security.Cryptography;
using System.Text;

namespace GuestBook.Helpers
{
    public static class Utilities
    {
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16];
            RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create();
            randomNumberGenerator.GetBytes(saltBytes);

            StringBuilder stringBuilder = new StringBuilder(16);

            for (int i = 0; i < 16; i++)
            {
                stringBuilder.Append(string.Format("{0:X2}", saltBytes[i]));
            }

            return stringBuilder.ToString();
        }

        public static string HashPassword(string password, string salt)
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
