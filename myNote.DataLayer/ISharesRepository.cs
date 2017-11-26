using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer
{
    public interface ISharesRepository
    {
        Share CreateShare(Note note, Token accessToken);
        IEnumerable<Note> GetAllUserSharesNotes(Guid userId);
        bool IsNoteShared(Guid noteId);
    }
}
