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
    /// Управление заметками
    /// </summary>
    public class NoteController : ApiController
    {
        private IUsersRepository usersRepository;
        private INotesRepository notesRepository;
        private const string ConnectionString = @"Data Source=.\SQLEXPRESS;Initial Catalog=test;Integrated Security=true";

        public NoteController()
        {
            usersRepository = new UsersRepository(ConnectionString, new GroupsRepository(ConnectionString));
            notesRepository = new NotesRepository(ConnectionString);
        }
        /// <summary>
        /// Получение заметки по идентификатору
        /// </summary>
        /// <param name="id">Идентификатор</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/notes/{id}")]
        public Note Get(Guid id)
        {
            return notesRepository.GetNote(id);
        }
        /// <summary>
        /// Создание заметки
        /// </summary>
        /// <param name="note">Заметка</param>
        /// <returns></returns>
        [HttpPost]
        [Route("api/notes")]
        public Note Post([FromBody] Note note)
        {
            return notesRepository.CreateNote(note);
        }
        /// <summary>
        /// Удаление пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        [HttpDelete]
        [Route("api/notes/{id}")]
        public void Delete(Guid id)
        {
            notesRepository.DeleteNote(id);
        }

        /// <summary>
        /// Обновление заметки
        /// </summary>
        /// <param name="note">Заметка</param>
        /// <returns></returns>
        [HttpPut]
        [Route("api/notes/{id}")]
        public Note Put([FromBody] Note note)
        {
            return notesRepository.UpdateNote(note);
        }

        /// <summary>
        /// Получение всех заметок пользователя
        /// </summary>
        /// <param name="id">Идентификатор пользователя</param>
        /// <returns></returns>
        [HttpGet]
        [Route("api/users/{id}/notes")]
        public IEnumerable<Note> GetUserNotes(Guid id)
        {
            return notesRepository.GetUserNotes(id);
        }
    }
}
