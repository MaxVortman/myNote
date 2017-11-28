using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer
{
    public interface ICredentialsRepository
    {
        Token Login(Credential credential);
        void Register(Credential credential);
        void Delete(string login, Token accessToken);
    }
}
