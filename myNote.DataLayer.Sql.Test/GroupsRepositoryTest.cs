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
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";
        private readonly List<string> tempUsersLogin = new List<string>();

        [TestMethod]
        public void ShouldCreateGroup()
        {
            //arrange
            var user = HelpingClass.CreateUser();
            const string groupName = "TestGroup";

            //act
            var groupsRepository = new GroupsRepository(ConnectionString);
            tempUsersLogin.Add(user.Login);
            groupsRepository.CreateGroup(user.Id, groupName);
            var groupFromDb = groupsRepository.GetUserGroups(user.Id);

            //asserts
            Assert.AreEqual(groupName, groupFromDb.Single().Name);
        }

        [TestMethod]
        public void ShouldCreateTenUserGroups()
        {
            //arrange
            const int CountOfGroups = 10;
            var user = HelpingClass.CreateUser();
            const string groupName = "TestGroup";
            var groups = new Group[CountOfGroups];

            //act
            var groupsRepository = new GroupsRepository(ConnectionString);
            tempUsersLogin.Add(user.Login);            

            for (int i = 0; i < CountOfGroups; i++)
            {
                groups[i] = groupsRepository.CreateGroup(user.Id, groupName + i);
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
            var user = HelpingClass.CreateUser();
            var groupsRepository = new GroupsRepository(ConnectionString);
            tempUsersLogin.Add(user.Login);
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
            var user = HelpingClass.CreateUser();
            var groupsRepository = new GroupsRepository(ConnectionString);
            tempUsersLogin.Add(user.Login);
            var group = groupsRepository.CreateGroup(user.Id, GroupName);

            //act
            var groupId = group.Id;
            groupsRepository.DeleteGroup(groupId);
            groupsRepository.GetGroup(groupId);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldDeleteGroupByName()
        {
            //arrange
            const string GroupName = "TestGroup";
            var user = HelpingClass.CreateUser();
            var groupsRepository = new GroupsRepository(ConnectionString);
            tempUsersLogin.Add(user.Login);
            var group = groupsRepository.CreateGroup(user.Id, GroupName);

            //act
            var groupId = group.Id;
            groupsRepository.DeleteGroup(group.UserId, group.Name);
            groupsRepository.GetGroup(groupId);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MustThrowInvalidOperationExceptionWhenDeleteByName()
        {
            //arrange
            const string GroupName = "TestGroup";
            var user = HelpingClass.CreateUser();
            var groupsRepository = new GroupsRepository(ConnectionString);
            tempUsersLogin.Add(user.Login);
            var group = groupsRepository.CreateGroup(user.Id, GroupName);
            var note = new Note { Title = "TestNote", UserId = user.Id };
            var notesRepository = new NotesRepository(ConnectionString);
            note = notesRepository.CreateNote(note);
            var noteGroupsRepository = new NoteGroupsRepository(ConnectionString);
            noteGroupsRepository.CreateNoteGroup(note.Id, group.Id);

            //act
            groupsRepository.DeleteGroup(group.UserId, group.Name);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void MustThrowInvalidOperationExceptionWhenDeleteById()
        {
            //arrange
            const string GroupName = "TestGroup";
            var user = HelpingClass.CreateUser();
            var groupsRepository = new GroupsRepository(ConnectionString);
            tempUsersLogin.Add(user.Login);
            var group = groupsRepository.CreateGroup(user.Id, GroupName);
            var note = new Note { Title = "TestNote", UserId = user.Id };
            var notesRepository = new NotesRepository(ConnectionString);
            note = notesRepository.CreateNote(note);
            var noteGroupsRepository = new NoteGroupsRepository(ConnectionString);
            noteGroupsRepository.CreateNoteGroup(note.Id, group.Id);

            //act
            groupsRepository.DeleteGroup(group.Id);
        }

        [TestCleanup]
        public void CleanData()
        {
            var credentialsRepository = new CredentialsRepository(ConnectionString, new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString)));
            foreach (var login in tempUsersLogin)
                credentialsRepository.Delete(login);
        }
    }
}
