using myNote.Api.Filters;
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

        public GroupController()
        {
            groupsRepository = new GroupsRepository(IoC.ConnectionString);
            usersRepository = new UsersRepository(IoC.ConnectionString, groupsRepository);
        }
        /// <summary>
        /// Создание группы пользователя
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="name">Название группы</param>
        /// <param name="accessToken">Токен доступа</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/groups/{name}")]
        [ArgumentExceptionFilter]
        public Group Post(string name, [FromBody]Token accessToken)
        {
            try
            {
                var result = groupsRepository.CreateGroup(name, accessToken);
                Logger.Log.Instance.Info($"Создание группы с id: {result.Id}");
                return result;
            }
            catch (ArgumentException e)
            {
                Logger.Log.Instance.Error(e.Message);
                throw new ArgumentException(e.Message);
            }
            catch(Exception e)
            {
                Logger.Log.Instance.Error(e.Message);
                throw;
            }
        }

        /// <summary>
        /// Удаление группы по ее идентификатору
        /// </summary>
        /// <param name="id">Идентификатор группы</param>
        /// <param name="accessToken">Токен доступа</param>
        [HttpPut]
        [Route("api/groups/deletebyid/{id}")]
        [ArgumentExceptionFilter]
        public void DeleteById(Guid id, [FromBody]Token accessToken)
        {
            try
            {
                Logger.Log.Instance.Info($"Удаление группы с id: {id}");
                groupsRepository.DeleteGroup(id, accessToken);
            }
            catch (ArgumentException e)
            {
                Logger.Log.Instance.Error(e.Message);
                throw new ArgumentException(e.Message);
            }
        }

        /// <summary>
        /// Удаление группы
        /// </summary>
        /// <param name="userId">Идентификатор пользователя</param>
        /// <param name="name">Название группы</param>
        /// <param name="accessToken">Токен доступа</param>
        [HttpPut]
        [Route("api/groups/deletebyname/{name}")]
        [ArgumentExceptionFilter]
        public void Delete(string name, [FromBody]Token accessToken)
        {
            try
            {
                Logger.Log.Instance.Info($"Удаление группы с userId: {accessToken.UserId} и именем: {name}");
                groupsRepository.DeleteGroup(name, accessToken);
            }
            catch (ArgumentException e)
            {
                Logger.Log.Instance.Error(e.Message);
                throw new ArgumentException(e.Message);
            }
        }
    }
}
