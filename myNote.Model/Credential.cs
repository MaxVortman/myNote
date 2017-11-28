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
        /// <summary>
        /// User's Login
        /// </summary>
        [Column(IsPrimaryKey = true)]
        public string Login { get; set; }
        /// <summary>
        /// Encrypted User's Password
        /// </summary>
        [Column]
        public byte[] Password { get; set; }
    }
}
