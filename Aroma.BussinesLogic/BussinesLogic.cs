using Aroma.BusinessLogic.Interface;
using Aroma.BusinessLogic.mainBL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BusinessLogic
{
    public class BusinessLogic
    {
        public ISupport GetSupport()
        {
            return new SupportBL();
        }

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
