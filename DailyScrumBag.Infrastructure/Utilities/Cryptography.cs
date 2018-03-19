using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace DailyScrumBag.Infrastructure.Utilities
{
    public static class Cryptography
    {
        #region Encryption/ Decryption

        /// <summary>
        /// Decrypts the specified value.
        /// </summary>
        /// <param name="value">The decryption value of type string</param>
        /// <returns>Decrypted value</returns>
        public static string Decrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            string result = null;
            using (var rijndael = RijndaelCrypto())
            {
                // Create a decrytor to perform the stream transform. 
                ICryptoTransform decryptor = rijndael.CreateDecryptor(rijndael.Key, rijndael.IV);

                // Create the streams used for decryption.                 
                byte[] bytes = Convert.FromBase64String(value.Trim());

                using (var msDecrypt = new MemoryStream(bytes))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream and place them in a string. 
                            result = srDecrypt.ReadToEnd();
                        }
                    }
                }
            }

            // Return the decrypted bytes from the memory stream. 
            return result;
        }

        /// <summary>
        /// Encrypts the specified value.
        /// </summary>
        /// <param name="value">The encryption value of type string</param>
        /// <returns>Encrypted value</returns>
        public static string Encrypt(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            string result = null;
            using (var rijndael = RijndaelCrypto())
            {
                // Create a decryptor to perform the stream transform. 
                ICryptoTransform encryptor = rijndael.CreateEncryptor(rijndael.Key, rijndael.IV);

                // Create the streams used for encryption. 
                using (var memoryStream = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            // Write all data to the stream. 
                            swEncrypt.Write(value);
                        }
                    }

                    result = Convert.ToBase64String(memoryStream.ToArray());
                }
            }

            // Return the encrypted bytes from the memory stream. 
            return result;
        }

        #endregion

        #region Private/ Helper Method

        /// <summary>
        /// Method to initialisation of Rijndael managed algorithm
        /// </summary>
        /// <returns>Rijndael managed alogrithm</returns>
        private static RijndaelManaged RijndaelCrypto()
        {
            const string ENCRYPTION_SALT = "E!p@r@$2%18";
            const string ENCRYPTION_PASSWORD = "D@ilyScrumB@g-Enterpri$e";
            var salt = Encoding.ASCII.GetBytes(ENCRYPTION_SALT);

            // Generate the key from the shared secret and the salt key 
            var key = new Rfc2898DeriveBytes(ENCRYPTION_PASSWORD, salt);

            // Create a RijndaelManaged object with the specified key and IV. 
            var rijndael = new RijndaelManaged();
            rijndael.Key = key.GetBytes(rijndael.KeySize / 8);
            rijndael.IV = key.GetBytes(rijndael.BlockSize / 8);
            return rijndael;
        }

        #endregion
    }
}
