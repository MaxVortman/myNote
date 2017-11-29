using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myNote.Model;

namespace myNote.ClientService.Test
{
    [TestClass]
    public class NoteGroupServiceTest
    {

        const string ConnectionString = @"http://localhost:64625/api/";
        private LoginService loginClient = new LoginService(ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateNoteGroup()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            var noteService = new NoteService(ConnectionString);
            var groupService = new GroupService(ConnectionString);

            var note = noteService.CreateNoteAsync(new Note { UserId = token.UserId }, token).Result;
            var group = groupService.CreateGroupAsync(NAME, token).Result;

            //act
            var noteGroupService = new NoteGroupService(ConnectionString);
            var noteGroup = noteGroupService.CreateNoteGroup(new NoteGroup { NoteId = note.Id, GroupId = group.Id }, token).Result;
        }

        [TestMethod]
        public void ShouldGetGroupByNoteId()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            var noteService = new NoteService(ConnectionString);
            var groupService = new GroupService(ConnectionString);

            var note = noteService.CreateNoteAsync(new Note { UserId = token.UserId }, token).Result;
            var group = groupService.CreateGroupAsync(NAME, token).Result;

            var noteGroupService = new NoteGroupService(ConnectionString);
            var noteGroup = noteGroupService.CreateNoteGroup(new NoteGroup { NoteId = note.Id, GroupId = group.Id }, token).Result;

            //act
            var groupFromApi = noteGroupService.GetGroupAsync(note.Id).Result;

            //assert
            Assert.AreEqual(group.Id, groupFromApi.Id);
        }

        [TestMethod]
        public void ShouldGetNotesByGroupId()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            var noteService = new NoteService(ConnectionString);
            var groupService = new GroupService(ConnectionString);

            var note = noteService.CreateNoteAsync(new Note { UserId = token.UserId }, token).Result;
            var group = groupService.CreateGroupAsync(NAME, token).Result;

            var noteGroupService = new NoteGroupService(ConnectionString);
            var noteGroup = noteGroupService.CreateNoteGroup(new NoteGroup { NoteId = note.Id, GroupId = group.Id }, token).Result;

            //act
            var notesFromApi = noteGroupService.GetNotesInGroupAsync(group.Id, token).Result;

            //assert
            Assert.AreEqual(note.Id, notesFromApi.First().Id);
        }


        [TestMethod]
        public void ShouldGetNotesByGroupName()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            loginClient.Register(credential);
            var token = loginClient.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);

            var noteService = new NoteService(ConnectionString);
            var groupService = new GroupService(ConnectionString);

            var note = noteService.CreateNoteAsync(new Note { UserId = token.UserId }, token).Result;
            var group = groupService.CreateGroupAsync(NAME, token).Result;

            var noteGroupService = new NoteGroupService(ConnectionString);
            var noteGroup = noteGroupService.CreateNoteGroup(new NoteGroup { NoteId = note.Id, GroupId = group.Id }, token).Result;

            //act
            var notesFromApi = noteGroupService.GetNotesInGroupAsync(NAME, token).Result;

            //assert
            Assert.AreEqual(note.Id, notesFromApi.First().Id);
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
