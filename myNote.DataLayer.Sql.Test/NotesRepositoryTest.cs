using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using myNote.Model;

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

        [TestCleanup]
        public void CleanData()
        {
            foreach (var id in tempUsersId)
                new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString)).DeleteUser(id);
        }
    }
}
