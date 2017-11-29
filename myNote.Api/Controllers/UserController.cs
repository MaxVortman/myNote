using myNote.Api.Filters;
using myNote.DataLayer;
using myNote.DataLayer.Sql;
using myNote.Model;
using Newtonsoft.Json.Linq;
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
        private IUsersRepository usersRepository;
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";

        public UserController()
        {
            usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
        }
        
        /// <summary>
        /// Получение пользователя по уникальному идентификатору
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="accessToken">Токен доступа</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/users/{id}")]
        [ArgumentExceptionFilter]
        public User Get(Guid id, [FromBody]Token accessToken)
        {
            try
            {
                return usersRepository.GetUser(id, accessToken);
            }
            catch (ArgumentException e)
            {
                Logger.Log.Instance.Error(e.Message);
                throw new ArgumentException(e.Message);
            }
        }

        /// <summary>
        /// Получение групп пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <param name="accessToken">Токен доступа</param>
        /// <returns>Группы пользователя</returns>
        [HttpPut]
        [Route("api/users/{id}/groups")]
        public IEnumerable<Group> GetUserGroups(Guid id, [FromBody]Token accessToken)
        {
            return usersRepository.GetUser(id, accessToken).UserGroups;
        }        
        /// <summary>
        /// Обновление данных о пользователе
        /// </summary>
        /// <param name="userAndToken">Новые данные пользователя и токен доступа в json</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/users/update")]
        [ArgumentExceptionFilter]
        public User Update([FromBody] JObject userAndToken)
        {
            var user = userAndToken["user"].ToObject<User>();
            var accessToken = userAndToken["accessToken"].ToObject<Token>();
            Logger.Log.Instance.Info("Изменение пользователя с именем: {0}", user.Name);
            try
            {
                return usersRepository.UpdateUser(user, accessToken);
            }
            catch (ArgumentException e)
            {
                Logger.Log.Instance.Error(e.Message);
                throw new ArgumentException(e.Message);
            }
        }
    }
}
