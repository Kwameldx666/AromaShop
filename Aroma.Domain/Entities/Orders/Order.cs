using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aroma.Domain.Entities.Product.DBModel;
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

        public virtual UDbTable UDbTable { get; set; } // Свойство для связи с пользователем
        public string ProductType { get; set; }
        // Другие свойства заказа
    }
}
