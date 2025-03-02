using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Adapter
{
    public class PostgreAdapter:IDatabase
    {
        protected readonly PostgreSQL _postgreSQL;

        public PostgreAdapter(PostgreSQL postgreSQL) => _postgreSQL = postgreSQL;

        public void AddUser(UDbTable User)
        {

            _postgreSQL.AddPotgreLocation(User.LastIP);
        }

        public void DeleteUser()
        {
            throw new NotImplementedException();
        }

        public void DeleteUser(int id)
        {
            throw new NotImplementedException();
        }

        public void UpdateUser()
        {
            throw new NotImplementedException();
        }
    }
}
