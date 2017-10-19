using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer
{
    public interface IGroupsRepository
    {
        Group CreateGroup(Guid userId, string name);
        IEnumerable<Group> GetUserGroups(Guid userId);
        Group GetGroup(Guid id);
        Group GetGroup(Guid userId, string name);
        void DeleteGroup(Guid id);
        void DeleteGroup(Guid userId, string name);
    }
}