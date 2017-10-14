using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.Model
{
    [Table(Name = "Groups")]
    public class Group
    {
        [Column(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public string Name { get; set; }
        [Column]
        public Guid UserId { get; set; }
    }
}
