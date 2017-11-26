using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer
{
    public interface ITokensRepository
    {
        Token CreateToken(Guid userId);
        void CompareToken(Token expectedToken, Guid userId);
        void CompareToken(Token expectedToken);
    }
}
