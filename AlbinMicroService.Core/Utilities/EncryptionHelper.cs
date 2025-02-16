using System.Security.Cryptography;
using System.Text;
using static AlbinMicroService.Core.Utilities.StaticProps;

namespace AlbinMicroService.Core.Utilities
{
    public class EncryptionHelper
    {
        // Example AES Key and IV (You should generate and store them securely)
        private static readonly string EncryptionKey = "1a2b3c4d5e6f7a8B9c0d1E2f3a4b5c6D"; // 32 bytes for AES-256
        private static readonly string IV = "5F4D3c2b1a0X9d8c"; // 16 bytes for AES-128

        private const string StaticEncryptionKey = "7g8h9i0j1K2l3m4n5o6p7q8r9S0t1u2V"; // New 32 bytes for AES-256
        private const string StaticIV = "1A2b3C4d5E6F7g8H"; // New 16 bytes for AES-128

        private static string GenerateNewString(string chars)
        {
            Random rand = new();

            // Create a character array to hold the resulting string
            char[] resultArray = new char[chars.Length];

            // Dynamically populate the resultArray with random characters from 'chars'
            for (int i = 0; i < resultArray.Length; i++)
            {
                resultArray[i] = chars[rand.Next(chars.Length)];
            }

            // Convert the char array into a string
            return new string(resultArray);
        }

        static EncryptionHelper()
        {
            if (!GlobalWebAppRunningMode.IsDev)
            {
                IV = GenerateNewString(IV);
                EncryptionKey = GenerateNewString(EncryptionKey);
            }

        }

        // Convert to Base64Url (for safe URL transmission)
        public static string ToBase64Url(string base64)
        {
            //The characters ==> { ?, &, #, and % } are not part of the Base64 character set by design.
            return base64.Replace('+', '-').Replace('/', '_').Replace("=", "");
        }

        // Convert back from Base64Url to Base64
        public static string FromBase64Url(string base64Url)
        {
            base64Url = base64Url.Replace('-', '+').Replace('_', '/');
            return string.Concat(base64Url, "==".AsSpan(0, (4 - base64Url.Length % 4) % 4)); // Padding to correct length
        }

