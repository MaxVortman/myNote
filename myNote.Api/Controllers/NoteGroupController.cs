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
    /// Управление группами заметок
    /// </summary>
    public class NoteGroupController : ApiController
    {
        private NoteGroupsRepository noteGroupsRepository;
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";

        public NoteGroupController()
        {
            noteGroupsRepository = new NoteGroupsRepository(ConnectionString);
        }
        /// <summary>
        /// Создание группы заметок
        /// </summary>
        /// <param name="noteGroupAndToken"></param>
        /// <param name="accessToken">Токен доступа</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/notegroup")]
        public NoteGroup Post([FromBody]JObject noteGroupAndToken)
        {
            var noteGroup = noteGroupAndToken["noteGroup"].ToObject<NoteGroup>();
            var accessToken = noteGroupAndToken["accessToken"].ToObject<Token>();
            Logger.Log.Instance.Info($"Создание группы заметок с noteId: {noteGroup.NoteId} и groupId: {noteGroup.GroupId}");
            return noteGroupsRepository.CreateNoteGroup(noteGroup, accessToken);
        }
        /// <summary>
        /// Получение группы заметки
        /// </summary>
        /// <param name="id">Идентификатор заметки</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/notegroups/notes/{id}")]
        [ArgumentExceptionFilter]
        public Group Get(Guid id)
        {
            try
            {
                return noteGroupsRepository.GetGroupBy(id);
            }
            catch (ArgumentException e)
            {
                Logger.Log.Instance.Error(e.Message);
                return null;
            }
        }

        /// <summary>
        /// Получение всех заметок в группе
        /// </summary>
        /// <param name="groupId">Идентификатор группы</param>
        /// <param name="accessToken">Токен доступа</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/notegroups/group/{groupId}")]
        public IEnumerable<Note> GetAllNotesBy(Guid groupId, [FromBody]Token accessToken)
        {
            return noteGroupsRepository.GetAllNoteBy(groupId, accessToken);
        }

        /// <summary>
        /// Получение всех заметок в группе
        /// </summary>
        /// <param name="accessToken">Токен доступа</param>
        /// <param name="name">Название группы</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/notegroups/user/group/{name}")]
        public IEnumerable<Note> GetAllNotesBy(string name, [FromBody]Token accessToken)
        {
            return noteGroupsRepository.GetAllNoteBy(name, accessToken);
        }

        /// <summary>
        /// Удаление заметки из группы
        /// </summary>
        /// <param name="noteId"></param>
        /// <param name="accessToken"></param>
        [HttpPut]
        [Route("api/notegroup/delete/{noteId}")]
        public void Delete(Guid noteId, [FromBody]Token accessToken)
        {
            noteGroupsRepository.Delete(noteId, accessToken);
        }
    }
}
