using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myNote.Model;

namespace myNote.ClientService.Test
{
    [TestClass]
    public class UserServiceTest
    {
        const string ConnectionString = @"http://localhost:64625/api/";
        private LoginService loginClient = new LoginService(ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldGetOurUser()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var userService = new UserService(ConnectionString);
            var ourUser = userService.GetUserAsync(token.UserId, token).Result;
        }

        [TestMethod]
        public void ShouldGetEnemyUser()
        {
            //arrange

            //I
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            //enemy
            var enemyCredential = new Credential { Login = "Enemy", Password = PasswordCrypter.GetPasswordMD5Hash("degwd") };
            loginClient.Register(enemyCredential);
            var enemyToken = loginClient.LoginAsync(enemyCredential).Result;
            tempUser.Add(enemyCredential.Login, enemyToken);

            //act
            var userService = new UserService(ConnectionString);
            var emenyUser = userService.GetUserAsync(enemyToken.UserId, token).Result;
        }

        [TestMethod]
        public void ShouldGetUserGroups()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var userService = new UserService(ConnectionString);
            var ourGroup = userService.GetUserGroupsAsync(token.UserId, token).Result;
        }

        [TestMethod]
        public void ShouldUpdateUser()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var userService = new UserService(ConnectionString);
            var ourUser = userService.GetUserAsync(token.UserId, token).Result;
            ourUser.Name = "Marlock";
            ourUser = userService.UpdateUserAsync(ourUser, token).Result;
        }

        #region Clean Up temp

        [TestCleanup]
        public void CleanData()
        {
            foreach (var keyvalue in tempUser)
            {
                loginClient.Delete(keyvalue.Key, keyvalue.Value);
            }
        }

        #endregion
    }
}
