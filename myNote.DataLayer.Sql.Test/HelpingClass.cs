using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer.Sql.Test
{
    static class HelpingClass
    {
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";
        private static UsersRepository usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
        private static CredentialsRepository credentialsRepository = new CredentialsRepository(ConnectionString, usersRepository, new TokensRepository(ConnectionString));

        internal static User CreateUser()
        {
            var credential = GetCredential();
            credentialsRepository.Register(credential);
            return usersRepository.GetUser(credential.Login);
        }

        internal static Token GetToken()
        {
            return credentialsRepository.Login(GetCredential());
        }

        internal static Credential GetCredential()
        {
            return new Credential { Login = "TestUser", Password = HelpingClass.GetPassword() };
        }

        internal static byte[] GetPassword()
        {
            var md5 = new MD5Cng();
            var password = "TestPassword";
            return md5.ComputeHash(Encoding.ASCII.GetBytes(password));
        }
    }
}
