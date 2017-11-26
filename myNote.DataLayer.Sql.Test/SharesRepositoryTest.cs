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
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";
        private readonly List<string> tempUsersLogin = new List<string>();

        [TestMethod]
        public void ShouldCreateShare()
        {
            //arrange
            var user = HelpingClass.CreateUser();
            var token = HelpingClass.GetToken();
            tempUsersLogin.Add(user.Login);
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
            var user = HelpingClass.CreateUser();
            var token = HelpingClass.GetToken();
            tempUsersLogin.Add(user.Login);
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
            var user = HelpingClass.CreateUser();
            tempUsersLogin.Add(user.Login);

            //act
            var sharesRepository = new SharesRepository(ConnectionString);
            var notes = sharesRepository.GetAllUserSharesNotes(user.Id);

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
