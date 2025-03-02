using Aroma.BusinessLogic.Abstraction;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BusinessLogic.Abstraction
{
    public class MailNotif:Notification
    {
        public MailNotif(IMessagerBuilder messager) : base(messager){ }

        public override void SendNotification(string message)
        {
            messager.SendMessage(message);
        }
    }
}
