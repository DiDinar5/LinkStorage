using System.Security.Cryptography;
using System.Text;

namespace LinkStorage.Safety
{
    /// <summary>
    /// Класс отвечающий за хеширование паролей(в скором времени добавиться хеш+соль)
    /// </summary>
    public class Hash
    {
        public static string HashPassword(string password)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                // ComputeHash - возращает массив байтов 
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                // Преобразование массива байтов в строку
                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();      
            }

        }
    }
}
