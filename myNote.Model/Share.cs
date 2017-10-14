using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote
{
    public class Share
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }

        public Guid NoteId { get; set; }
    }
}
