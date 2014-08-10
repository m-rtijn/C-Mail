using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.IO;

namespace C_Mail_2._0
{
    class EncryptionClass
    {
        // Okay I'm sorry I have no idea how this works, I just copy pasted it from StackOverflow because I can't
        // find a decent tutorial I understand. I did add the try/catch
        private static readonly byte[] initVectorBytes = Encoding.ASCII.GetBytes("tu89geji340t89u2");

        private const int keysize = 256;

        public static string Encrypt(string Plaintext, string Password)
        {
            try
            {
                byte[] plainTextBytes = Encoding.UTF8.GetBytes(Plaintext);
                using (PasswordDeriveBytes password = new PasswordDeriveBytes(Password, null))
                {
                    byte[] keyBytes = password.GetBytes(keysize / 8);
                    using (RijndaelManaged symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.Mode = CipherMode.CBC;
                        using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                        {
                            using (MemoryStream memoryStream = new MemoryStream())
                            {
                                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                                {
                                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                    cryptoStream.FlushFinalBlock();
                                    byte[] cipherTextBytes = memoryStream.ToArray();
                                    return Convert.ToBase64String(cipherTextBytes);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                // Create the ErrorMessage
                string ErrorMessage = "ERROR 40001:" + "\n" + exception.ToString();

                // Show the ErrorMessage to the user
                Program.ErrorPopupCall(ErrorMessage);

                // Stop executing this method
                return "";
            }
        }

        public static string Decrypt(string EncryptedText, string Password)
        {
            try
            {
                byte[] cipherTextBytes = Convert.FromBase64String(EncryptedText);
                using (PasswordDeriveBytes password = new PasswordDeriveBytes(Password, null))
                {
                    byte[] keyBytes = password.GetBytes(keysize / 8);
                    using (RijndaelManaged symmetricKey = new RijndaelManaged())
                    {
                        symmetricKey.Mode = CipherMode.CBC;
                        using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                        {
                            using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                            {
                                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                                {
                                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                                    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                // Create the ErrorMessage
                string ErrorMessage = "ERROR 40002:" + "\n" + exception.ToString();

                // Show the ErrorMessage to the user
                Program.ErrorPopupCall(ErrorMessage);

                // Stop executing this method
                return "";
            } 
        }
    }
}
