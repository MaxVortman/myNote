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
        private LoginService client = new LoginService(ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldRegisterUserThroughApi()
        {
            //arrange
            var credential = new Credential { Login = "Max", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };

            //act
            client.Register(credential);
            var token = client.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
        }

        #region Clean Up temp

        [TestCleanup]
        public void CleanData()
        {
            foreach (var keyvalue in tempUser)
            {
                client.Delete(keyvalue.Key, keyvalue.Value);
            }
        }

        #endregion

    }
}

