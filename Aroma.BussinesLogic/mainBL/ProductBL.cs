using Aroma.BussinesLogic.Core.Levels;
using Aroma.BussinesLogic.Interface;
using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Product;
using Aroma.Domain.Entities.Product.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.mainBL
{
    public class ProductBL : AdminAPI, IProduct
    {
        public ProductDataModel AddManyProducts()
        {
            return ActionAddMAnyProducts();
        }


        public RResponseData AdminAddAction(Product product)
        {
            return AddAdminActionProduct(product);
        }

    }
}
