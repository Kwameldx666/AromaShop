using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.User
{
    public  class UserMinimal
    {
        
        public int Id { get; set; }

     
        public string Username { get; set; }

   



 
        public string Email { get; set; }


        public DateTime LastLogin { get; set; }


        public string LastIP { get; set; }

        public UserRole Level { get; set; }
    }
}
