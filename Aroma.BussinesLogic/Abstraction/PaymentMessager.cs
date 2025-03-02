using Aroma.BusinessLogic.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BusinessLogic.Abstraction
{
    public class PaymentMessager : IMessagerBuilder
    {
        public void SendMessage(string message)
        {
            Console.WriteLine("Thanks dear customer " + message );
        }
    }
}
