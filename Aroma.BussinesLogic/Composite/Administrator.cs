using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Composite
{
    public class Administrator : IEmployee
    {
        public string Name { get; set; }
        private List<IEmployee> _employees = new List<IEmployee>();
        public Administrator(string Name) 
        {
         this.Name = Name;
        }
        public void AddUsers(Users user)
        {
            _employees.Add(user);
        }
        public void ShowInfo()
        {
            Console.WriteLine($"Administrator is {Name}");
            foreach (var user in _employees) {

                user.ShowInfo();
            }

        }
    }
}
