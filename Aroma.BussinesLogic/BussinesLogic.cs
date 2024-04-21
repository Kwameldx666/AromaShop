using Aroma.BussinesLogic.Interface;
using Aroma.BussinesLogic.mainBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic
{
    public class BussinesLogic
    {
        public ISession GetSessionBL()
        {
            return new SessionBL();
        }
        public IProduct AddProductBL()
        {
            return new ProductBL();
        }

        public IOrderService OrderServBL()
        {
            return new OrderBL();
        }
    }
}
