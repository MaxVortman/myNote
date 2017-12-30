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
        private ApiClient api = ApiClient.CreateInstance(IoC.ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateShare()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var note = api.NoteService.CreateNoteAsync(new Note { UserId = token.UserId }, token).Result;
            var share = api.ShareService.CreateShareAsync(note, token).Result;
        }


        [TestMethod]
        public void ShouldGetUserShares()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            
            var note = api.NoteService.CreateNoteAsync(new Note { UserId = token.UserId }, token).Result;
            var share = api.ShareService.CreateShareAsync(note, token).Result;

            //act
            var userSharesFromApi = api.ShareService.GetUserSharesAsync(token.UserId).Result;

            //assert
            Assert.AreEqual(share.NoteId, userSharesFromApi.First().Id);
        }

        [TestMethod]
        public void ShouldGetSomeShares()
        {
            //arrange
            const int COUNT = 10;
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            
            Note note;
            Share share;
            for (int i = 0; i < COUNT; i++)
            {
                note = api.NoteService.CreateNoteAsync(new Note { UserId = token.UserId }, token).Result;
                share = api.ShareService.CreateShareAsync(note, token).Result; 
            }

            //act
            var sharesFromApi = api.ShareService.GetSomeShares(COUNT).Result;

            //assert
            Assert.AreEqual(COUNT, sharesFromApi.Count());
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
