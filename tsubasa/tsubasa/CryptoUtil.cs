using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace tsubasa
{
    public static class CryptoUtil
    {
        public static byte[] Cbc(byte[] toEncrypt,byte[] key, int blockSize, byte[] offset,PaddingMode padding = PaddingMode.Zeros)
        {
            using (Aes aes = Aes.Create())
            {
                //aes.BlockSize = blockSize;
                aes.Mode = CipherMode.CBC;
                aes.Padding = padding;
                aes.Key = key;
                aes.IV = offset;
                ICryptoTransform encryptor = aes.CreateEncryptor();
                byte[] resultArray = encryptor.TransformFinalBlock(toEncrypt, 0, toEncrypt.Length);
                return resultArray;
            }
        }

        public static byte[] Cbc(string toEncrypt,string key, int blockSize, string offset, PaddingMode padding = PaddingMode.Zeros)
        {
            using (Aes aes = Aes.Create())
            {
                return Cbc(Encoding.UTF8.GetBytes(toEncrypt), Encoding.UTF8.GetBytes(key), blockSize, Encoding.UTF8.GetBytes(offset), padding); 
            }
        }

        public static string Base64Cbc(byte[] toEncrypt, byte[] key, int blockSize, byte[] offset, PaddingMode padding = PaddingMode.Zeros)
        {
            return Convert.ToBase64String(Cbc(toEncrypt,key, blockSize, offset, padding));
        }

        public static string Base64Cbc(string toEncrypt, string key, int blockSize, string offset, PaddingMode padding = PaddingMode.Zeros)
        {
            return Convert.ToBase64String(Cbc(toEncrypt,key, blockSize, offset, padding));
        }
    }
}
