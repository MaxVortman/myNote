using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using myNote.Model;
using System.Linq;

namespace myNote.DataLayer.Sql.Test
{
    [TestClass]
    public class NotesRepositoryTest
    {

        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";
        private readonly List<Guid> tempUsersId = new List<Guid>();

        [TestMethod]
        public void ShouldCreateNote()
        {
            //arrange
            var user = new User { Name = "TestUser", Email = "dghja@ff.ry" };
            var note = new Note
            {
                Title = "TestNote"
            };

            //act
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            var notesRepository = new NotesRepository(ConnectionString);

            user = usersRepository.CreateUser(user);
            note.UserId = user.Id;
            tempUsersId.Add(user.Id);
            note = notesRepository.CreateNote(note);
            var noteFromDb = notesRepository.GetNote(note.Id);

            Assert.AreEqual(note.Title, noteFromDb.Title);
            Assert.AreEqual(note.UserId, noteFromDb.UserId);
        }

        [TestMethod]
        public void ShouldCreateTenUserNotes()
        {
            //arrange
            const int CountOfNotes = 10;
            var user = new User { Name = "TestUser", Email = "dfa@ff.ghhjry" };
            const string noteTitle = "TestNote";
            var notes = new Note[CountOfNotes];

            

            //act
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            var notesRepository = new NotesRepository(ConnectionString);

            user = usersRepository.CreateUser(user);
            tempUsersId.Add(user.Id);

            for (int i = 0; i < CountOfNotes; i++)
            {
                notes[i] = new Note { Title = $"{noteTitle}{i}", UserId = user.Id };
            }

            for (int i = 0; i < CountOfNotes; i++)
            {
                notes[i] = notesRepository.CreateNote(notes[i]);
            }

            var notesFromDb = notesRepository.GetUserNotes(user.Id).ToArray();

            //asserts
            Assert.AreEqual(CountOfNotes, notesFromDb.Length);
            var isContains = false;
            for (int i = 0; i < CountOfNotes; i++)
            {
                isContains = false;
                for (int j = 0; j < CountOfNotes; j++)
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
        public void ShouldUpdateNote()
        {
            //arrange
            var user = new User { Name = "TestUser", Email = "dfa@ff.ghhjry" };
            var note = new Note
            {
                Title = "TestNote"
            };

            //act
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            var notesRepository = new NotesRepository(ConnectionString);

            user = usersRepository.CreateUser(user);
            tempUsersId.Add(user.Id);

            note.UserId = user.Id;

            note = notesRepository.CreateNote(note);

            note.Title = "UpdatedTestNote";
            notesRepository.UpdateNote(note);

            var noteFromDb = notesRepository.GetNote(note.Id);

            //asserts
            Assert.AreEqual(note.Title, noteFromDb.Title);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenReceiveNote()
        {
            var notesRepository = new NotesRepository(ConnectionString);
            notesRepository.GetNote(Guid.NewGuid());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenUpdateNote()
        {
            var notesRepository = new NotesRepository(ConnectionString);
            notesRepository.UpdateNote(new Note { Id = Guid.NewGuid() });
        }

        [TestCleanup]
        public void CleanData()
        {
            foreach (var id in tempUsersId)
                new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString)).DeleteUser(id);
        }
    }
}
