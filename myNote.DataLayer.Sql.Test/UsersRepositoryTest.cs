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
        private readonly Dictionary<string, Token> tempUsersLogin = new Dictionary<string, Token>();

        [TestMethod]
        public void ShouldCreateUser()
        {
            //arrange
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);

            //act
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            var userFromDb = usersRepository.GetUser(user.Id, token);

            //asserts
            Assert.AreEqual(user.Name, userFromDb.Name);
        }        

        [TestMethod]
        public void ShouldCreateUserAndAddCategory()
        {
            //arrange
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            const string category = "testCategory";

            //act
            var categoriesRepository = new GroupsRepository(ConnectionString);
            var usersRepository = new UsersRepository(ConnectionString, categoriesRepository);
            categoriesRepository.CreateGroup(category, token);
            user = usersRepository.GetUser(user.Id, token);

            //asserts
            Assert.AreEqual(category, user.UserGroups.Single().Name);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenReceiveUser()
        {
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            var usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            usersRepository.GetUser(Guid.NewGuid(), token);
        }


        [TestMethod]
        public void ShouldUpdateUser()
        {
            //arrange
            const string UserName = "UpdatedName";
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            var usersRpository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));

            //act
            user.Name = UserName;
            user = usersRpository.UpdateUser(user, token);

            //asserts
            Assert.AreEqual(UserName, user.Name);
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
