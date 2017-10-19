using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using myNote.Model;
using System.Linq;

namespace myNote.DataLayer.Sql.Test
{
    [TestClass]
    public class UsersRepositoryTest
    {
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";
        private readonly List<Guid> tempUsersId = new List<Guid>();

        [TestMethod]
        public void ShouldCreateUser()
        {
            //arrange
            var user = new User
            {
                Name = "TestUser",
                Email = "dfa@ff.ry"
            };

            //act
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));

            user = usersRepository.CreateUser(user);
            tempUsersId.Add(user.Id);
            var userFromDb = usersRepository.GetUser(user.Id);

            //asserts
            Assert.AreEqual(user.Name, userFromDb.Name);
        }

        [TestMethod]
        public void ShouldCreateUserAndAddCategory()
        {
            //arrange
            var user = new User { Name = "test", Email = "fgha@fsf.re" };
            const string category = "testCategory";

            //act
            var categoriesRepository = new GroupsRepository(ConnectionString);
            var usersRepository = new UsersRepository(ConnectionString, categoriesRepository);
            user = usersRepository.CreateUser(user);

            tempUsersId.Add(user.Id);

            categoriesRepository.CreateGroup(user.Id, category);
            user = usersRepository.GetUser(user.Id);

            //asserts
            Assert.AreEqual(category, user.UserGroups.Single().Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenReceiveUser()
        {
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            usersRepository.GetUser(Guid.NewGuid());
        }


        [TestMethod]
        public void ShouldUpdateUser()
        {
            //arrange
            const string UserName = "UpdatedName";
            var user = new User { Name = "test", Email = "fgha@fsf.re" };
            var usersRpository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            user = usersRpository.CreateUser(user);
            tempUsersId.Add(user.Id);

            //act
            user.Name = UserName;
            user = usersRpository.UpdateUser(user);

            //asserts
            Assert.AreEqual(UserName, user.Name);
        }

        [TestCleanup]
        public void CleanData()
        {
            foreach (var id in tempUsersId)
                new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString)).DeleteUser(id);
        }
    }
}
