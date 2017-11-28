using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace myNote.WPFClient.ApiServices
{
    public static class PasswordCrypter
    {
        public static byte[] GetPasswordMD5Hash(string password)
        {
            var md5 = new MD5Cng();
            return md5.ComputeHash(Encoding.UTF8.GetBytes(password));
        }
    }
}
