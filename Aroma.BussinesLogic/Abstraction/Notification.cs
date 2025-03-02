using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BusinessLogic.Abstraction
{
    public abstract class Notification
    {
        protected IMessagerBuilder messager;
        public Notification(IMessagerBuilder messager)
        {
            this.messager = messager;
        }
         public abstract void SendNotification(string message);
    }
}
