using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace BasicExtensions
{
    public static class EncryptionExtensions
    {
        private static byte[] GetRandomBytes()
        {
            try
            {
                byte[] data = new byte[16];
                using (RNGCryptoServiceProvider cryptoServiceProvider = new RNGCryptoServiceProvider())
                {
                    cryptoServiceProvider.GetBytes(data);
                    return data;
                }
            }
            catch
            {
                return new byte[16]
                {
          (byte) 202,
          (byte) 28,
          (byte) 223,
          (byte) 164,
          (byte) 177,
          (byte) 123,
          (byte) 212,
          (byte) 124,
          (byte) 132,
          (byte) 124,
          (byte) 52,
          (byte) 12,
          (byte) 6,
          (byte) 12,
          (byte) 45,
          (byte) 124
                };
            }
        }

        public static string EncryptText(this string text, string key = "")
        {
            byte[] randomBytes1 = EncryptionExtensions.GetRandomBytes();
            byte[] randomBytes2 = EncryptionExtensions.GetRandomBytes();
            byte[] bytes1 = Encoding.UTF8.GetBytes(text);
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(key, randomBytes1, 1000))
            {
                byte[] bytes2 = rfc2898DeriveBytes.GetBytes(16);
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.BlockSize = 128;
                    rijndaelManaged.Mode = CipherMode.CBC;
                    rijndaelManaged.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform encryptor = rijndaelManaged.CreateEncryptor(bytes2, randomBytes2))
                    {
                        using (MemoryStream memoryStream = new MemoryStream())
                        {
                            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(bytes1, 0, bytes1.Length);
                                cryptoStream.FlushFinalBlock();
                                byte[] array = ((IEnumerable<byte>)((IEnumerable<byte>)randomBytes1).Concat<byte>((IEnumerable<byte>)randomBytes2).ToArray<byte>()).Concat<byte>((IEnumerable<byte>)memoryStream.ToArray()).ToArray<byte>();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(array);
                            }
                        }
                    }
                }
            }
        }

        public static string DecryptText(this string text, string key = "")
        {
            byte[] source = Convert.FromBase64String(text);
            byte[] array1 = ((IEnumerable<byte>)source).Take<byte>(16).ToArray<byte>();
            byte[] array2 = ((IEnumerable<byte>)source).Skip<byte>(16).Take<byte>(16).ToArray<byte>();
            byte[] array3 = ((IEnumerable<byte>)source).Skip<byte>(32).Take<byte>(source.Length - 32).ToArray<byte>();
            using (Rfc2898DeriveBytes rfc2898DeriveBytes = new Rfc2898DeriveBytes(key, array1, 1000))
            {
                byte[] bytes = rfc2898DeriveBytes.GetBytes(16);
                using (RijndaelManaged rijndaelManaged = new RijndaelManaged())
                {
                    rijndaelManaged.BlockSize = 128;
                    rijndaelManaged.Mode = CipherMode.CBC;
                    rijndaelManaged.Padding = PaddingMode.PKCS7;
                    using (ICryptoTransform decryptor = rijndaelManaged.CreateDecryptor(bytes, array2))
                    {
                        using (MemoryStream memoryStream = new MemoryStream(array3))
                        {
                            using (CryptoStream cryptoStream = new CryptoStream((Stream)memoryStream, decryptor, CryptoStreamMode.Read))
                            {
                                byte[] numArray = new byte[array3.Length];
                                int count = cryptoStream.Read(numArray, 0, numArray.Length);
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Encoding.UTF8.GetString(numArray, 0, count);
                            }
                        }
                    }
                }
            }
        }

        public static string CreateMD5(this string input)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] bytes = Encoding.ASCII.GetBytes(input);
                byte[] hash = md5.ComputeHash(bytes);
                StringBuilder stringBuilder = new StringBuilder();
                for (int index = 0; index < hash.Length; ++index)
                    stringBuilder.Append(hash[index].ToString("X2"));
                return stringBuilder.ToString();
            }
        }
    }
}
