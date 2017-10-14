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
        private readonly List<Guid> tempUsersId = new List<Guid>();

        [TestMethod]
        public void ShouldCreateGroup()
        {
            //arrange
            var user = new User { Name = "TestUser", Email = "dghja@ff.ry" };
            const string groupName = "TestGroup";

            //act
            var groupsRepository = new GroupsRepository(ConnectionString);
            var usersRepository = new UsersRepository(ConnectionString, groupsRepository);

            user = usersRepository.CreateUser(user);
            tempUsersId.Add(user.Id);
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
            var user = new User { Name = "TestUser", Email = "dfa@ff.ghhjry" };
            const string groupName = "TestGroup";
            var groups = new Group[CountOfGroups];

            //act
            var groupsRepository = new GroupsRepository(ConnectionString);
            var usersRepository = new UsersRepository(ConnectionString, groupsRepository);

            user = usersRepository.CreateUser(user);
            tempUsersId.Add(user.Id);            

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

        

        [TestCleanup]
        public void CleanData()
        {
            foreach (var id in tempUsersId)
                new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString)).DeleteUser(id);
        }
    }
}
