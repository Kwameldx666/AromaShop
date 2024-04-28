using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;
using Aroma.Domain.Enums.OrdersStatus;

namespace Aroma.Domain.Entities.Orders
{
    public class Purchase
    {
        [Key]
        public int Id { get; set; }

        public int UserId { get; set; } // Внешний ключ для пользователя

        [ForeignKey("UserId")]
        public virtual Aroma.Domain.Entities.User.UDbTable User { get; set; } // Используем полное квалифицированное имя

        public int ProductId { get; set; } // Внешний ключ для товара

        [ForeignKey("ProductId")]
        public virtual Aroma.Domain.Entities.Product.DBModel.Product Product { get; set; } // Используем полное квалифицированное имя

        public DateTime PurchaseDate { get; set; }

        public int Quantity { get; set; }

        public  OrderStatus orderStatus { get;set; }
    }

}
