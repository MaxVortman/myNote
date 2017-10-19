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
        [Route("api/users/{userId}/groups/{name}")]
        public Group Post(Guid userId, string name)
        {
            return groupsRepository.CreateGroup(userId, name);
        }

        /// <summary>
        /// Удаление группы по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор группы</param>
        [HttpDelete]
        [Route("api/groups/{id}")]
        public void DeleteById(Guid id)
        {
            groupsRepository.DeleteGroup(id);
        }

        /// <summary>
        /// Удаление группы
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="name">Название группы</param>
        [HttpDelete]
        [Route("api/users/{userId}/groups/{name}")]
        public void Delete(Guid userId, string name)
        {
            groupsRepository.DeleteGroup(userId, name);
        }
    }
}
