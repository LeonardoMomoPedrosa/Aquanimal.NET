using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

/// <summary>
/// Summary description for EcoUtils
/// </summary>
/// 

namespace eco.utils
{
    public class EcoUtils
    {
        public EcoUtils()
        {
        }
        public static bool isWet(int aId)
        {
            return aId == 8 || aId == 19 || aId == 20 || aId == 21;
        }

        public static string Sic(string plainText, string key)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.GenerateIV();
                aesAlg.Mode = CipherMode.CBC;

                using (var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV))
                {
                    byte[] encrypted = encryptor.TransformFinalBlock(Encoding.UTF8.GetBytes(plainText), 0, plainText.Length);
                    return Convert.ToBase64String(aesAlg.IV) + ":" + Convert.ToBase64String(encrypted);
                }
            }
        }

        public static string Asic(string cipherText, string key)
        {
            string[] parts = cipherText.Split(':');
            byte[] iv = Convert.FromBase64String(parts[0]);
            byte[] encryptedData = Convert.FromBase64String(parts[1]);

            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = Encoding.UTF8.GetBytes(key);
                aesAlg.IV = iv;
                aesAlg.Mode = CipherMode.CBC;

                using (var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV))
                {
                    byte[] decrypted = decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
                    return Encoding.UTF8.GetString(decrypted);
                }
            }
        }
    }

}