using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myNote.Model;
using System.Collections.Generic;
using System.Linq;

namespace myNote.DataLayer.Sql.Test
{
    [TestClass]
    public class GroupsRepositoryTest
    {
        private const string ConnectionString = IoC.ConnectionString;
        private readonly Dictionary<string, Token> tempUsersLogin = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateGroup()
        {
            //arrange
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            const string groupName = "TestGroup";

            //act
            var groupsRepository = new GroupsRepository(ConnectionString);
            groupsRepository.CreateGroup(groupName, token);
            var groupFromDb = groupsRepository.GetUserGroups(user.Id);

            //asserts
            Assert.AreEqual(groupName, groupFromDb.Single().Name);
        }

        [TestMethod]
        public void ShouldCreateTenUserGroups()
        {
            //arrange
            const int CountOfGroups = 10;
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            const string groupName = "TestGroup";
            var groups = new Group[CountOfGroups];

            //act
            var groupsRepository = new GroupsRepository(ConnectionString);          

            for (int i = 0; i < CountOfGroups; i++)
            {
                groups[i] = groupsRepository.CreateGroup(groupName + i, token);
            }

            var groupsFromDb = groupsRepository.GetUserGroups(user.Id).ToArray();

            //asserts
            Assert.AreEqual(CountOfGroups, groupsFromDb.Length);
            var isContains = false;
            for (int i = 0; i < CountOfGroups; i++)
            {
                isContains = false;
                for (int j = 0; j < CountOfGroups; j++)
                {
                    if (groupsFromDb[i].Id == groups[j].Id)
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
            var groupsRepository = new GroupsRepository(ConnectionString);

            //act
            var groups = groupsRepository.GetUserGroups(user.Id);

            //asserts
            Assert.AreEqual(Zero, groups.Count());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldDeleteGroupById()
        {
            //arrange
            const string GroupName = "TestGroup";
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            var groupsRepository = new GroupsRepository(ConnectionString);
            var group = groupsRepository.CreateGroup(GroupName, token);

            //act
            var groupId = group.Id;
            groupsRepository.DeleteGroup(groupId, token);
            groupsRepository.GetGroup(groupId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldDeleteGroupByName()
        {
            //arrange
            const string GroupName = "TestGroup";
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            var groupsRepository = new GroupsRepository(ConnectionString);
            var group = groupsRepository.CreateGroup(GroupName, token);

            //act
            var groupId = group.Id;
            groupsRepository.DeleteGroup(group.Name, token);
            groupsRepository.GetGroup(groupId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MustThrowInvalidOperationExceptionWhenDeleteByName()
        {
            //arrange
            const string GroupName = "TestGroup";
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            var groupsRepository = new GroupsRepository(ConnectionString);
            var group = groupsRepository.CreateGroup(GroupName, token);
            var note = new Note { Title = "TestNote", UserId = user.Id };
            var notesRepository = new NotesRepository(ConnectionString);
            note = notesRepository.CreateNote(note, token);
            var noteGroupsRepository = new NoteGroupsRepository(ConnectionString);
            noteGroupsRepository.CreateNoteGroup(new NoteGroup { NoteId = note.Id, GroupId = group.Id }, token);

            //act
            groupsRepository.DeleteGroup(group.Name, token);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MustThrowInvalidOperationExceptionWhenDeleteById()
        {
            //arrange
            const string GroupName = "TestGroup";
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            var groupsRepository = new GroupsRepository(ConnectionString);
            var group = groupsRepository.CreateGroup(GroupName, token);
            var note = new Note { Title = "TestNote", UserId = user.Id };
            var notesRepository = new NotesRepository(ConnectionString);
            note = notesRepository.CreateNote(note, token);
            var noteGroupsRepository = new NoteGroupsRepository(ConnectionString);
            noteGroupsRepository.CreateNoteGroup(new NoteGroup { NoteId = note.Id, GroupId = group.Id }, token);

            //act
            groupsRepository.DeleteGroup(group.Id, token);
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
