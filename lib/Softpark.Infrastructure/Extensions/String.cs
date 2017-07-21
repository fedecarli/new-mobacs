using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Softpark.Infrastructure.Extensions
{
    /// <summary>
    /// Extensões de string
    /// </summary>
    public static class String
    {
        /// <summary>
        /// Hash de senha
        /// </summary>
        public static string PasswordHash { get; private set; } = ":cO^k5Fa%lGQpG<";
        /// <summary>
        /// Salto de hash
        /// </summary>
        public static string SaltKey { get; private set; } = "WQ0J5.Q8";
        /// <summary>
        /// Chave de verificação
        /// </summary>
        public static string VIKey { get; private set; } = "0X8c;e3KD<af.Hj^";

        /// <summary>
        /// Criptografa um texto
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Encrypt(this string plainText, string password = null)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            byte[] keyBytes = new Rfc2898DeriveBytes(password ?? PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.Zeros };
            var encryptor = symmetricKey.CreateEncryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));

            byte[] cipherTextBytes;

            using (var memoryStream = new MemoryStream())
            {
                using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    cipherTextBytes = memoryStream.ToArray();
                    cryptoStream.Close();
                }
                memoryStream.Close();
            }
            return Convert.ToBase64String(cipherTextBytes);
        }

        /// <summary>
        /// Decriptografa um texto
        /// </summary>
        /// <param name="encryptedText"></param>
        /// <param name="password"></param>
        /// <returns></returns>
        public static string Decrypt(this string encryptedText, string password = null)
        {
            byte[] cipherTextBytes = Convert.FromBase64String(encryptedText);
            byte[] keyBytes = new Rfc2898DeriveBytes(password ?? PasswordHash, Encoding.ASCII.GetBytes(SaltKey)).GetBytes(256 / 8);
            var symmetricKey = new RijndaelManaged() { Mode = CipherMode.CBC, Padding = PaddingMode.None };

            var decryptor = symmetricKey.CreateDecryptor(keyBytes, Encoding.ASCII.GetBytes(VIKey));
            var memoryStream = new MemoryStream(cipherTextBytes);
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainTextBytes = new byte[cipherTextBytes.Length];

            int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
            memoryStream.Close();
            cryptoStream.Close();
            return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount).TrimEnd("\0".ToCharArray());
        }
    }
}
