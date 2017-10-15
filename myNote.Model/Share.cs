using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.Model
{
    [Table(Name = "Shares")]
    public class Share
    {
        [Column(IsPrimaryKey = true)]
        public Guid UserId { get; set; }
        [Column(IsPrimaryKey = true)]
        public Guid NoteId { get; set; }
    }
}
