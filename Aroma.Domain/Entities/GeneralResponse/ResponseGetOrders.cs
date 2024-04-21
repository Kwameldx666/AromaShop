using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseGetOrders
    {
        public bool Status { get; set; } // Успех операции
        public string Message { get; set; } // Сообщение, например, об ошибке

        public List<Order> Orders;

        public ResponseGetOrders()
        {
            Orders = new List<Order>(); // Инициализация списка для избежания NullReferenceException
        }


    }
}
