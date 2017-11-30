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
    /// Управление раздачами
    /// </summary>
    public class ShareController : ApiController
    {
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";
        private ISharesRepository sharesRepository;

        public ShareController()
        {
            sharesRepository = new SharesRepository(ConnectionString);
        }

        /// <summary>
        /// Создание раздачи заметки
        /// </summary>
        /// <param name="noteAndToken">Заметка и токен доступа</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/shares")]
        public Share Post([FromBody] JObject noteAndToken)
        {
            var note = noteAndToken["note"].ToObject<Note>();
            var accessToken = noteAndToken["accessToken"].ToObject<Token>();
            Logger.Log.Instance.Info("Создание раздачи заметки с id: {0}", note.Id);
            return sharesRepository.CreateShare(note, accessToken);
        }

        /// <summary>
        /// Получение всех раздач пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/shares/user/{id}")]
        public IEnumerable<Note> GetUserSharesNotes(Guid id)
        {
            return sharesRepository.GetAllUserSharesNotes(id);
        }   

        /// <summary>
        /// Получение определенного колличества "раздач"
        /// </summary>
        /// <param name="count">необходимое число раздач</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/shares/{count}")]
        public IEnumerable<Note> GetSomeShares(int count)
        {
            return sharesRepository.GetSomeShares(count);
        }
    }
}
