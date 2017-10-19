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
        /// <param name="noteId">Идентификатор заметки</param>
        /// <param name="groupId">Идентификатор группы</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/users/groups/{groupId}/notes/{noteId}")]
        public NoteGroup Post(Guid noteId, Guid groupId)
        {
            return noteGroupsRepository.CreateNoteGroup(noteId, groupId);
        }
    }
}
