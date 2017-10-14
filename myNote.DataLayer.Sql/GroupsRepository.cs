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
    }
}
