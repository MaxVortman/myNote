using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer.Sql.Test
{
    class CreatingUserClass
    {

        #region Private Members

        private const string ConnectionString = IoC.ConnectionString;
        private readonly string login;
        private readonly Credential credential;
        private static UsersRepository usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
        private static CredentialsRepository credentialsRepository = new CredentialsRepository(ConnectionString, usersRepository, new TokensRepository(ConnectionString));
        private Token token;
        private User user;

        #endregion

        #region Public Properties

        public Token Token { get { return token ?? (token = GetToken()); } }
        public User User { get { return user ?? (user = CreateUser()); } }

        #endregion

        #region Constructor

        public CreatingUserClass(string login)
        {
            this.login = login;
            credential = new Credential { Login = login, Password = GetPassword() };
        }

        #endregion

        #region Initial Properties Methods

        private User CreateUser()
        {
            credentialsRepository.Register(credential);
            return usersRepository.GetUser(credential.Login);
        }

        private Token GetToken()
        {
            return credentialsRepository.Login(credential);
        }

        #endregion

        #region Helping method

        internal static byte[] GetPassword()
        {
            var md5 = new MD5Cng();
            var password = "TestPassword";
            return md5.ComputeHash(Encoding.ASCII.GetBytes(password));
        }

        #endregion
    }
}
