﻿using myNote.Model;
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer.Sql
{
    public class UsersRepository : IUsersRepository
    {
        #region Private Properties

        private readonly string connectionString;
        private readonly IGroupsRepository groupsRepository;

        #endregion

        #region Constructor

        public UsersRepository(string connectionString, IGroupsRepository groupsRepository)
        {
            this.connectionString = connectionString;
            this.groupsRepository = groupsRepository;            
        }

        #endregion

        #region Create User

        public User CreateUser(User user)
        {
            var db = new DataContext(connectionString);
            var checkUser = (from u in db.GetTable<User>()
                             where u.Email == user.Email
                             select u).FirstOrDefault();
            if (checkUser != default(User))
            {
                throw new ArgumentException($"Пользователь с таким email ({user.Email}) уже существует.");
            }
            user.Id = Guid.NewGuid();
            db.GetTable<User>().InsertOnSubmit(user);
            db.SubmitChanges();
            return user;
        }

        #endregion

        #region Get User

        public User GetUser(Guid id, Token accessToken)
        {
            //access check
            new TokensRepository(connectionString).CompareToken(accessToken);

            var db = new DataContext(connectionString);

            var user = (from u in db.GetTable<User>()
                        where u.Id == id
                        select u).FirstOrDefault();
            if (user == default(User))            
                throw new ArgumentException($"Пользователь с id {id} не найден");
            user.UserGroups = groupsRepository.GetUserGroups(user.Id);
            return user;
        }

        public User GetUser(string login)
        {
            var db = new DataContext(connectionString);

            var user = (from u in db.GetTable<User>()
                        where u.Login == login
                        select u).FirstOrDefault();
            if (user == default(User))
                throw new ArgumentException($"Пользователь {login} не найден");
            user.UserGroups = groupsRepository.GetUserGroups(user.Id);
            return user;
        }

        #endregion

        #region Update User

        public User UpdateUser(User user, Token accessToken)
        {
            //access check
            new TokensRepository(connectionString).CompareToken(accessToken, user.Id);

            var db = new DataContext(connectionString);

            var userFromDb = (from u in db.GetTable<User>()
                              where u.Id == user.Id
                              select u).FirstOrDefault();
            if (userFromDb == default(User))
                throw new ArgumentException($"Пользователь с id {user.Id} не найден");
            UpdateUserContent(user, userFromDb);
            db.SubmitChanges();
            return userFromDb;
        }

        private void UpdateUserContent(User sourceUser, User destinationUser)
        {
            destinationUser.Birthday = sourceUser.Birthday;
            destinationUser.Email = sourceUser.Email;
            destinationUser.Name = sourceUser.Name;
            destinationUser.UserGroups = sourceUser.UserGroups;
        }

        #endregion
    }
}
