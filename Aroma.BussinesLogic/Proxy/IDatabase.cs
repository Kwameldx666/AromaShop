using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Proxy
{
    public interface IDatabase
    {
        void Request(UserRole role);
    }
}
