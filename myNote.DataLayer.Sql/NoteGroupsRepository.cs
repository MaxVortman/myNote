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
        #region Private Properties

        private readonly string connectionString;

        #endregion

        #region Constructor

        public NoteGroupsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        #endregion

        #region Create Note Group

        public NoteGroup CreateNoteGroup(NoteGroup noteGroup, Token accessToken)
        {
            #region Compare userId from note and from group
            var userIdFormNote = new NotesRepository(connectionString).GetNote(noteGroup.NoteId).UserId;
            var userIdFromGroup = new GroupsRepository(connectionString).GetGroup(noteGroup.GroupId).UserId;
            if (userIdFormNote != userIdFromGroup)
                throw new ArgumentException("The note and group do not belong to the same user");
            #endregion

            new TokensRepository(connectionString).CompareToken(accessToken, new NotesRepository(connectionString).GetNote(noteGroup.NoteId).UserId);

            CheckForContains(noteGroup.NoteId);

            var db = new DataContext(connectionString);            
            db.GetTable<NoteGroup>().InsertOnSubmit(noteGroup);
            db.SubmitChanges();
            return noteGroup;
        }

        #endregion

        #region Help method
        private void CheckForContains(Guid noteId)
        {
            var db = new DataContext(connectionString);
            var noteGroupsFromDb = (from ng in db.GetTable<NoteGroup>()
                                   where ng.NoteId == noteId
                                   select ng).AsEnumerable();
            if (noteGroupsFromDb.Count() > 0)
            {
                db.GetTable<NoteGroup>().DeleteAllOnSubmit(noteGroupsFromDb);
                db.SubmitChanges();
            }            
        }

        #endregion

        #region Get All Note

        public IEnumerable<Note> GetAllNoteBy(Guid groupId, Token accessToken)
        {
            new TokensRepository(connectionString).CompareToken(accessToken, new GroupsRepository(connectionString).GetGroup(groupId).UserId);

            var db = new DataContext(connectionString);
            var notesRepository = new NotesRepository(connectionString);
            return from ng in db.GetTable<NoteGroup>()
                   where ng.GroupId == groupId
                   select notesRepository.GetNote(ng.NoteId);
        }

        public IEnumerable<Note> GetAllNoteBy(string name, Token accessToken)
        {
            var db = new DataContext(connectionString);
            var notesRepository = new NotesRepository(connectionString);
            var groupsRepository = new GroupsRepository(connectionString);
            var groupFromDb = groupsRepository.GetGroup(accessToken.UserId, name);
            return from ng in db.GetTable<NoteGroup>()
                   where ng.GroupId == groupFromDb.Id
                   select notesRepository.GetNote(ng.NoteId);
        }

        #endregion

        #region Get Group from Note

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
        #endregion

        #region Delete

        public void Delete(Guid noteId, Token accessToken)
        {
            new TokensRepository(connectionString).CompareToken(accessToken, new NotesRepository(connectionString).GetNote(noteId).UserId);

            CheckForContains(noteId);
        }

        #endregion
    }
}
