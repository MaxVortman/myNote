using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myNote.Model;
using System.Data.SqlClient;
using System.Data.Linq;

namespace myNote.DataLayer.Sql
{
    public class NotesRepository : INotesRepository
    {
        #region Private Properties

        private readonly string connectionString;

        #endregion

        #region Constructor

        public NotesRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #endregion

        #region Create Note

        public Note CreateNote(Note note, Token accessToken)
        {
            new TokensRepository(connectionString).CompareToken(accessToken, note.UserId);

            var db = new DataContext(connectionString);
            note.Id = Guid.NewGuid();
            db.GetTable<Note>().InsertOnSubmit(note);
            db.SubmitChanges();
            return note;
        }

        #endregion

        #region Delete Note

        public void DeleteNote(Guid id, Token accessToken)
        {
            new TokensRepository(connectionString).CompareToken(accessToken, GetNote(id).UserId);

            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "delete from Notes where Id = @Id";
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        #endregion

        #region Get Note

        public Note GetNote(Guid noteId)
        {
            var db = new DataContext(connectionString);
            var note = (from n in db.GetTable<Note>()
                        where n.Id == noteId
                        select n).FirstOrDefault();
            if(note == default(Note))
                throw new ArgumentException($"Заметка с id {noteId} не найдена");
            return note;
        }

        public IEnumerable<Note> GetUserNotes(Guid userId)
        {
            var db = new DataContext(connectionString);
            return from n in db.GetTable<Note>()
                   where n.UserId == userId
                   select n;
        }

        #endregion

        #region Update Note

        public Note UpdateNote(Note note, Token accessToken)
        {
            new TokensRepository(connectionString).CompareToken(accessToken, note.UserId);

            var db = new DataContext(connectionString);
            var noteFromDb = (from n in db.GetTable<Note>()
                              where n.Id == note.Id
                              select n).FirstOrDefault();
            if (noteFromDb == default(Note))
                throw new ArgumentException($"Заметка с id {note.Id} не найдена");
            UpdateNote(note, noteFromDb);
            db.SubmitChanges();
            return noteFromDb;
        }

        private void UpdateNote(Note sourceNote, Note destinationNote)
        {
            destinationNote.Title = sourceNote.Title;
            destinationNote.UserId = sourceNote.UserId;
            destinationNote.CreationDate = sourceNote.CreationDate;
            destinationNote.Content = sourceNote.Content;
            destinationNote.ChangeDate = DateTime.Now;
        }

        #endregion
    }
}
