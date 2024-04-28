using Aroma.Domain.Entities.Product.DBModel;
using Aroma.Domain.Entities.Support;
using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.DBModel.Seed
{
    internal class SupportContext : DbContext
    {
        public SupportContext() : base("name=Lab_TW") // connection string name defined in your web.config
        {

        }

        public virtual DbSet<UDbTable> Users { get; set; }
        public virtual DbSet<USupportForm> SupportMesages { get; set; } // DbSet для продуктов

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<USupportForm>()
                .HasRequired(s => s.SupportUser) // Каждое сообщение должно иметь отправителя
                .WithMany(u => u.SupportMesages) // Один пользователь может отправить много сообщений
                .HasForeignKey(m => m.SupportUserId); // Внешний ключ в сообщении поддержки, который ссылается на ID отправителя в таблице пользователей

            base.OnModelCreating(modelBuilder);
        }

    }
}