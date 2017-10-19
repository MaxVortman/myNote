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
        /// <param name="note">Заметка</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/users/shares")]
        public Share Post([FromBody] Note note)
        {
            return sharesRepository.CreateShare(note);
        }

        /// <summary>
        /// Получение всех раздач пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/users/{id}/shares")]
        public IEnumerable<Note> GetUserSharesNotes(Guid id)
        {
            return sharesRepository.GetAllUserSharesNotes(id);
        }   
    }
}
