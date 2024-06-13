using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace ChefHesab.Share.Extiontions
{
    public class ConfigurationBasedStringEncrypter
    {
        private static readonly ICryptoTransform Encrypter;

        private static readonly ICryptoTransform Decrypter;

        private static readonly string _prefix;

        public static string Prefix => _prefix;

        static ConfigurationBasedStringEncrypter()
        {
            string s = ConfigurationManager.AppSettings["EncryptionKey"];
            string strA = ConfigurationManager.AppSettings["UseHashingForEncryption"];
            bool flag = true;
            if (string.Compare(strA, "false", ignoreCase: true) == 0)
            {
                flag = false;
            }

            _prefix = ConfigurationManager.AppSettings["EncryptionPrefix"];
            if (string.IsNullOrWhiteSpace(_prefix))
            {
                _prefix = "encryptedHidden_";
            }

            byte[] key;
            if (flag)
            {
                MD5CryptoServiceProvider mD5CryptoServiceProvider = new MD5CryptoServiceProvider();
                key = mD5CryptoServiceProvider.ComputeHash(Encoding.UTF8.GetBytes(s));
                mD5CryptoServiceProvider.Clear();
            }
            else
            {
                key = Encoding.UTF8.GetBytes(s);
            }

            TripleDESCryptoServiceProvider tripleDESCryptoServiceProvider = new TripleDESCryptoServiceProvider
            {
                Key = key,
                Mode = CipherMode.ECB,
                Padding = PaddingMode.PKCS7
            };
            Encrypter = tripleDESCryptoServiceProvider.CreateEncryptor();
            Decrypter = tripleDESCryptoServiceProvider.CreateDecryptor();
            tripleDESCryptoServiceProvider.Clear();
        }

        //
        // Parameters:
        //   value:
        public static string Encrypt(string value)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(value);
            byte[] inArray = Encrypter.TransformFinalBlock(bytes, 0, bytes.Length);
            return Convert.ToBase64String(inArray);
        }

        //
        // Parameters:
        //   value:
        public static string Decrypt(string value)
        {
            byte[] array = Convert.FromBase64String(value);
            byte[] bytes = Decrypter.TransformFinalBlock(array, 0, array.Length);
            return Encoding.UTF8.GetString(bytes);
        }
    }
}
