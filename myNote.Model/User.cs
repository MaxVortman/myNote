using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote
{
    public class User
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public string Email { get; set; }

        public DateTime RegisterDate { get; set; }
    }
}
