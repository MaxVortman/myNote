using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.Model
{
    [Table(Name = "Tokens")]
    public class Token
    {
        [Column(IsPrimaryKey = true)]
        public Guid UserId { get; set; }
        [Column (Name = "TokenKey")]
        public string Key { get; set; }
    }
}
