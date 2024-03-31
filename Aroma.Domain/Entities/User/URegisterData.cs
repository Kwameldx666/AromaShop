using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.User
{
    public class URegisterData
    {
        public string Name {  get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateTime RegDate { get; set; }
        public string IP {  get; set; }
    }
}
