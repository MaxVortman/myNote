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
        private readonly List<Guid> tempUsersId = new List<Guid>();

        [TestMethod]
        public void ShouldCreateNoteGroup()
        {
            //arrange
            const string groupName = "TestGroup";
            var user = new User { Name = "TestUser", Email = "dghja@ff.ry" };
            var groupsRepository = new GroupsRepository(ConnectionString);
            var usersRepository = new UsersRepository(ConnectionString, groupsRepository);
            var notesRepository = new NotesRepository(ConnectionString);
            user = usersRepository.CreateUser(user);
            tempUsersId.Add(user.Id);
            var note = new Note { Title = "TestNote", UserId = user.Id };
            var group = groupsRepository.CreateGroup(user.Id, groupName);
            note = notesRepository.CreateNote(note);

            //act
            var noteGroupsRepository = new NoteGroupsRepository(ConnectionString);
            var noteGroups = noteGroupsRepository.CreateNoteGroup(note.Id, group.Id);
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
            var user = new User { Name = "TestUser", Email = "dfa@ff.ghhjry" };
            const string noteTitle = "TestNote";
            const string groupName = "TestGroup";
            var notes = new Note[CountOfNotesGroups];
            var groupsRepository = new GroupsRepository(ConnectionString);
            var usersRepository = new UsersRepository(ConnectionString, groupsRepository);
            user = usersRepository.CreateUser(user);
            tempUsersId.Add(user.Id);
            var notesRepository = new NotesRepository(ConnectionString);
            for (int i = 0; i < CountOfNotesGroups; i++)
            {
                notes[i] = new Note
                {
                    Title = $"{noteTitle}{i}",
                    UserId = user.Id
                };
                notes[i] = notesRepository.CreateNote(notes[i]);
            }
            var group = groupsRepository.CreateGroup(user.Id, groupName);

            //act
            var noteGroupsRepository = new NoteGroupsRepository(ConnectionString);
            for (int i = 0; i < CountOfNotesGroups; i++)
            {
                noteGroupsRepository.CreateNoteGroup(notes[i].Id, group.Id);
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

        [TestCleanup]
        public void CleanData()
        {
            foreach (var id in tempUsersId)
                new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString)).DeleteUser(id);
        }
    }
}
