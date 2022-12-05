using System.Security.Cryptography;
using System.Text;

namespace Domain.Helpers
{
    public static class Security
    {
        public static string EncryptPasswordWithSHA256(this string password)
        {
            byte[] bytes = Encoding.Unicode.GetBytes(password);
            var hashstring = SHA256.Create();
            byte[] hash = hashstring.ComputeHash(bytes);
            string hashString = string.Empty;

            foreach (byte x in hash)
            {
                hashString += String.Format("{0:x2}", x);
            }

            return hashString;
        }
    }
}
