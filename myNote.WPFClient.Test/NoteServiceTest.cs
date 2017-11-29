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
        private LoginService loginClient = new LoginService(ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateNote()
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

        [TestMethod]
        public void ShouldGetNote()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            var noteClient = new NoteService(ConnectionString);
            var note = noteClient.CreateNoteAsync(new Note { Title = "TestNote", UserId = token.UserId }, token).Result;

            //act
            var noteFromApi = noteClient.GetUserNoteAsync(note.Id).Result;

            //assert
            Assert.AreEqual(note.Id, noteFromApi.Id);
        }

        [TestMethod]
        public void ShouldGetNotes()
        {
            //arrange
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            var noteClient = new NoteService(ConnectionString);
            var note = noteClient.CreateNoteAsync(new Note { Title = "TestNote", UserId = token.UserId }, token).Result;

            //act
            var notesFromApi = noteClient.GetUserNotesAsync(token.UserId).Result;

            //assert
            Assert.AreEqual(note.Id, notesFromApi.First().Id);
        }

        [TestMethod]
        public void ShouldUpdateNote()
        {
            //arrange
            const string CONTENT = "So big CONTENT";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            var noteClient = new NoteService(ConnectionString);
            var note = noteClient.CreateNoteAsync(new Note { Title = "TestNote", UserId = token.UserId }, token).Result;

            //act
            note.Content = CONTENT;
            var noteFromApi = noteClient.UpdateNoteAsync(note, token).Result;

            //assert
            Assert.AreEqual(CONTENT, noteFromApi.Content);
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
