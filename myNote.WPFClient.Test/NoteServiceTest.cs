using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myNote.Model;
using myNote.WPFClient.ApiServices;

namespace myNote.WPFClient.Test
{
    [TestClass]
    public class NoteServiceTest
    {
        const string ConnectionString = @"http://localhost:64625/api/";
        private LoginService loginClient = new LoginService(ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateNoteThroughApi()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var noteClient = new NoteService(ConnectionString);
            var note = noteClient.CreateNoteAsync(new Note { Title = "TestNote", UserId = token.UserId}, token).Result;
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
