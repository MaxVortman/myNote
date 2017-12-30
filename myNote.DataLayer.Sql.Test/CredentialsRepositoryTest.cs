using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myNote.Model;

namespace myNote.DataLayer.Sql.Test
{
    [TestClass]
    public class CredentialsRepositoryTest
    {
        private const string ConnectionString = IoC.ConnectionString;
        private readonly Dictionary<string, Token> tempUsersLogin = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldRegister()
        {
            //arrange
            var credential = new Credential { Login = "TestLogin", Password = CreatingUserClass.GetPassword() };

            //act
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            var credentialsRepository = new CredentialsRepository(ConnectionString, usersRepository, new TokensRepository(ConnectionString));
            credentialsRepository.Register(credential);
            var token = credentialsRepository.Login(credential);
            tempUsersLogin.Add(credential.Login, token);
            var userFromDb = usersRepository.GetUser(credential.Login);

            //asserts
            Assert.AreEqual(credential.Login, userFromDb.Login);
        }

        [TestMethod]
        public void ShouldLogin()
        {
            //arrange
            var credential = new Credential { Login = "TestLogin", Password = CreatingUserClass.GetPassword() };
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            var credentialsRepository = new CredentialsRepository(ConnectionString, usersRepository, new TokensRepository(ConnectionString));
            credentialsRepository.Register(credential);

            //act
            var token = credentialsRepository.Login(credential);
            tempUsersLogin.Add(credential.Login, token);
            var userFromDb = usersRepository.GetUser(credential.Login);

            //asserts
            Assert.AreEqual(credential.Login, userFromDb.Login);
        }

        [TestCleanup]
        public void CleanData()
        {
            var credentialsRepository = new CredentialsRepository(ConnectionString, new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString)), new TokensRepository(ConnectionString));
            foreach (var keyvalue in tempUsersLogin)
                credentialsRepository.Delete(keyvalue.Key, keyvalue.Value);
        }
    }
}
