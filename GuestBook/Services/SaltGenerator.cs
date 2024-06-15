using System.Security.Cryptography;
using System.Text;

namespace GuestBook.Services
{
    public class SaltGenerator : ISaltGenerator
    {
        public string GenerateSalt()
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
    }
}
