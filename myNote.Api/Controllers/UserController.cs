using myNote.DataLayer.Sql;
using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace myNote.Api.Controllers
{
    /// <summary>
    /// Управление пользователями
    /// </summary>
    public class UserController : ApiController
    {
        private UsersRepository usersRepository;
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";

        public UserController()
        {
            usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
        }
        
        /// <summary>
        /// Получение пользователя по уникальному идентификатору
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/users/{id}")]
        public User Get(Guid id)
        {
            return usersRepository.GetUser(id);
        }
        /// <summary>
        /// Создание пользователя
        /// </summary>
        /// <param name="user">Пользователь</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/users")]
        public User Post([FromBody] User user)
        {
            return usersRepository.CreateUser(user);
        }
        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        [HttpDelete]
        [Route("api/users/{id}")]
        public void Delete(Guid id)
        {
            usersRepository.DeleteUser(id);
        }
        /// <summary>
        /// Получение групп пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns>Группы пользователя</returns>
        [HttpGet]
        [Route("api/users/{id}/groups")]
        public IEnumerable<Group> GetUserGroups(Guid id)
        {
            return usersRepository.GetUser(id).UserGroups;
        }        
    }
}
