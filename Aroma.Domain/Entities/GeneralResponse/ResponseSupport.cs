using Aroma.Domain.Entities.Support;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseSupport
    {
        public bool Status { get; set; }
        public string StatusMessage { get; set; }

        public List<USupportForm> SupportMesages { get; set; } // Свойство для сообщений поддержки

        public List<UDbTable> TotalUsers { get; set; } // Свойство для списка пользователей
    }
}
