using Aroma.Domain.Entities.Product.DBModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.DBModel.Seed
{
    public class ProductContext : DbContext
    {
        public ProductContext() : base("name=Lab_TW")
        {
        }

        public virtual DbSet<ProductDbTable> Products { get; set; }
    }
};
