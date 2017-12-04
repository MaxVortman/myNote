using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer
{
    public interface INoteGroupsRepository
    {
        NoteGroup CreateNoteGroup(NoteGroup noteGroup, Token accessToken);
        Group GetGroupBy(Guid noteId);
        IEnumerable<Note> GetAllNoteBy(Guid groupId, Token accessToken);
        IEnumerable<Note> GetAllNoteBy(string name, Token accessToken);
        void Delete(Guid noteId, Token accessToken);
    }
}
