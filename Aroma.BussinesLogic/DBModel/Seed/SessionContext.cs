using Aroma.Domain.Entities.User;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.DBModel.Seed
{
    public class SessionContext : DbContext
    {
        public SessionContext() : base("name=Lab_TW")
        {
        }

        public virtual DbSet<Session> Sessions { get; set; }
    }

}
