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
        private readonly Dictionary<string, Token> tempUsersLogin = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateNote()
        {
            //arrange
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            var note = new Note
            {
                Title = "TestNote"
            };

            //act
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            var notesRepository = new NotesRepository(ConnectionString);

            note.UserId = user.Id;
            note = notesRepository.CreateNote(note, token);
            var noteFromDb = notesRepository.GetNote(note.Id);

            Assert.AreEqual(note.Title, noteFromDb.Title);
            Assert.AreEqual(note.UserId, noteFromDb.UserId);
        }

        [TestMethod]
        public void ShouldCreateTenUserNotes()
        {
            //arrange
            const int CountOfNotes = 10;
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            const string noteTitle = "TestNote";
            var notes = new Note[CountOfNotes];

            

            //act
            var notesRepository = new NotesRepository(ConnectionString);

            for (int i = 0; i < CountOfNotes; i++)
            {
                notes[i] = new Note { Title = $"{noteTitle}{i}", UserId = user.Id };
            }

            for (int i = 0; i < CountOfNotes; i++)
            {
                notes[i] = notesRepository.CreateNote(notes[i], token);
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
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            var note = new Note
            {
                Title = "TestNote"
            };

            //act
            var notesRepository = new NotesRepository(ConnectionString);

            note.UserId = user.Id;

            note = notesRepository.CreateNote(note, token);

            note.Title = "UpdatedTestNote";
            notesRepository.UpdateNote(note, token);

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
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            var notesRepository = new NotesRepository(ConnectionString);
            notesRepository.UpdateNote(new Note { Id = Guid.NewGuid() }, token);
        }

        [TestMethod]
        public void ShouldBeZeroNotes()
        {
            //arrange
            const int Zero = 0;
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);

            //act
            var notesRepository = new NotesRepository(ConnectionString);
            var notes = notesRepository.GetUserNotes(user.Id);

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
