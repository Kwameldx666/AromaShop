using Aroma.Domain.Entities.Product.DBModel;
using Aroma.Domain.Entities.Rating;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.DBModel.Seed
{
    using System.Data.Entity;

    public class RatingContext : DbContext
    {
        public RatingContext() : base("name=Lab_TW")
        {
        }

        public virtual DbSet<RatingUdbTable> Rating { get; set; }
        public virtual DbSet<ProductDbTable> Products { get; set; } // DbSet для продуктов
        public virtual DbSet<UDbTable> Users { get; set; } // DbSet для пользователей

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Определение отношения "один ко многим" между продуктами и оценками
            modelBuilder.Entity<ProductDbTable>()
                .HasMany(p => p.Ratings)
                .WithRequired(r => r.Product)
                .HasForeignKey(r => r.ProductId);

            // Определение отношения "один ко многим" между пользователями и оценками
            modelBuilder.Entity<UDbTable>()
                .HasMany(u => u.Ratings)
                .WithRequired(r => r.User)
                .HasForeignKey(r => r.UserId);
        }
    }

}

