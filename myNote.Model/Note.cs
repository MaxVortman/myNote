using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.Model
{
    [Table(Name = "Notes")]
    public class Note
    {
        [Column(IsPrimaryKey = true)]
        public Guid Id { get; set; }
        [Column]
        public string Title { get; set; }
        [Column]
        public string Content { get; set; }
        [Column]
        public DateTime? CreationDate { get; set; }
        [Column]
        public DateTime? ChangeDate { get; set; }
        [Column]
        public Guid UserId { get; set; }
    }
}
