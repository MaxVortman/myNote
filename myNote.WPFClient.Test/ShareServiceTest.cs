using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myNote.Model;

namespace myNote.ClientService.Test
{
    [TestClass]
    public class ShareServiceTest
    {

        const string ConnectionString = @"http://localhost:64625/api/";
        private LoginService loginClient = new LoginService(ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateShare()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var noteService = new NoteService(ConnectionString);
            var shareService = new ShareService(ConnectionString);

            var note = noteService.CreateNoteAsync(new Note { UserId = token.UserId }, token).Result;
            var share = shareService.CreateShareAsync(note, token).Result;
        }


        [TestMethod]
        public void ShouldGetUserShares()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            var noteService = new NoteService(ConnectionString);
            var shareService = new ShareService(ConnectionString);

            var note = noteService.CreateNoteAsync(new Note { UserId = token.UserId }, token).Result;
            var share = shareService.CreateShareAsync(note, token).Result;

            //act
            var userSharesFromApi = shareService.GetUserSharesAsync(token.UserId).Result;

            //assert
            Assert.AreEqual(share.NoteId, userSharesFromApi.First().Id);
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
