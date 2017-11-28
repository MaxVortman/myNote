using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.WPFClient.ApiInteractions
{
    public class Registerer
    {
        public Registerer(string login, string password)
        {
            Login = login;
            Password = password;
        }

        public string Login { get; }
        public string Password { get; }

        public void Register()
        {

        }
    }
}
