using System.Security.Cryptography;
using System.Text;

namespace GuestBook.Helpers
{
    /// <summary>
    /// Утилиты для работы с паролями    
    /// </summary>
    public static class Utilities
    {
        /// <summary>
        /// статический метод, который будет генерировать соль и возвращать ее в виде строки
        /// </summary>
        /// <returns>Возвращает строку, представляющую соль, в шестнадцатеричном формате</returns>
        public static string GenerateSalt()
        {
            byte[] saltBytes = new byte[16]; // Создает массив байтов длиной 16, который будет хранить случайные байты соли

            RandomNumberGenerator randomNumberGenerator = RandomNumberGenerator.Create(); // Создает экземпляр генератора случайных чисел

            randomNumberGenerator.GetBytes(saltBytes); // Заполняет массив saltBytes случайными байтами

            StringBuilder stringBuilder = new StringBuilder(16); // Создает экземпляр StringBuilder для эффективного построения строки. Начальная емкость — 16 символов

            // Цикл, который проходит по каждому байту массива saltBytes
            for (int i = 0; i < 16; i++)
            {
                stringBuilder.Append(string.Format("{0:X2}", saltBytes[i])); // Преобразует каждый байт в шестнадцатеричный формат и добавляет его в stringBuilder
            }

            return stringBuilder.ToString(); // Возвращает строку, представляющую соль, в шестнадцатеричном формате
        }


        /// <summary>
        /// Хеширует пароль с использованием указанной соли
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <returns>Возвращает хеш в виде строки</returns>
        public static string HashPassword(string password, string salt)
        {
            byte[] passwordBytes = Encoding.Unicode.GetBytes(salt + password); // Преобразует комбинацию соли и пароля в массив байтов, используя кодировку Unicode

            var md5 = MD5.Create(); // Создает экземпляр MD5-хеш-функции

            byte[] hashBytes = md5.ComputeHash(passwordBytes); // Вычисляет MD5-хеш массива байтов passwordBytes

            StringBuilder hash = new StringBuilder(hashBytes.Length); // Создает экземпляр StringBuilder для построения строки хеша. Начальная емкость — длина массива hashBytes

            // Цикл, который проходит по каждому байту массива hashBytes
            for (int i = 0; i < hashBytes.Length; i++)
            {
                hash.Append(string.Format("{0:X2}", hashBytes[i])); // Преобразует каждый байт в шестнадцатеричный формат и добавляет его в hash
            }

            return hash.ToString(); // Возвращает строку, представляющую хеш, в шестнадцатеричном формате
        }
    }
}
