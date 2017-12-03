using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myNote.ClientService;
using myNote.Model;

namespace myNote.ClientService.Test
{
    [TestClass]
    public class LoginServiceTest
    {
        const string ConnectionString = @"http://localhost:64625/api/";
        private ApiClient api = ApiClient.CreateInstance(ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldRegisterUserThroughApi()
        {
            //arrange
            var credential = new Credential { Login = "Max", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };

            //act
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
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

