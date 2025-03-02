using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Adapter
{
    public class PostgreSQL
    {
        public void AddPotgreLocation(string location)
        {
            Console.WriteLine("From PostgreSQL Location is " + location);
            
        }
    }
}