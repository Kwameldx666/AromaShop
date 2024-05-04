using Aroma.Domain.Entities.Product.DBModel;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Lab_TW.Models
{
    public class RatingData
    {
        public int Id { get; set; }

        public int Rating { get; set; }

        // Внешний ключ для связи с таблицей продуктов
        public int ProductId { get; set; }

        public int UserId { get; set; }

        public string Feedback { get;set; }

        // Навигационное свойство к таблице продуктов
        [ForeignKey("ProductId")]
        public virtual ProductDbTable Product { get; set; }

        [ForeignKey("UserId")]
        public virtual UDbTable User { get; set; }
    }
}