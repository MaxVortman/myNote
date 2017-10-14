using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;

namespace myNote.Model
{
    [Table(Name = "Users")]
    public class User
    {
        [Column(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public DateTime? Birthday { get; set; }
        [Column]
        public string Email { get; set; }
        [Column]
        public DateTime? RegisterDate { get; set; }

        public IEnumerable<Group> UserGroups { get; set; }
    }
}
