using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace myNote.DataLayer.Sql.Test
{
    [TestClass]
    public class TokensTest
    {
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";
        private readonly List<string> tempUsersLogin = new List<string>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void ShouldThrowExceptionWhenTryToAccess()
        {
            //arrange
            var user = HelpingClass.CreateUser();
            var token = HelpingClass.GetToken();
            var anotherUser = HelpingClass.CreateAnotherUser();
            tempUsersLogin.Add(user.Login);
            tempUsersLogin.Add(anotherUser.Login);

            //act
            var notesRepository = new NotesRepository(ConnectionString);
            notesRepository.CreateNote(new Model.Note { UserId = anotherUser.Id }, token);
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
