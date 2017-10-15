using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myNote.Model;
using System.Data.Linq;

namespace myNote.DataLayer.Sql
{
    public class SharesRepository : ISharesRepository
    {
        private readonly string connectionString;

        public SharesRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Share CreateShare(Note note)
        {
            var db = new DataContext(connectionString);
            var share = new Share { NoteId = note.Id, UserId = note.UserId };
            db.GetTable<Share>().InsertOnSubmit(share);
            db.SubmitChanges();
            return share;
        }

        public IEnumerable<Note> GetAllUserSharesNotes(Guid userId)
        {
            var db = new DataContext(connectionString);
            var notesRepository = new NotesRepository(connectionString);
            return from s in db.GetTable<Share>()
                   where s.UserId == userId
                   select notesRepository.GetNote(s.NoteId);
        }

        public bool IsNoteShared(Guid noteId)
        {
            var db = new DataContext(connectionString);
            return (from s in db.GetTable<Share>()
                    where s.NoteId == noteId
                    select s).FirstOrDefault() == default(Share) ? false : true;
        }
    }
}
