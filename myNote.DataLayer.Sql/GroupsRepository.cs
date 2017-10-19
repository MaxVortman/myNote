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
    public class GroupsRepository : IGroupsRepository
    {
        private readonly string connectionString;
            
        public GroupsRepository(string connectionString)
        {
            this.connectionString = connectionString;
        }

        public Group CreateGroup(Guid userId, string name)
        {
            var db = new DataContext(connectionString);
            var group = new Group
            {
                Id = Guid.NewGuid(),
                Name = name,
                UserId = userId
            };
            db.GetTable<Group>().InsertOnSubmit(group);
            db.SubmitChanges();
            return group;
        }

        public IEnumerable<Group> GetUserGroups(Guid userId)
        {
            var db = new DataContext(connectionString);
            return from g in db.GetTable<Group>()
                   where g.UserId == userId
                   select g;
        }

        public Group GetGroup(Guid groupId)
        {
            var db = new DataContext(connectionString);
            var group = (from g in db.GetTable<Group>()
                         where g.Id == groupId
                         select g).FirstOrDefault();
            if (group == default(Group))
                throw new ArgumentException($"Нет группы с id {groupId}");
            return group;
        }

        public void DeleteGroup(Guid id)
        {
            var noteGroupsRepository = new NoteGroupsRepository(connectionString);
            if (noteGroupsRepository.GetAllNoteBy(id).Count() != 0)
                throw new InvalidOperationException("Невозможно удалить группу, в которой есть записи");
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "delete from Groups where Id = @Id";
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteGroup(Guid userId, string name)
        {
            var noteGroupsRepository = new NoteGroupsRepository(connectionString);
            if (noteGroupsRepository.GetAllNoteBy(userId, name).Count() != 0)
                throw new InvalidOperationException("Невозможно удалить группу, в которой есть записи");
            using (var sqlConnection = new SqlConnection(connectionString))
            {
                sqlConnection.Open();
                using (var command = sqlConnection.CreateCommand())
                {
                    command.CommandText = "delete from Groups where UserId = @userId and Name = @name";
                    command.Parameters.AddWithValue("@userId", userId);
                    command.Parameters.AddWithValue("@name", name);
                    command.ExecuteNonQuery();
                }
            }
        }

        public Group GetGroup(Guid userId, string name)
        {
            var db = new DataContext(connectionString);
            var group = (from g in db.GetTable<Group>()
                         where g.UserId == userId && g.Name == name
                         select g).FirstOrDefault();
            if (group == default(Group))
                throw new ArgumentException($"Нет группы с user id {userId} и name {name}");
            return group;
        }
    }
}
