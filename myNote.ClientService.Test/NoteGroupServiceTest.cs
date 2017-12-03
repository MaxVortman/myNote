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
        private ApiClient api = ApiClient.CreateInstance(ConnectionString);
        private Dictionary<string, Token> tempUser = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateNoteGroup()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            
            var note = api.NoteService.CreateNoteAsync(new Note { UserId = token.UserId }, token).Result;
            var group = api.GroupService.CreateGroupAsync(NAME, token).Result;

            //act
            var noteGroup = api.NoteGroupService.CreateNoteGroupAsync(new NoteGroup { NoteId = note.Id, GroupId = group.Id }, token).Result;
        }

        [TestMethod]
        public void ShouldGetGroupByNoteId()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            
            var note = api.NoteService.CreateNoteAsync(new Note { UserId = token.UserId }, token).Result;
            var group = api.GroupService.CreateGroupAsync(NAME, token).Result;
            
            var noteGroup = api.NoteGroupService.CreateNoteGroupAsync(new NoteGroup { NoteId = note.Id, GroupId = group.Id }, token).Result;

            //act
            var groupFromApi = api.NoteGroupService.GetGroupAsync(note.Id).Result;

            //assert
            Assert.AreEqual(group.Id, groupFromApi.Id);
        }

        [TestMethod]
        public void ShouldGetNotesByGroupId()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            
            var note = api.NoteService.CreateNoteAsync(new Note { UserId = token.UserId }, token).Result;
            var group = api.GroupService.CreateGroupAsync(NAME, token).Result;
            
            var noteGroup = api.NoteGroupService.CreateNoteGroupAsync(new NoteGroup { NoteId = note.Id, GroupId = group.Id }, token).Result;

            //act
            var notesFromApi = api.NoteGroupService.GetNotesInGroupAsync(group.Id, token).Result;

            //assert
            Assert.AreEqual(note.Id, notesFromApi.First().Id);
        }


        [TestMethod]
        public void ShouldGetNotesByGroupName()
        {
            //arrange
            const string NAME = "TestGroup";
            var credential = new Credential { Login = "MaxTest", Password = PasswordCrypter.GetPasswordMD5Hash("kljkljkl") };
            api.LoginService.Register(credential);
            var token = api.LoginService.LoginAsync(credential).Result;
            tempUser.Add(credential.Login, token);
            
            var note = api.NoteService.CreateNoteAsync(new Note { UserId = token.UserId }, token).Result;
            var group = api.GroupService.CreateGroupAsync(NAME, token).Result;
            
            var noteGroup = api.NoteGroupService.CreateNoteGroupAsync(new NoteGroup { NoteId = note.Id, GroupId = group.Id }, token).Result;

            //act
            var notesFromApi = api.NoteGroupService.GetNotesInGroupAsync(NAME, token).Result;

            //assert
            Assert.AreEqual(note.Id, notesFromApi.First().Id);
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
