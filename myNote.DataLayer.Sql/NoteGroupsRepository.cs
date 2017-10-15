using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using myNote.Model;
using System.Data.Linq;

namespace myNote.DataLayer.Sql
{
    public class NoteGroupsRepository : INoteGroupsRepository
    {
        private readonly string connectionString;

        public NoteGroupsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public NoteGroup CreateNoteGroup(Guid noteId, Guid groupId)
        {
            var db = new DataContext(connectionString);
            var noteGroup = new NoteGroup { GroupId = groupId, NoteId = noteId };
            db.GetTable<NoteGroup>().InsertOnSubmit(noteGroup);
            db.SubmitChanges();
            return noteGroup;
        }

        public IEnumerable<Note> GetAllNoteBy(Guid groupId)
        {
            var db = new DataContext(connectionString);
            var notesRepository = new NotesRepository(connectionString);
            return from ng in db.GetTable<NoteGroup>()
                   where ng.GroupId == groupId
                   select notesRepository.GetNote(ng.NoteId);
        }

        public Group GetGroupBy(Guid noteId)
        {
            var db = new DataContext(connectionString);
            var groupsRepository = new GroupsRepository(connectionString);
            var noteGroup = (from ng in db.GetTable<NoteGroup>()
                             where ng.NoteId == noteId
                             select ng).FirstOrDefault();
            if (noteGroup == default(NoteGroup))
                throw new ArgumentException($"Группы с заметкой {noteId} нет.");
            return groupsRepository.GetGroup(noteGroup.GroupId);
        }
    }
}
