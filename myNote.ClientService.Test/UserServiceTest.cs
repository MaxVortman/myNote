using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myNote.Model;

namespace myNote.ClientService.Test
{
    [TestClass]
    public class UserServiceTest
    {
        private ApiClient api = ApiClient.CreateInstance(IoC.ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldGetOurUser()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            
            //act
            var ourUser = api.UserService.GetUserAsync(token.UserId, token).Result;
        }

        [TestMethod]
        public void ShouldGetEnemyUser()
        {
            //arrange

            //I
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            //enemy
            var enemyCredential = new Credential { Login = "Enemy", Password = PasswordCrypter.GetPasswordMD5Hash("degwd") };
            api.LoginService.Register(enemyCredential);
            var enemyToken = api.LoginService.LoginAsync(enemyCredential).Result;
            tempUser.Add(enemyCredential.Login, enemyToken);

            //act
            var emenyUser = api.UserService.GetUserAsync(enemyToken.UserId, token).Result;
        }

        [TestMethod]
        public void ShouldGetUserGroups()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var ourGroup = api.UserService.GetUserGroupsAsync(token.UserId, token).Result;
        }

        [TestMethod]
        public void ShouldUpdateUser()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var ourUser = api.UserService.GetUserAsync(token.UserId, token).Result;
            ourUser.Name = "Marlock";
            ourUser = api.UserService.UpdateUserAsync(ourUser, token).Result;
        }

        #region Clean Up temp

        [TestCleanup]
        public void CleanData()
        {
            foreach (var keyvalue in tempUser)
            {
                api.LoginService.Delete(keyvalue.Key, keyvalue.Value);
            }
        }

        #endregion
    }
}
