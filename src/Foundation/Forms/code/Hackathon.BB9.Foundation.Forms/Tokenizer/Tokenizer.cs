using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Hackathon.BB9.Foundation.Forms.Tokenizer
{
    /// <summary>
    /// This implementation should be in sync with the Sitecore solution's CipherService
    /// </summary>
    public static class Tokenizer
    {
        public static string Encrypt(string plainText, string password)
        {
            if (string.IsNullOrEmpty(plainText))
            {
                return string.Empty;
            }

            var bytesToBeEncrypted = Encoding.UTF8.GetBytes(plainText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var bytesEncrypted = Encrypt(bytesToBeEncrypted, passwordBytes);
            return Convert.ToBase64String(bytesEncrypted);
        }



        public static string Decrypt(string encryptedText, string password)
        {
            if (string.IsNullOrEmpty(encryptedText))
            {
                return string.Empty;
            }

            var bytesToBeDecrypted = Convert.FromBase64String(encryptedText);
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            var bytesDecrypted = Decrypt(bytesToBeDecrypted, passwordBytes);
            return Encoding.UTF8.GetString(bytesDecrypted);
        }



        private static byte[] Encrypt(byte[] bytesToBeEncrypted, byte[] passwordBytes)
        {
            byte[] encryptedBytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    SetupAES(AES, passwordBytes);

                    using (var cs = new CryptoStream(ms, AES.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeEncrypted, 0, bytesToBeEncrypted.Length);
                        cs.Close();
                    }

                    encryptedBytes = ms.ToArray();
                }
            }

            return encryptedBytes;
        }

        private static byte[] Decrypt(byte[] bytesToBeDecrypted, byte[] passwordBytes)
        {
            byte[] decryptedBytes = null;

            using (MemoryStream ms = new MemoryStream())
            {
                using (RijndaelManaged AES = new RijndaelManaged())
                {
                    SetupAES(AES, passwordBytes);

                    using (var cs = new CryptoStream(ms, AES.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(bytesToBeDecrypted, 0, bytesToBeDecrypted.Length);
                        cs.Close();
                    }

                    decryptedBytes = ms.ToArray();
                }
            }

            return decryptedBytes;
        }

        private static void SetupAES(RijndaelManaged AES, byte[] passwordBytes)
        {
            AES.KeySize = 256;
            AES.BlockSize = 128;
            AES.Key = passwordBytes.Take(AES.KeySize / 8).ToArray();
            AES.IV = passwordBytes.Take(AES.BlockSize / 8).ToArray();
            AES.Mode = CipherMode.CBC;
            AES.Padding = PaddingMode.PKCS7;
        }
    }
}