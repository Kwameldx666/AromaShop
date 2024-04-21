using Aroma.Domain.Entities.Product.DBModel;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.DBModel.Seed
{
    public class OrderContext : DbContext
    {
        public OrderContext() : base("name=Lab_TW")
        {
        }

        public virtual DbSet<Order> Orders { get; set; }
        public virtual DbSet<UDbTable> Users { get; set; } // DbSet для пользователей
        public virtual DbSet<ProductDbTable> Products { get; set; } // DbSet для продуктов
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Вызываем метод OnModelCreating из базового класса для применения общих настроек
            base.OnModelCreating(modelBuilder);

            // Дополнительные настройки для отношений, если необходимо
            // Например, убедитесь, что используются настройки для сущности Order из UserContext
            modelBuilder.Entity<Order>()
                .HasRequired(o => o.UDbTable)
                .WithMany(u => u.Orders)
                .HasForeignKey(o => o.UserId);

            modelBuilder.Entity<Order>()
                .HasRequired(o => o.Product)
                .WithMany()
                .HasForeignKey(o => o.ProductId);
        }
    }
}

