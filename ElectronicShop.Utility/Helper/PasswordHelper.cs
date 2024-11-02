using System.Security.Cryptography;
using System.Text;

namespace ElectronicShop.Common.Helper
{
    public class PasswordHelper
    {
        private static string key = "nhumaidao";

        public static string Encrypt(string toEncrypt)
        {
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = UTF8Encoding.UTF8.GetBytes(toEncrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateEncryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return Convert.ToBase64String(resultArray, 0, resultArray.Length);
        }

        public static string Decrypt(string toDecrypt)
        {
            bool useHashing = true;
            byte[] keyArray;
            byte[] toEncryptArray = Convert.FromBase64String(toDecrypt);

            if (useHashing)
            {
                MD5CryptoServiceProvider hashmd5 = new MD5CryptoServiceProvider();
                keyArray = hashmd5.ComputeHash(UTF8Encoding.UTF8.GetBytes(key));
            }
            else
                keyArray = UTF8Encoding.UTF8.GetBytes(key);

            TripleDESCryptoServiceProvider tdes = new TripleDESCryptoServiceProvider();
            tdes.Key = keyArray;
            tdes.Mode = CipherMode.ECB;
            tdes.Padding = PaddingMode.PKCS7;

            ICryptoTransform cTransform = tdes.CreateDecryptor();
            byte[] resultArray = cTransform.TransformFinalBlock(toEncryptArray, 0, toEncryptArray.Length);

            return UTF8Encoding.UTF8.GetString(resultArray);
        }

        public static string CreateRandomPassword(int length = 8)
        {
            string lowerChars = "abcdefghijklmnopqrstuvwxyz";
            string uppercaseChars = "ABCDEFGHJKLMNOPQRSTUVWXYZ";
            string numericChars = "0123456789";
            string specChars = "!@#$%^&*?_-";

            Random random = new Random();

            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = lowerChars[random.Next(0, lowerChars.Length)];
            }
            var uppercaseIndex = random.Next(0, length);
            var numericIndex = 1;
            var specIndex = 1;
            while (numericIndex == uppercaseIndex)
            {
                numericIndex = random.Next(0, length);
            }

            while (specIndex == uppercaseIndex || specIndex == numericIndex)
            {
                specIndex = random.Next(0, length);
            }

            chars[uppercaseIndex] = uppercaseChars[random.Next(0, uppercaseChars.Length)];
            chars[numericIndex] = numericChars[random.Next(0, numericChars.Length)];
            chars[specIndex] = specChars[random.Next(0, specChars.Length)];

            return new string(chars);
        }
    }
}