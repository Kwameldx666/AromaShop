using Aroma.Domain.Entities.Product.DBModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Aroma.Domain.Entities.User;

namespace Aroma.Domain.Entities.Rating
{
    public class RatingUdbTable
    {
        [Key]
        public int Id { get; set; }

        public string Feedback { get; set; }
        public int Rating { get; set; }

        // Внешний ключ для связи с таблицей продуктов
        public int ProductId { get; set; }

        public int UserId { get; set; }

        // Навигационное свойство к таблице продуктов
        /*[ForeignKey("ProductId")]*/
        public virtual ProductDbTable Product { get; set; }

/*        [ForeignKey("UserId")]*/
        public virtual UDbTable User { get; set; }
    }
}
