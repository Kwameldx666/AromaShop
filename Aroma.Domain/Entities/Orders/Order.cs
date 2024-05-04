using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aroma.Domain.Entities.Orders;
using Aroma.Domain.Entities.Product.DBModel;
using Aroma.Domain.Entities.Rating;
using Aroma.Domain.Enums.OrdersStatus;
namespace Aroma.Domain.Entities.User
{
    public class Order
    {
        public int OrderId { get; set; }
        public int UserId { get; set; }
        public int ProductId { get; set; }
        public int QuantityOrder { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalPrice { get; set; }
        public virtual ProductDbTable Product { get; set; } // Свойство для связи с продуктом
        public string ImageUrl { get; set; }
        public virtual UDbTable UDbTable { get; set; } // Свойство для связи с пользователем

        public virtual RatingUdbTable RatingUdbTable { get; set; }
        public string ProductType { get; set; }
        public OrderStatus orderStatus { get; set; }
        public string Feedback { get; set; }

        public int Reting { get; set; }
        public double AverageRating { get; set; }

        // Другие свойства заказа
    }
}
