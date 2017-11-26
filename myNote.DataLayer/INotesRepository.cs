using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer
{
    public interface INotesRepository
    {
        Note CreateNote(Note note, Token accessToken);
        void DeleteNote(Guid id, Token accessToken);
        IEnumerable<Note> GetUserNotes(Guid userId);
        Note UpdateNote(Note note, Token accessToken);
        Note GetNote(Guid noteId);
    }
}
