using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer
{
    public interface IUsersRepository
    {
        User CreateUser(User user);
        User GetUser(Guid id, Token accessToken);
        User GetUser(string Login, Token accessToken);
        User UpdateUser(User user, Token accessToken);
    }
}
