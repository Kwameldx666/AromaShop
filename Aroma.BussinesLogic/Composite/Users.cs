using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Composite
{
    public class Users:IEmployee
    
    {


        public string Name {  get; set; }
        public UserRole Role { get; set; }
        public Users(string Name, UserRole role)
        {
            this.Name = Name;
            Role = role;
        }
        
        public void ShowInfo()
        {
            Console.WriteLine($"Username: {Name}, his role is {Role} ");
        }
    }

}
