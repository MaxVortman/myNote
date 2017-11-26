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
        #region Private Properties

        private readonly string connectionString;

        #endregion

        #region Constructor

        public SharesRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #endregion

        #region Create Share

        public Share CreateShare(Note note, Token accessToken)
        {
            new TokensRepository(connectionString).CompareToken(accessToken, note.UserId);

            var db = new DataContext(connectionString);
            var share = new Share { NoteId = note.Id, UserId = note.UserId };
            db.GetTable<Share>().InsertOnSubmit(share);
            db.SubmitChanges();
            return share;
        }

        #endregion

        #region Get Shares

        public IEnumerable<Note> GetAllUserSharesNotes(Guid userId)
        {
            var db = new DataContext(connectionString);
            var notesRepository = new NotesRepository(connectionString);
            return from s in db.GetTable<Share>()
                   where s.UserId == userId
                   select notesRepository.GetNote(s.NoteId);
        }

        #endregion

        #region Helping methods

        public bool IsNoteShared(Guid noteId)
        {
            var db = new DataContext(connectionString);
            return (from s in db.GetTable<Share>()
                    where s.NoteId == noteId
                    select s).FirstOrDefault() == default(Share) ? false : true;
        }

        #endregion
    }
}
