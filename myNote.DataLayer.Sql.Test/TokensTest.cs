using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using myNote.Model;

namespace myNote.DataLayer.Sql.Test
{
    [TestClass]
    public class TokensTest
    {
        private const string ConnectionString = IoC.ConnectionString;
        private readonly Dictionary<string, Token> tempUsersLogin = new Dictionary<string, Token>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenTryToAccess()
        {
            //arrange
            var factory = new CreatingUserClass("Test");
            var user = factory.User;
            var token = factory.Token;
            tempUsersLogin.Add(user.Login, token);
            var factory2 = new CreatingUserClass("Test2");
            var user2 = factory2.User;
            var token2 = factory2.Token;
            tempUsersLogin.Add(user2.Login, token2);

            //act
            var notesRepository = new NotesRepository(ConnectionString);
            notesRepository.CreateNote(new Model.Note { UserId = user2.Id }, token);
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
