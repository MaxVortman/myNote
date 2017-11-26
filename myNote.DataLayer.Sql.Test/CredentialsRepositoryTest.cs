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
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";
        private readonly List<string> tempUsersLogin = new List<string>();

        [TestMethod]
        public void ShouldRegister()
        {
            //arrange
            var credential = new Credential { Login = "TestLogin", Password = HelpingClass.GetPassword() };
            tempUsersLogin.Add(credential.Login);

            //act
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            var credentialsRepository = new CredentialsRepository(ConnectionString, usersRepository, new TokensRepository(ConnectionString));
            credentialsRepository.Register(credential);
            var token = credentialsRepository.Login(credential);
            var userFromDb = usersRepository.GetUser(credential.Login);

            //asserts
            Assert.AreEqual(credential.Login, userFromDb.Login);
        }

        [TestMethod]
        public void ShouldLogin()
        {
            //arrange
            var credential = new Credential { Login = "TestLogin", Password = HelpingClass.GetPassword() };
            tempUsersLogin.Add(credential.Login);
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            var credentialsRepository = new CredentialsRepository(ConnectionString, usersRepository, new TokensRepository(ConnectionString));
            credentialsRepository.Register(credential);

            //act
            var token = credentialsRepository.Login(credential);
            var userFromDb = usersRepository.GetUser(credential.Login);

            //asserts
            Assert.AreEqual(credential.Login, userFromDb.Login);
        }        

        [TestCleanup]
        public void CleanData()
        {
            var credentialsRepository = new CredentialsRepository(ConnectionString, new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString)), new TokensRepository(ConnectionString));
            foreach (var login in tempUsersLogin)
                credentialsRepository.Delete(login);
        }
    }
}
