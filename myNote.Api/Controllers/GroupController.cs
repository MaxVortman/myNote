using myNote.DataLayer;
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
    /// Управление группами
    /// </summary>
    public class GroupController : ApiController
    {
        private IGroupsRepository groupsRepository;
        private IUsersRepository usersRepository;
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";

        public GroupController()
        {
            groupsRepository = new GroupsRepository(ConnectionString);
            usersRepository = new UsersRepository(ConnectionString, groupsRepository);
        }
        /// <summary>
        /// Создание группы пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="name">Название группы</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/groups/{name}/user/{userId}")]
        public Group Post(string name, Guid userId, [FromBody]Token accessToken)
        {
            var result = groupsRepository.CreateGroup(userId, name, accessToken);
            Logger.Log.Instance.Info($"Создание группы с id: {result.Id}");
            return result;
        }

        /// <summary>
        /// Удаление группы по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор группы</param>
        [HttpDelete]
        [Route("api/groups/{id}")]
        public void DeleteById(Guid id, [FromBody]Token accessToken)
        {
            Logger.Log.Instance.Info($"Удаление группы с id: {id}");
            groupsRepository.DeleteGroup(id, accessToken);
        }

        /// <summary>
        /// Удаление группы
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="name">Название группы</param>
        [HttpDelete]
        [Route("api/groups/{name}/user/{userId}")]
        public void Delete(string name, Guid userId, [FromBody]Token accessToken)
        {
            Logger.Log.Instance.Info($"Удаление группы с userId: {userId} и именем: {name}");
            groupsRepository.DeleteGroup(userId, name, accessToken);
        }
    }
}
