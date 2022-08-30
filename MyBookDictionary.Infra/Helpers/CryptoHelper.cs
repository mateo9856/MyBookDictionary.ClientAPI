using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MyBookDictionary.Infra.Helpers
{
    public class CryptoHelper
    {
        public static string EncodeToMD5(string text)
        {
            var TextToByte = Encoding.UTF8.GetBytes(text);

            using(var md5 = MD5.Create())
            {
                var hashByte = md5.ComputeHash(TextToByte);

                var ToHashString = BitConverter.ToString(hashByte).Replace("-", string.Empty);

                return ToHashString;
            }

        }
    }
}
