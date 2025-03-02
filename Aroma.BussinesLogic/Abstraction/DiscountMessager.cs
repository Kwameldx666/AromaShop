using Aroma.BusinessLogic.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BusinessLogic.Abstraction
{
    public class DiscountMessager : IMessagerBuilder
    {
        public void SendMessage(string message)
        {
           Console.WriteLine("Your discount is " + message + "$");
        }
    }
}
