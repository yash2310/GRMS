namespace AMS.ApplicationCore.Utilities
{
    #region Namespaces

    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Text;

    #endregion

    public class SecurityManager
    {
        const string Password = "AppraisalManagementSystem";
        public static string Encrypt(string text)
        {
            var rijndaelCipher = new RijndaelManaged();
            byte[] plainText = Encoding.Unicode.GetBytes(text);
            byte[] salt = Encoding.ASCII.GetBytes(Password.Length.ToString());
            var secretKey = new PasswordDeriveBytes(Password, salt);

            //Creates a symmetric encryptor object.
            ICryptoTransform encryptor = rijndaelCipher.CreateEncryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
            var memoryStream = new MemoryStream();
            //Defines a stream that links data streams to cryptographic transformations
            var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write);
            cryptoStream.Write(plainText, 0, plainText.Length);
            //Writes the final state and clears the buffer
            cryptoStream.FlushFinalBlock();
            byte[] cipherBytes = memoryStream.ToArray();
            memoryStream.Close();
            cryptoStream.Close();
            string encryptedData = Convert.ToBase64String(cipherBytes);

            return encryptedData;

        }

        public static string Decrypt(string text)
        {

            var rijndaelCipher = new RijndaelManaged();
            byte[] encryptedData = Convert.FromBase64String(text);
            byte[] salt = Encoding.ASCII.GetBytes(Password.Length.ToString());

            //Making of the key for decryption
            var secretKey = new PasswordDeriveBytes(Password, salt);

            //Creates a symmetric Rijndael decryptor object.
            ICryptoTransform decryptor = rijndaelCipher.CreateDecryptor(secretKey.GetBytes(32), secretKey.GetBytes(16));
            var memoryStream = new MemoryStream(encryptedData);

            //Defines the cryptographics stream for decryption.THe stream contains decrpted data
            var cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read);
            byte[] plainText = new byte[encryptedData.Length];
            int decryptedCount = cryptoStream.Read(plainText, 0, plainText.Length);
            memoryStream.Close();
            cryptoStream.Close();

            //Converting to string
            string decryptedData = Encoding.Unicode.GetString(plainText, 0, decryptedCount);

            return decryptedData;
        }

        public static string DecryptStringAES(string cipherText)
        {
            var keybytes = Encoding.UTF8.GetBytes("7061737323313233");
            var iv = Encoding.UTF8.GetBytes("7061737323313233");

            var encrypted = Convert.FromBase64String(cipherText);
            var decriptedFromJavascript = DecryptStringFromBytes(encrypted, keybytes, iv);
            return string.Format(decriptedFromJavascript);
        }

        private static string DecryptStringFromBytes(byte[] cipherText, byte[] key, byte[] iv)
        {
            // Check arguments.  
            if (cipherText == null || cipherText.Length <= 0)
            {
                throw new ArgumentNullException("cipherText");
            }
            if (key == null || key.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }
            if (iv == null || iv.Length <= 0)
            {
                throw new ArgumentNullException("key");
            }

            // Declare the string used to hold  
            // the decrypted text.  
            string plaintext = null;

            // Create an RijndaelManaged object  
            // with the specified key and IV.  
            using (var rijAlg = new RijndaelManaged())
            {
                //Settings  
                rijAlg.Mode = CipherMode.CBC;
                rijAlg.Padding = PaddingMode.PKCS7;
                rijAlg.FeedbackSize = 128;

                rijAlg.Key = key;
                rijAlg.IV = iv;

                // Create a decrytor to perform the stream transform.  
                var decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                try
                {
                    // Create the streams used for decryption.  
                    using (var msDecrypt = new MemoryStream(cipherText))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {

                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                // Read the decrypted bytes from the decrypting stream  
                                // and place them in a string.  
                                plaintext = srDecrypt.ReadToEnd();

                            }

                        }
                    }
                }
                catch
                {
                    plaintext = "keyError";
                }
            }
            return plaintext;
        }
    }
}
