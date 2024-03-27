using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.DBModel.Seed
{
    public class UserContext:DbContext
    {
        public UserContext() : base("name=Lab_TW") // connection string name defined in your web.config
        {

        }

        public virtual DbSet<UDbTable> Users { get; set; }
    }
}
