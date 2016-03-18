using System;
using System.Configuration;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Silicus.ProjectTracker.Core
{
    public static class EncryptionHelper
    {
        public static string EncryptString(string clearText)
        {
            byte[] clearTextBytes = Encoding.UTF8.GetBytes(clearText);

            SymmetricAlgorithm rijn = SymmetricAlgorithm.Create();

            var ms = new MemoryStream();
            byte[] rgbIv = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["encryption_salt"]);
            byte[] key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["encryption_key"]);
            var cs = new CryptoStream(ms, rijn.CreateEncryptor(key, rgbIv), CryptoStreamMode.Write);

            cs.Write(clearTextBytes, 0, clearTextBytes.Length);

            cs.Close();

            return Convert.ToBase64String(ms.ToArray());
        }

        public static string DecryptString(string encryptedText)
        {
            byte[] encryptedTextBytes = Convert.FromBase64String(encryptedText);

            var ms = new MemoryStream();

            var rijn = SymmetricAlgorithm.Create();


            byte[] rgbIv = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["encryption_salt"]);
            byte[] key = Encoding.ASCII.GetBytes(ConfigurationManager.AppSettings["encryption_key"]);

            var cs = new CryptoStream(ms, rijn.CreateDecryptor(key, rgbIv), CryptoStreamMode.Write);
            cs.Write(encryptedTextBytes, 0, encryptedTextBytes.Length);
            cs.Close();

            return Encoding.UTF8.GetString(ms.ToArray());
        }
    }
}
