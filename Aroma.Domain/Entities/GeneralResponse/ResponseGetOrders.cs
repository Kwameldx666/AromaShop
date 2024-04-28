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
        public decimal Price { get; set; }
        public string Description { get; set; }
        public int ProductId { get; set; }
        public string Name { get; set; }
        public int Quantity { get; set; }
        public string Category { get; set; }
        public string ImageUrl { get; set; }

        public List<Order> Orders;
        public string Feedback { get; set; }

        public int Reting { get; set; }
        public ResponseGetOrders()
        {
            Orders = new List<Order>(); // Инициализация списка для избежания NullReferenceException
        }


    }
}
