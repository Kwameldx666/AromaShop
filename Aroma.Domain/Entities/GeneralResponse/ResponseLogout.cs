using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseLogout
    {
        public bool Status { get; set; } // Успех операции
        public string Message { get; set; } // Сообщение, например, об ошибке
    }
}
