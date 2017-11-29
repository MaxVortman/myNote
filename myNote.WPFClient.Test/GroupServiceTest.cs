using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myNote.Model;

namespace myNote.ClientService.Test
{
    [TestClass]
    public class GroupServiceTest
    {

        const string ConnectionString = @"http://localhost:64625/api/";
        private LoginService loginClient = new LoginService(ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateGroup()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var groupService = new GroupService(ConnectionString);
            var group = groupService.CreateGroup(NAME, token).Result;
        }

        [TestMethod]
        public void ShouldCreateAndDeleteGroupByName()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var groupService = new GroupService(ConnectionString);
            var group = groupService.CreateGroup(NAME, token).Result;
            groupService.DeleteGroup(NAME, token);
        }

        [TestMethod]
        public void ShouldCreateAndDeleteGroupById()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var groupService = new GroupService(ConnectionString);
            var group = groupService.CreateGroup(NAME, token).Result;
            groupService.DeleteGroup(group.Id, token);
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
