using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace myNote.ClientService
{
    internal static class PasswordCrypter
    {
        /// <summary>
        /// Counting md5 hash of password
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>
        public static byte[] GetPasswordMD5Hash(string password)
        {
            var md5 = new MD5Cng();
            return md5.ComputeHash(Encoding.UTF8.GetBytes(password));
        }

        /// <summary>
        /// Creating credential by login and crypted password
        /// </summary>
        /// <returns>User's Credential model</returns>
        public static Credential GetCredential(string login, string password)
        {
            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
                throw new ArgumentException("U didn't enter login and password");
            return new Credential { Login = login, Password = PasswordCrypter.GetPasswordMD5Hash(password) };
        }
    }
}
