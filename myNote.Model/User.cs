using System;
using System.Collections.Generic;

namespace myNote.Model
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public string Email { get; set; }

        public DateTime RegisterDate { get; set; }

        public IEnumerable<Group> UserGroups { get; set; }
    }
}
