using Aroma.Domain.Entities.GeneralResponse;
using Aroma.Domain.Entities.Product;
using Aroma.Domain.Entities.Product.DBModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aroma.BussinesLogic.Interface
{
    public interface IProduct
    {
        ProductDataModel AddManyProducts();

        RResponseData AdminAddAction(List<Product> product);

    }
}
