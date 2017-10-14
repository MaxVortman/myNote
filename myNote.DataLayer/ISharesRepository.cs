using myNote.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.DataLayer
{
    public interface ISharesRepository
    {
        IEnumerable<Share> GetAllUserShares(Guid userId);
    }
}
