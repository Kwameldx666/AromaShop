using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Proxy
{
    public class StandartDb:IDatabase
    {
        private UserRole _role;
        public StandartDb(UserRole role)
        {
            this._role = role;
        }
        
        public void Request(UserRole role)
        {
            Console.WriteLine("Success");
        }
    }
    public class AdminDb:IDatabase
    {
        private UserRole _role;
        public AdminDb(UserRole role)
        {
            this._role = role;
        }
        
        public void Request(UserRole role)
        {
            if(role == UserRole.Admin)
            {
                Console.WriteLine("Success");
            }    
            else
            Console.WriteLine("Error");
        }
    }
}
