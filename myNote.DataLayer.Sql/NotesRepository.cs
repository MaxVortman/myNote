using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myNote.Model;
using System.Data.SqlClient;

namespace myNote.DataLayer.Sql
{
    public class NotesRepository : INotesRepository
    {
        public Note CreateNote(Note note)
        {
            using (var sqlConnection = new SqlConnection())
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "insert into Notes (Id, Title, Content, CreationDate, ChangeDate, UserId) values (@Id, @Title, @Content, @CreationDate, @ChangeDate, @UserId)";

                    note.Id = Guid.NewGuid();
                    command.Parameters.AddWithValue("@Id", note.Id);
                    command.Parameters.AddWithValue("@Title", note.Title);
                    command.Parameters.AddWithValue("@Content", note.Content);

                    if (note.CreationDate != default(DateTime))
                        command.Parameters.AddWithValue("@CreationDate", note.CreationDate);
                    else
                        command.Parameters.AddWithValue("@CreationDate", DBNull.Value);

                    if (note.ChangeDate != default(DateTime))
                        command.Parameters.AddWithValue("@ChangeDate", note.ChangeDate);
                    else
                        command.Parameters.AddWithValue("@ChangeDate", DBNull.Value);

                    command.Parameters.AddWithValue("@UserId", note.UserId);

                    command.ExecuteNonQuery();

                    return note;
                }
            }
        }

        public void DeleteNote(Guid id)
        {
            using (var sqlConnection = new SqlConnection())
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
            using (var sqlConnection = new SqlConnection())
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "select * from Notes where Id = @Id";
                    command.Parameters.AddWithValue("@Id", noteId);

                    using (var reader = command.ExecuteReader())
                    {
                        if (!reader.Read())
                            throw new ArgumentException($"Заметка с id {noteId} не найдена");

                        Note note = ExtractNoteFromDb(reader);

                        return note;
                    }
                }
            }
        }

        private static Note ExtractNoteFromDb(SqlDataReader reader)
        {
            var note = new Note
            {
                Id = reader.GetGuid(reader.GetOrdinal("Id")),
                UserId = reader.GetGuid(reader.GetOrdinal("UserId"))
            };
            if (!reader.IsDBNull(reader.GetOrdinal("Title")))
                note.Title = reader.GetString(reader.GetOrdinal("Title"));
            if (!reader.IsDBNull(reader.GetOrdinal("Content")))
                note.Content = reader.GetString(reader.GetOrdinal("Content"));
            if (!reader.IsDBNull(reader.GetOrdinal("CreationDate")))
                note.CreationDate = reader.GetDateTime(reader.GetOrdinal("CreationDate"));
            if (!reader.IsDBNull(reader.GetOrdinal("ChangeDate")))
                note.ChangeDate = reader.GetDateTime(reader.GetOrdinal("ChangeDate"));
            return note;
        }

        public IEnumerable<Note> GetUserNotes(Guid userId)
        {
            using (var sqlConnection = new SqlConnection())
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "select * from Notes where UserId = @userId";
                    command.Parameters.AddWithValue("@userId", userId);

                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            yield return ExtractNoteFromDb(reader);
                        }
                    }
                }
            }
        }

        public Note UpdateNote(Note note)
        {
            using (var sqlConnection = new SqlConnection())
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {                    
                    command.CommandText = "update Notes set Title = @Title, Content = @Content, ChangeDate = @ChangeDate where Id = @Id";
                    note.ChangeDate = DateTime.Now;
                    command.Parameters.AddWithValue("@Title", note.Title);
                    command.Parameters.AddWithValue("@Content", note.Content);
                    command.Parameters.AddWithValue("@Id", note.Id);
                    command.Parameters.AddWithValue("@ChangeDate", note.ChangeDate);

                    command.ExecuteNonQuery();

                    return note;
                }
            }
        }
    }
}
