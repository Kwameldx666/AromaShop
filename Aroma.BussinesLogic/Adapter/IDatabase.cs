using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Adapter
{
    public interface IDatabase
    {
        void AddUser(UDbTable user);
        void UpdateUser();
        void DeleteUser();
        void DeleteUser(int id);

    }
}
