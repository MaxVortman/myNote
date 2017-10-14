using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer
{
    public interface INotesRepository
    {
        Note CreateNote(Note note);
        void DeleteNote(Guid id);
        IEnumerable<Note> GetUserNotes(Guid userId);
        Note UpdateNote(Note note);
        Note GetNote(Guid noteId);
    }
}
