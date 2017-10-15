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
        private readonly List<Guid> tempUsersId = new List<Guid>();

        [TestMethod]
        public void ShouldCreateShare()
        {
            //arrange
            var user = new User { Name = "TestUser", Email = "dghja@ff.ry" };
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            user = usersRepository.CreateUser(user);
            tempUsersId.Add(user.Id);
            var note = new Note
            {
                UserId = user.Id,
                Title = "TestNote"
            };
            var notesRepository = new NotesRepository(ConnectionString);
            note = notesRepository.CreateNote(note);

            //act
            var sharesRepository = new SharesRepository(ConnectionString);
            sharesRepository.CreateShare(note);

            //asserts
            Assert.IsTrue(sharesRepository.IsNoteShared(note.Id));
            Assert.IsNotNull(sharesRepository.GetAllUserSharesNotes(user.Id));
        }

        [TestMethod]
        public void ShouldCreateTenShares()
        {
            //arrange
            const int CountOfShares = 10;
            var user = new User { Name = "TestUser", Email = "dfa@ff.ghhjry" };
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            user = usersRepository.CreateUser(user);
            tempUsersId.Add(user.Id);
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
                notes[i] = notesRepository.CreateNote(notes[i]);
            }

            //act
            var shares = new Share[CountOfShares];
            var sharesRepository = new SharesRepository(ConnectionString);
            for (int i = 0; i < CountOfShares; i++)
            {
                shares[i] = sharesRepository.CreateShare(notes[i]);
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
            var user = new User
            {
                Email = "fd@ggd.gds",
                Name = "TestUser"
            };
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            user = usersRepository.CreateUser(user);
            tempUsersId.Add(user.Id);

            //act
            var sharesRepository = new SharesRepository(ConnectionString);
            var notes = sharesRepository.GetAllUserSharesNotes(user.Id);

            //asserts
            Assert.AreEqual(Zero, notes.Count());
        }

        [TestCleanup]
        public void CleanData()
        {
            foreach (var id in tempUsersId)
                new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString)).DeleteUser(id);
        }
    }
}
