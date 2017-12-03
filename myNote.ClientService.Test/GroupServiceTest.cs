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
        private ApiClient api = ApiClient.CreateInstance(ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateGroup()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var group = api.GroupService.CreateGroupAsync(NAME, token).Result;
        }

        [TestMethod]
        public void ShouldCreateAndDeleteGroupByName()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var group = api.GroupService.CreateGroupAsync(NAME, token).Result;
            api.GroupService.DeleteGroup(NAME, token);
        }

        [TestMethod]
        public void ShouldCreateAndDeleteGroupById()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var group = api.GroupService.CreateGroupAsync(NAME, token).Result;
            api.GroupService.DeleteGroup(group.Id, token);
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
