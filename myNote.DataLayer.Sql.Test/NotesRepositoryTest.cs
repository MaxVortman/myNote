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
        private readonly List<string> tempUsersLogin = new List<string>();

        [TestMethod]
        public void ShouldCreateNote()
        {
            //arrange
            var user = HelpingClass.CreateUser();
            var note = new Note
            {
                Title = "TestNote"
            };

            //act
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            var notesRepository = new NotesRepository(ConnectionString);

            note.UserId = user.Id;
            tempUsersLogin.Add(user.Login);
            note = notesRepository.CreateNote(note, HelpingClass.GetToken());
            var noteFromDb = notesRepository.GetNote(note.Id);

            Assert.AreEqual(note.Title, noteFromDb.Title);
            Assert.AreEqual(note.UserId, noteFromDb.UserId);
        }

        [TestMethod]
        public void ShouldCreateTenUserNotes()
        {
            //arrange
            const int CountOfNotes = 10;
            var user = HelpingClass.CreateUser();
            var token = HelpingClass.GetToken();
            const string noteTitle = "TestNote";
            var notes = new Note[CountOfNotes];

            

            //act
            var notesRepository = new NotesRepository(ConnectionString);

            tempUsersLogin.Add(user.Login);

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
            var user = HelpingClass.CreateUser();
            var token = HelpingClass.GetToken();
            var note = new Note
            {
                Title = "TestNote"
            };

            //act
            var notesRepository = new NotesRepository(ConnectionString);

            tempUsersLogin.Add(user.Login);

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
            var user = HelpingClass.CreateUser();
            tempUsersLogin.Add(user.Login);
            var notesRepository = new NotesRepository(ConnectionString);
            notesRepository.UpdateNote(new Note { Id = Guid.NewGuid() }, HelpingClass.GetToken());
        }

        [TestMethod]
        public void ShouldBeZeroNotes()
        {
            //arrange
            const int Zero = 0;
            var user = HelpingClass.CreateUser();
            tempUsersLogin.Add(user.Login);

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
            foreach (var login in tempUsersLogin)
                credentialsRepository.Delete(login);
        }
    }
}
