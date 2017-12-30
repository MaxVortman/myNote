using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using myNote.Model;
using System.Linq;

namespace myNote.DataLayer.Sql.Test
{
    [TestClass]
    public class SharesRepositoryTest
    {
        private const string ConnectionString = IoC.ConnectionString;
        private readonly Dictionary<string, Token> tempUsersLogin = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateShare()
        {
            //arrange
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            var note = new Note
            {
                UserId = user.Id,
                Title = "TestNote"
            };
            var notesRepository = new NotesRepository(ConnectionString);
            note = notesRepository.CreateNote(note, token);

            //act
            var sharesRepository = new SharesRepository(ConnectionString);
            sharesRepository.CreateShare(note, token);

            //asserts
            Assert.IsTrue(sharesRepository.IsNoteShared(note.Id));
            Assert.IsNotNull(sharesRepository.GetAllUserSharesNotes(user.Id));
        }

        [TestMethod]
        public void ShouldCreateTenShares()
        {
            //arrange
            const int CountOfShares = 10;
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            const string noteTitle = "TestNote";
            var notes = new Note[CountOfShares];
            var notesRepository = new NotesRepository(ConnectionString);
            for (int i = 0; i < CountOfShares; i++)
            {
                notes[i] = new Note
                {
                    Title = $"{noteTitle}{i}",
                    UserId = user.Id
                };
                notes[i] = notesRepository.CreateNote(notes[i], token);
            }

            //act
            var shares = new Share[CountOfShares];
            var sharesRepository = new SharesRepository(ConnectionString);
            for (int i = 0; i < CountOfShares; i++)
            {
                shares[i] = sharesRepository.CreateShare(notes[i], token);
            }
            var sharesFromDb = sharesRepository.GetAllUserSharesNotes(user.Id).ToArray();

            //asserts
            Assert.AreEqual(CountOfShares, sharesFromDb.Length);
            var isContains = false;
            for (int i = 0; i < CountOfShares; i++)
            {
                isContains = false;
                for (int j = 0; j < CountOfShares; j++)
                {
                    if (sharesFromDb[i].Id == notes[j].Id)
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
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);

            //act
            var sharesRepository = new SharesRepository(ConnectionString);
            var notes = sharesRepository.GetAllUserSharesNotes(user.Id);

            //asserts
            Assert.AreEqual(Zero, notes.Count());
        }

        [TestMethod]
        public void ShouldGetSomeShares()
        {
            //arrange
            const int COUNT = 10;
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);

            var sharesRepository = new SharesRepository(ConnectionString);
            var notesRepository = new NotesRepository(ConnectionString);
            Note note;
            for (int i = 0; i < COUNT; i++)
            {

                note = notesRepository.CreateNote(new Note { UserId = token.UserId, Title = i.ToString() }, token);
                sharesRepository.CreateShare(note, token); 
            }

            //act
            var notesFromShares = sharesRepository.GetSomeShares(COUNT);

            //assert
            Assert.AreEqual(COUNT, notesFromShares.Count());
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