        #region Encryption
        // Encrypt the plaintext using AES algorithm
        public static string EncryptData(string plainText)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey); // Ensure 32 bytes (256-bit key)
                aesAlg.IV = Encoding.UTF8.GetBytes(IV); // Ensure 16 bytes IV

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new())
                {
                    using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    // Convert to Base64 string, then apply Base64Url encoding
                    string base64 = Convert.ToBase64String(msEncrypt.ToArray());
                    return ToBase64Url(base64); // Apply Base64Url encoding
                }
            }
        }

        //aysnc method
        public static async Task<string> EncryptDataAsync(string plainText)
        {
            return await Task.Run(() =>
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey);
                    aesAlg.IV = Encoding.UTF8.GetBytes(IV);

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                    using (MemoryStream msEncrypt = new MemoryStream())
                    {
                        using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        string base64 = Convert.ToBase64String(msEncrypt.ToArray());
                        return ToBase64Url(base64);
                    }
                }
            });
        }

        /// <summary>
        /// Encrypts the plaintext using AES algorithm and returns the encrypted string.
        /// The output can be URL-safe or non-URL-safe based on the urlSafe parameter.
        /// </summary>
        /// <param name="plainText">The plaintext to be encrypted.</param>
        /// <param name="urlSafe">If true, the output will be URL-safe; otherwise, it will be a standard Base64 string.</param>
        /// <returns>The encrypted string.</returns>
        public static string EncryptData(string plainText, bool urlSafe)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(StaticEncryptionKey); // Ensure 32 bytes (256-bit key)
                aesAlg.IV = Encoding.UTF8.GetBytes(StaticIV); // Ensure 16 bytes IV

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                using (MemoryStream msEncrypt = new())
                {
                    using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                    using (StreamWriter swEncrypt = new(csEncrypt))
                    {
                        swEncrypt.Write(plainText);
                    }
                    string base64 = Convert.ToBase64String(msEncrypt.ToArray());
                    return urlSafe ? ToBase64Url(base64) : base64; // Apply Base64Url encoding if urlSafe is true
                }
            }
        }

        public static async Task<string> EncryptDataAsync(string plainText, bool urlSafe)
        {
            return await Task.Run(() =>
            {
                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(StaticEncryptionKey); // Ensure 32 bytes (256-bit key)
                    aesAlg.IV = Encoding.UTF8.GetBytes(StaticIV); // Ensure 16 bytes IV

                    ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msEncrypt = new())
                    {
                        using (CryptoStream csEncrypt = new(msEncrypt, encryptor, CryptoStreamMode.Write))
                        using (StreamWriter swEncrypt = new(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                        string base64 = Convert.ToBase64String(msEncrypt.ToArray());
                        return urlSafe ? ToBase64Url(base64) : base64; // Apply Base64Url encoding if urlSafe is true
                    }
                }
            });
        }
        #endregion

        #region Decryption

        // Decrypt the ciphertext back to plaintext
        public static string DecryptData(string base64UrlCipherText)
        {
            if (string.IsNullOrEmpty(base64UrlCipherText))
            {
                return "0";
            }
            try
            {
                // Convert from Base64Url to Base64
                string base64CipherText = FromBase64Url(base64UrlCipherText);

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey); // Ensure 32 bytes (256-bit key)
                    aesAlg.IV = Encoding.UTF8.GetBytes(IV); // Ensure 16 bytes IV

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msDecrypt = new MemoryStream(Convert.FromBase64String(base64CipherText))) // Convert Base64 back to byte array
                    {
                        using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd(); // Return decrypted plaintext
                        }
                    }
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                return "0";
            }
        }

        public static async Task<string> DecryptDataAsync(string base64UrlCipherText)
        {
            if (string.IsNullOrEmpty(base64UrlCipherText))
            {
                return "0";
            }
            try
            {
                // Convert from Base64Url to Base64
                string base64CipherText = FromBase64Url(base64UrlCipherText);

                return await Task.Run(() =>
                {
                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = Encoding.UTF8.GetBytes(EncryptionKey); // Ensure 32 bytes (256-bit key)
                        aesAlg.IV = Encoding.UTF8.GetBytes(IV); // Ensure 16 bytes IV

                        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                        using (MemoryStream msDecrypt = new(Convert.FromBase64String(base64CipherText))) // Convert Base64 back to byte array
                        {
                            using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
                            using (StreamReader srDecrypt = new(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd(); // Return decrypted plaintext
                            }
                        }
                    }
                });

            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                return "0";
            }
        }

        /// <summary>
        /// Decrypts the ciphertext back to plaintext using AES algorithm.
        /// The input can be URL-safe or non-URL-safe based on the urlSafe parameter.
        /// </summary>
        /// <param name="cipherText">The encrypted string to be decrypted.</param>
        /// <param name="urlSafe">If true, the input is expected to be URL-safe; otherwise, it will be a standard Base64 string.</param>
        /// <returns>The decrypted plaintext.</returns>
        public static string DecryptData(string cipherText, bool urlSafe)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return "0";
            }
            try
            {
                string base64CipherText = urlSafe ? FromBase64Url(cipherText) : cipherText;

                using (Aes aesAlg = Aes.Create())
                {
                    aesAlg.Key = Encoding.UTF8.GetBytes(StaticEncryptionKey); // Ensure 32 bytes (256-bit key)
                    aesAlg.IV = Encoding.UTF8.GetBytes(StaticIV); // Ensure 16 bytes IV

                    ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                    using (MemoryStream msDecrypt = new(Convert.FromBase64String(base64CipherText))) // Convert Base64 back to byte array
                    {
                        using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
                        using (StreamReader srDecrypt = new(csDecrypt))
                        {
                            return srDecrypt.ReadToEnd(); // Return decrypted plaintext
                        }
                    }
                }
            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                return "0";
            }
        }

        public static async Task<string> DecryptDataAsync(string cipherText, bool urlSafe)
        {
            if (string.IsNullOrEmpty(cipherText))
            {
                return "0";
            }
            try
            {
                string base64CipherText = urlSafe ? FromBase64Url(cipherText) : cipherText;
                return await Task.Run(() =>
                {
                    using (Aes aesAlg = Aes.Create())
                    {
                        aesAlg.Key = Encoding.UTF8.GetBytes(StaticEncryptionKey); // Ensure 32 bytes (256-bit key)
                        aesAlg.IV = Encoding.UTF8.GetBytes(StaticIV); // Ensure 16 bytes IV

                        ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);
                        using (MemoryStream msDecrypt = new(Convert.FromBase64String(base64CipherText))) // Convert Base64 back to byte array
                        {
                            using (CryptoStream csDecrypt = new(msDecrypt, decryptor, CryptoStreamMode.Read))
                            using (StreamReader srDecrypt = new(csDecrypt))
                            {
                                return srDecrypt.ReadToEnd(); // Return decrypted plaintext
                            }
                        }
                    }
                });

            }
            catch (Exception x)
            {
                Console.WriteLine(x);
                return "0";
            }
        }
        #endregion

    }
}
