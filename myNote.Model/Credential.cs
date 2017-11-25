using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.Model
{
    [Table(Name = "Credentials")]
    public class Credential
    {
        [Column(IsPrimaryKey = true)]
        public string Login { get; set; }
        [Column]
        public byte[] Password { get; set; }
    }
}
