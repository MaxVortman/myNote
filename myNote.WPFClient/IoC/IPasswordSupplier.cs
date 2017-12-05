using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace myNote.WPFClient.IoC
{
    public interface IPasswordSupplier
    {
        string GetPassword();
    }
}
