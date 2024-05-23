using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.Domain.Entities.GeneralResponse
{
    public class ResponseUpdateQuantityOrders
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public List<Order> Orders;

        public Order OneOrder;
        public ResponseUpdateQuantityOrders()
        {
            Orders = new List<Order>(); // Инициализация списка для избежания NullReferenceException
        }
    }
}
