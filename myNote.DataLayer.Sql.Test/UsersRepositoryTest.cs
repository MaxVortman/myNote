using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using myNote.Model;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace myNote.DataLayer.Sql.Test
{
    [TestClass]
    public class UsersRepositoryTest
    {
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";
        private readonly List<string> tempUsersLogin = new List<string>();

        [TestMethod]
        public void ShouldCreateUser()
        {
            //arrange
            var user = HelpingClass.CreateUser();

            //act
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            tempUsersLogin.Add(user.Login);
            var userFromDb = usersRepository.GetUser(user.Id);

            //asserts
            Assert.AreEqual(user.Name, userFromDb.Name);
        }        

        [TestMethod]
        public void ShouldCreateUserAndAddCategory()
        {
            //arrange
            var user = HelpingClass.CreateUser();
            const string category = "testCategory";

            //act
            var categoriesRepository = new GroupsRepository(ConnectionString);
            var usersRepository = new UsersRepository(ConnectionString, categoriesRepository);
            tempUsersLogin.Add(user.Login);
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
            var user = HelpingClass.CreateUser();
            var usersRpository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            tempUsersLogin.Add(user.Login);

            //act
            user.Name = UserName;
            user = usersRpository.UpdateUser(user);

            //asserts
            Assert.AreEqual(UserName, user.Name);
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
