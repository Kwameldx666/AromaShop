using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseToEditProduct
    {
        public bool Status { get; set; } // Успех операции
        public string MessageError { get; set; } // Сообщение, например, об ошибке

   
    }
}
