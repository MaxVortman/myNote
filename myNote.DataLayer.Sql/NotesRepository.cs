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
        private readonly string connectionString;

        public NotesRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Note CreateNote(Note note)
        {
            var db = new DataContext(connectionString);
            note.Id = Guid.NewGuid();
            db.GetTable<Note>().InsertOnSubmit(note);
            db.SubmitChanges();
            return note;
        }

        public void DeleteNote(Guid id)
        {
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

        public Note UpdateNote(Note note)
        {
            var db = new DataContext(connectionString);
            var noteFromDb = (from n in db.GetTable<Note>()
                              where n.Id == note.Id
                              select n).FirstOrDefault();
            if (noteFromDb == default(Note))
                throw new ArgumentException($"Заметка с id {note.Id} не найдена");
            noteFromDb = note;
            db.SubmitChanges();
            return noteFromDb;
        }
    }
}
