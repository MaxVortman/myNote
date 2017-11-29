using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using myNote.Model;
using System.Linq;

namespace myNote.DataLayer.Sql.Test
{
    [TestClass]
    public class NoteGroupsRepositoryTest
    {

        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";
        private readonly Dictionary<string, Token> tempUsersLogin = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateNoteGroup()
        {
            //arrange
            const string groupName = "TestGroup";
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            var groupsRepository = new GroupsRepository(ConnectionString);
            var notesRepository = new NotesRepository(ConnectionString);
            var note = new Note { Title = "TestNote", UserId = user.Id };
            var group = groupsRepository.CreateGroup(groupName, token);
            note = notesRepository.CreateNote(note, token);

            //act
            var noteGroupsRepository = new NoteGroupsRepository(ConnectionString);
            var noteGroups = noteGroupsRepository.CreateNoteGroup(note.Id, group.Id, token);
            var groupFromDb = noteGroupsRepository.GetGroupBy(noteGroups.NoteId);

            //asserts
            Assert.AreEqual(group.Id, groupFromDb.Id);
            Assert.AreEqual(group.Name, groupFromDb.Name);
        }

        [TestMethod]
        public void ShouldCreateTenNoteGroup()
        {
            //arrange
            const int CountOfNotesGroups = 10;
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            const string noteTitle = "TestNote";
            const string groupName = "TestGroup";
            var notes = new Note[CountOfNotesGroups];
            var groupsRepository = new GroupsRepository(ConnectionString);            
            var notesRepository = new NotesRepository(ConnectionString);
            for (int i = 0; i < CountOfNotesGroups; i++)
            {
                notes[i] = new Note
                {
                    Title = $"{noteTitle}{i}",
                    UserId = user.Id
                };
                notes[i] = notesRepository.CreateNote(notes[i], token);
            }
            var group = groupsRepository.CreateGroup(groupName, token);

            //act
            var noteGroupsRepository = new NoteGroupsRepository(ConnectionString);
            for (int i = 0; i < CountOfNotesGroups; i++)
            {
                noteGroupsRepository.CreateNoteGroup(notes[i].Id, group.Id, token);
            }
            var notesFromDb = noteGroupsRepository.GetAllNoteBy(group.Id).ToArray();

            //asserts
            Assert.AreEqual(CountOfNotesGroups, notesFromDb.Length);
            var isContains = false;
            for (int i = 0; i < CountOfNotesGroups; i++)
            {
                isContains = false;
                for (int j = 0; j < CountOfNotesGroups; j++)
                {
                    if (notesFromDb[i].Id == notes[j].Id)
                    {
                        isContains = true;
                        break;
                    }
                }
                if (isContains == false)
                {
                    break;
                }
            }
            Assert.IsTrue(isContains);
        }

        [TestMethod]
        public void ShouldBeZeroNotes()
        {
            //arrange
            const int Zero = 0;
            const string GroupName = "TestGroup";
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            var groupsRepository = new GroupsRepository(ConnectionString);            
            var group = groupsRepository.CreateGroup(GroupName, token);
            //act
            var noteGroupsRepository = new NoteGroupsRepository(ConnectionString);
            var notes = noteGroupsRepository.GetAllNoteBy(group.Id);

            //asserts
            Assert.AreEqual(Zero, notes.Count());
        }

        [TestCleanup]
        public void CleanData()
        {
            var credentialsRepository = new CredentialsRepository(ConnectionString, new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString)), new TokensRepository(ConnectionString));
            foreach (var keyvalue in tempUsersLogin)
                credentialsRepository.Delete(keyvalue.Key, keyvalue.Value);
        }
    }
}
