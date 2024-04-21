using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.User
{
    public class ULoginData
    {
        public int Id { get; set; }
        public string credential { get; set; }
        public string password { get; set; }

        public string IP { get; set; }

        public DateTime FirstLoginTime { get; set; }

        public string Email { get; set; }

        public decimal Balance { get; set; }
    }
    
}
