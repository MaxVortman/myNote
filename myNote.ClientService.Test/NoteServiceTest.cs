using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myNote.ClientService;
using myNote.Model;
using System.Linq;

namespace myNote.ClientService.Test
{
    [TestClass]
    public class NoteServiceTest
    {
        const string ConnectionString = @"http://localhost:64625/api/";
        private ApiClient api = ApiClient.CreateInstance(ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateNote()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            //act
            var note = api.NoteService.CreateNoteAsync(new Note { Title = "TestNote", UserId = token.UserId}, token).Result;
        }

        [TestMethod]
        public void ShouldGetNote()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            var note = api.NoteService.CreateNoteAsync(new Note { Title = "TestNote", UserId = token.UserId }, token).Result;

            //act
            var noteFromApi = api.NoteService.GetUserNoteAsync(note.Id).Result;

            //assert
            Assert.AreEqual(note.Id, noteFromApi.Id);
        }

        [TestMethod]
        public void ShouldGetNotes()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            var note = api.NoteService.CreateNoteAsync(new Note { Title = "TestNote", UserId = token.UserId }, token).Result;

            //act
            var notesFromApi = api.NoteService.GetUserNotesAsync(token.UserId).Result;

            //assert
            Assert.AreEqual(note.Id, notesFromApi.First().Id);
        }

        [TestMethod]
        public void ShouldUpdateNote()
        {
            //arrange
            const string CONTENT = "So big CONTENT";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            var note = api.NoteService.CreateNoteAsync(new Note { Title = "TestNote", UserId = token.UserId }, token).Result;

            //act
            note.Content = CONTENT;
            var noteFromApi = api.NoteService.UpdateNoteAsync(note, token).Result;

            //assert
            Assert.AreEqual(CONTENT, noteFromApi.Content);
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
